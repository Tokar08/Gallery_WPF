using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using FileCombine;
using System.Windows.Documents;
using System.Security.Cryptography;
using System.CodeDom;
using System.Resources;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Gallery_WPF
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();

    }

    public class Album
    {

        public Border? activeBorder = null;
        public Border border = new();
        public string[] filePatterns = { };
        public System.Windows.Controls.Image MainImage;

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

     
        public void AddPatternForImageSearch(params string[] newPattern)
        {
            var patterns = filePatterns.ToList();
            foreach (var pattern in newPattern)
            {
                patterns.Add(pattern);
            }
            filePatterns = patterns.ToArray();
        }
        public void RemovePatternForImageSearch(params string[] newPattern)
        {
            var patterns = filePatterns.ToList();
            foreach (var pattern in newPattern)
            {
                patterns.Remove(pattern);
            }
            filePatterns = patterns.ToArray();
        }

        public void SelectImage(object sender, System.Windows.Controls.Image PreviewImage)
        {
            border = (Border)sender;
            if (border != null)
            {
                if (activeBorder != null)
                    activeBorder.BorderBrush = new SolidColorBrush(Colors.White);

                activeBorder = border;
                border.BorderBrush = new SolidColorBrush(Colors.DarkOliveGreen);

                if (border.Child is System.Windows.Controls.Image image)
                    PreviewImage.Source = image.Source;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        public void SetWallpaper(string path, int style, int tile)
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
            registry.SetValue("WallpaperStyle", style.ToString());
            registry.SetValue("TileWallpaper", style.ToString());

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
        public void DisplayImages(string path)
        {

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path);
            image.EndInit();
            image.Freeze();

            MainImage = new()
            {
                Source = image
            };

            border = new Border()
            {
                Child = MainImage,

                MinWidth = 150,
                BorderThickness = new Thickness(3, 3, 3, 3),
                Margin = new Thickness(10, 15, 10, 15),
            };

        }

        public void AddWatermark(string imagePath, string watermarkText, string outputPath, int FontSize, string FontFamily)
        {
            BitmapImage image = new BitmapImage(new Uri(imagePath));

            // Создаем новый BitmapSource с размерами изображения
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(image, new Rect(0, 0, image.PixelWidth, image.PixelHeight));

                // Настраиваем параметры текста водяного знака
                FormattedText formattedText = new FormattedText(watermarkText, CultureInfo.CurrentCulture,FlowDirection.LeftToRight,new Typeface(FontFamily), FontSize, System.Windows.Media.Brushes.Gray);

                // Расстояние от края изображения до водяного знака
               

                // ВЛ
                drawingContext.DrawText(formattedText, new System.Windows.Point(10, 10));

                // ВП
                double topRightX = image.PixelWidth - formattedText.Width - 10;
                drawingContext.DrawText(formattedText, new System.Windows.Point(topRightX, 10));

                // НЛ
                double bottomLeftY = image.PixelHeight - formattedText.Height - 10;
                drawingContext.DrawText(formattedText, new System.Windows.Point(10, bottomLeftY));

                // НП
                double bottomRightX = image.PixelWidth - formattedText.Width - 10;
                double bottomRightY = image.PixelHeight - formattedText.Height - 10;
                drawingContext.DrawText(formattedText, new System.Windows.Point(bottomRightX, bottomRightY));
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
            image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY, PixelFormats.Pbgra32);

            renderTargetBitmap.Render(drawingVisual);

            using (FileStream fileStream = new FileStream(outputPath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                encoder.Save(fileStream);
            }
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

    }

    public class AddPatternOnCommand : ICommand
    {

        private Album album = new();
        public AddPatternOnCommand(Album albom) => this.album = albom;
        public void Execute() => album.AddPatternForImageSearch("*.jpeg", "*.jpg", "*.png", "*.PNG", "*.gif", "*.ico");
        public void Undo() => album.Exit();

    }
    public class RemovePatternOnCommand : ICommand
    {

        private Album album = new();
        public RemovePatternOnCommand(Album album) => this.album = album;
        public void Execute() => album.RemovePatternForImageSearch();
        public void Undo() => album.Exit();

    }
    public class DisplayOnCommand : ICommand
    {
        private Album album = new();
        private string? path;

        public DisplayOnCommand(Album album, string? path) { this.album = album; this.path = path; }
        public void Execute() => album.DisplayImages(path);

        public void Undo() => album.Exit();
    }
    public class SelectOnCommand : ICommand
    {
        private Album album = new();
        private object? sender;
        private System.Windows.Controls.Image Image = new();
        public SelectOnCommand(Album album, object sender, System.Windows.Controls.Image image) { this.album = album; this.Image = image; this.sender = sender; }

        public void Execute() => album.SelectImage(sender, Image);
        public void Undo() => album.Exit();

    }


    public class SetWallpaperOnCommand : ICommand
    {
       private Album? album = new();
       private string? Path;
       private int Style;
       private int Tile;


        public SetWallpaperOnCommand(Album album,string path,int style, int tile)
        {
            this.album = album;
            this.Path = path;
            this.Style = style;
            this.Tile = tile;   
        }
        public void Execute() => album.SetWallpaper(Path, Style, Tile);
        public void Undo() => album.Exit();


    }

    public class AddWatermarkOnCommand: ICommand 
    {
        private Album? album = new();
        private string? ImagePath;
        private string? Watermark;
        private string? OutpuPath;
        private int fontSize;
        private string fontFamily;

        public AddWatermarkOnCommand(Album album, string imagePath, string watermarkText, string outputPath, int FontSize, string FontFamily)
        {
            this.album = album;
            this.ImagePath = imagePath;
            this.Watermark = watermarkText;
            this.OutpuPath = outputPath;
            this.fontSize = FontSize;
            this.fontFamily = FontFamily;  
        }

        public void Execute() => album.AddWatermark(ImagePath, Watermark, OutpuPath, fontSize, fontFamily);
        public void Undo() => album.Exit();
    }




    public class Gallery
    {
        public ICommand command;

        public Gallery() { }

        public void SetCommans(ICommand com) => this.command = com;

        public void Execute() => command.Execute();

        public void Undo() => command.Undo();

    }
}
