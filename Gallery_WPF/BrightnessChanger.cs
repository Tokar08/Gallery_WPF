using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

using System.IO;

namespace Gallery_WPF
{
    interface IChanger
    {
        void Change(string pathToPicture, Image image, double Multiplier);
    }

     public class LighteningChanger : IChanger
    {
        public void Change(string pathToPicture, Image image, double Multiplier) 
        {
            BitmapImage imgMain = new BitmapImage(new Uri(pathToPicture));
            FormatConvertedBitmap formatConverted = new FormatConvertedBitmap(imgMain, PixelFormats.Pbgra32, null, 0);

            int pixelWidth = formatConverted.PixelWidth;
            int pixelHeight = formatConverted.PixelHeight;
            int stride = pixelWidth * 4; 
            byte[] pixels = new byte[pixelHeight * stride];
            formatConverted.CopyPixels(pixels, stride, 0);


            for (int i = 0; i < pixelHeight; i++)
            {
                for (int j = 0; j < pixelWidth; j++)
                {
                    int index = i * stride + j * 4;
                    byte transparency = pixels[index + 3];

                    if (transparency == 0)
                        return;


                    byte red = pixels[index + 2];
                    byte green = pixels[index + 1];
                    byte blue = pixels[index];


                    byte adjustedRed = (byte)Math.Min(255, red * Multiplier);
                    byte adjustedGreen = (byte)Math.Min(255, green * Multiplier); ;
                    byte adjustedBlue = (byte)Math.Min(255, blue * Multiplier); ;

                    pixels[index + 2] = adjustedRed;
                    pixels[index + 1] = adjustedGreen;
                    pixels[index] = adjustedBlue;

                }
            }

          
            WriteableBitmap writeable = new WriteableBitmap(pixelWidth, pixelHeight, formatConverted.DpiX, formatConverted.DpiY, PixelFormats.Pbgra32, null);
            writeable.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), pixels, stride, 0);

          
            image.Source = writeable;

            string originalFolderPath =Path.GetDirectoryName(pathToPicture);
            string outputFolderPath = Path.Combine(originalFolderPath, "After lighting in gallery");
            Directory.CreateDirectory(outputFolderPath);

            string outputFileName = Path.GetFileNameWithoutExtension(pathToPicture) + " changed in Gallery" + Path.GetExtension(pathToPicture);


            string outputPath = System.IO.Path.Combine(outputFolderPath, outputFileName);

            using (var stream = new FileStream(outputPath, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(writeable));
                encoder.Save(stream);
            }
        }  
    }


    public class DimmingChanger : IChanger
    {
        public void Change(string pathToPicture, Image image, double Multiplier)
        {
            BitmapImage imgMain = new BitmapImage(new Uri(pathToPicture));
            FormatConvertedBitmap formatConverted = new FormatConvertedBitmap(imgMain, PixelFormats.Pbgra32, null, 0);
            WriteableBitmap writeable = new WriteableBitmap(formatConverted.PixelWidth, formatConverted.PixelHeight, formatConverted.DpiX, formatConverted.DpiY, PixelFormats.Pbgra32, null);

        
            byte[] pixels = new byte[formatConverted.PixelWidth * formatConverted.PixelHeight * 4];
            formatConverted.CopyPixels(pixels, formatConverted.PixelWidth * 4, 0);

         
            for (int i = 0; i < pixels.Length; i += 4)
            {
                byte red = pixels[i + 2];
                byte green = pixels[i + 1];
                byte blue = pixels[i];

                byte newRed = (byte)Math.Max(0, red - Multiplier);
                byte newGreen = (byte)Math.Max(0, green - Multiplier);
                byte newBlue = (byte)Math.Max(0, blue - Multiplier);

        
                pixels[i + 2] = newRed;
                pixels[i + 1] = newGreen;
                pixels[i] = newBlue;
            }

            writeable.WritePixels(new Int32Rect(0, 0, formatConverted.PixelWidth, formatConverted.PixelHeight), pixels, formatConverted.PixelWidth * 4, 0);

         
            image.Source = writeable;

            string originalFolderPath = Path.GetDirectoryName(pathToPicture);
            string outputFolderPath = Path.Combine(originalFolderPath, "After dimming in gallery");
            Directory.CreateDirectory(outputFolderPath);

            string outputFileName = Path.GetFileNameWithoutExtension(pathToPicture) + " changed in Gallery" + Path.GetExtension(pathToPicture);

    
            string outputPath = Path.Combine(outputFolderPath, outputFileName); 

            using (FileStream stream = new FileStream(outputPath, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(writeable));
                encoder.Save(stream);
            }
        }
    }

  
}
