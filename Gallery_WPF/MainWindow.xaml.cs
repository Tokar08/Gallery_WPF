using FileCombine;
using Shell32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Gallery_WPF
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly System.Windows.Forms.FolderBrowserDialog fbd;
        public readonly List<string> list = new();
        public readonly Album albom = new();
        private readonly Finder finder = new();
        private readonly Gallery gallery = new();
        private readonly Shell shell = new Shell();
        private readonly List<string> Favorites = new();
    

        public MainWindow()
        {

            InitializeComponent();

            fbd = new System.Windows.Forms.FolderBrowserDialog();

            gallery.SetCommans(new AddPatternOnCommand(albom));
            finder.FileMasks = albom.filePatterns;


            MySliderLight.ValueChanged += MySliderLight_ValueChanged;
            MySliderDark.ValueChanged += MySliderDark_ValueChanged;
            ContextMenuRemoveToFavorites.Visibility = Visibility.Collapsed;
            MySliderDark.Visibility = Visibility.Collapsed;
            MySliderLight.Visibility = Visibility.Collapsed;
            DarkLabel.Visibility = Visibility.Collapsed;
            LightLabel.Visibility = Visibility.Collapsed;
        }

       

        // Open the folder browser dialog and select a directory
        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                finder.FindFiles(fbd.SelectedPath);

                foreach (var file in finder.Container.Files)
                {
                    list.Add(file.FullName);
                }
            }
        

            stack.Children.Clear();
            MySliderDark.Visibility = Visibility.Visible;
            MySliderLight.Visibility = Visibility.Visible;
            DarkLabel.Visibility = Visibility.Visible;
            LightLabel.Visibility = Visibility.Visible;

            list.ForEach(path =>
            {
                gallery.SetCommans(new DisplayOnCommand(albom, path));
                gallery.Execute();
                albom.border.MouseDown += Border_MouseDown;
                stack.Children.Add(albom.border);
                albom.border.Style = FindResource("ActiveImageStyle") as Style;
            });
         
        }

        // Border settings
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                gallery.SetCommans(new SelectOnCommand(albom, sender, imgPreview));
                gallery.Execute();

            }
        }

        // FullScreen mode
        private void MenuFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (albom.activeBorder == null)
                return;

            FullScreenWindow fsWin = new(list, stack.Children.IndexOf(albom.activeBorder));

            fsWin.ShowDialog();

            gallery.SetCommans(new SelectOnCommand(albom, stack.Children[fsWin.Id], imgPreview));
            gallery.Execute();
        }


        // Deleting a file
        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {

            FullScreenWindow fsWin = new FullScreenWindow(list, stack.Children.IndexOf(albom.activeBorder));

            imgPreview.Source = null;
            Folder folder = shell.NameSpace(10);

            folder.MoveHere(list[fsWin.Id]);

            list.Remove(list[fsWin.Id]);
            stack.Children.Remove(albom.activeBorder);

        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e) => MenuDelete_Click(sender, e);

        // Logic with adding / removing from favorites
        private void ContextMenuAddToFavorites_Click(object sender, RoutedEventArgs e)
        {
            FullScreenWindow fsWin = new(list, stack.Children.IndexOf(albom.activeBorder));
            FileInfo file = new FileInfo(list[fsWin.Id]);
            Favorites.Add(file.FullName);
            MessageBox.Show(file.Name + "  has been added to favorites", "Successfully", MessageBoxButton.OK, MessageBoxImage.Asterisk);

            string folderPath = Path.GetDirectoryName(file.FullName);
            string favoritesFolderPath = Path.Combine(folderPath, "Favorites");
            Directory.CreateDirectory(favoritesFolderPath);
            string newImagePath = Path.Combine(favoritesFolderPath, Path.GetFileName(file.FullName));
            File.Copy(file.FullName, newImagePath);

        }
        private void ContextMenuRemoveFromFavorites_Click(object sender, RoutedEventArgs e)
        {
            FullScreenWindow fsWin = new(Favorites, stack.Children.IndexOf(albom.activeBorder));
            FileInfo file = new FileInfo(Favorites[fsWin.Id]);
            MessageBox.Show(file.Name + " has been removed from favorites", "Successfully", MessageBoxButton.OK, MessageBoxImage.Asterisk);

            imgPreview.Source = null;
            stack.Children.RemoveAt(fsWin.Id);
            Favorites.Remove(Favorites[fsWin.Id]);

            string folderPath = Path.GetDirectoryName(file.FullName);
            string favoritesPath = Path.Combine(folderPath + "\\Favorites");
            string deletePath = Path.Combine(favoritesPath, Path.GetFileName(file.FullName));
            File.Delete(deletePath);

            if (Directory.GetFileSystemEntries(favoritesPath).Length == 0)
            {
               Directory.Delete(favoritesPath, true);
            }

        }

        // Drag and drop implementation
        private void ScrollViewer_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                System.Windows.Controls.Image ImageContainer = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(files[0])) };

                gallery.SetCommans(new DisplayOnCommand(albom, ImageContainer.Source.ToString()));
                gallery.Execute();

                albom.border.MouseDown += Border_MouseDown;
                stack.Children.Add(albom.border);

                foreach (string file in files)
                {
                    list.Add(file);
                }
            }
        }
        private void ScrollViewer_DragOver(object sender, System.Windows.DragEventArgs e) => e.Handled = true;
        private void ScrollViewer_DragEnter(object sender, DragEventArgs e) => e.Effects = DragDropEffects.Copy;


        //Add water mark to picture
        private void ContextAddWaterMark_Click(object sender, RoutedEventArgs e)
        {
            FullScreenWindow fsWin = new FullScreenWindow(list, stack.Children.IndexOf(albom.activeBorder));
            WaterMarkText waterMark = new WaterMarkText();
            waterMark.ShowDialog();

            gallery.SetCommans(new AddWatermarkOnCommand(albom, list[fsWin.Id], waterMark.MarkText, list[fsWin.Id], 15, "Yu Gothic Medium"));
            gallery.Execute();

        }

        // Configuration
        public void FavoriteConfiguration() 
        {
            menuShowFavorites.IsChecked = true;
            ContextMenuDelete.Visibility = Visibility.Collapsed;
            ContextMenuAddToFavorites.Visibility = Visibility.Collapsed;
            menuFullScreen.Visibility = Visibility.Collapsed;
            menuDelete.Visibility = Visibility.Collapsed;
            ContextMenuRemoveToFavorites.Visibility = Visibility.Visible;
            MySliderDark.Visibility = Visibility.Collapsed;
            MySliderLight.Visibility = Visibility.Collapsed;
            DarkLabel.Visibility = Visibility.Collapsed;
            LightLabel.Visibility = Visibility.Collapsed;
            ContextPictureInformation.Visibility = Visibility.Collapsed;
            ContextAddWaterMark.Visibility = Visibility.Collapsed;
            ContextMenuSetWallpaper.Visibility = Visibility.Collapsed;
            ttAdd.Visibility = Visibility.Collapsed;
            ttCarousel.Visibility = Visibility.Collapsed;   
            ttDelete.Visibility = Visibility.Collapsed;
            ttFull.Visibility = Visibility.Collapsed;
            ttInfo.Visibility = Visibility.Collapsed;
            ttInfo.Visibility = Visibility.Collapsed;
            ttOpen.Visibility = Visibility.Collapsed;
            ttSet.Visibility = Visibility.Collapsed;
            ttDisplayF.Visibility = Visibility.Collapsed;
        }
        public void UnFavoriteConfiguration()
        {
            menuShowFavorites.IsChecked = false;
            ContextMenuDelete.Visibility = Visibility.Visible;
            ContextMenuAddToFavorites.Visibility = Visibility.Visible;
            menuFullScreen.Visibility = Visibility.Visible;
            menuDelete.Visibility = Visibility.Visible;
            ContextMenuRemoveToFavorites.Visibility = Visibility.Collapsed;
            MySliderDark.Visibility = Visibility.Visible;
            MySliderLight.Visibility = Visibility.Visible;
            DarkLabel.Visibility = Visibility.Visible;
            LightLabel.Visibility = Visibility.Visible;
            ContextPictureInformation.Visibility = Visibility.Visible;
            ContextAddWaterMark.Visibility = Visibility.Visible;
            ContextMenuSetWallpaper.Visibility = Visibility.Visible;
            ttAdd.Visibility = Visibility.Visible;
            ttCarousel.Visibility = Visibility.Visible;
            ttDelete.Visibility = Visibility.Visible;
            ttFull.Visibility = Visibility.Visible;
            ttInfo.Visibility = Visibility.Visible;
            ttInfo.Visibility = Visibility.Visible;
            ttOpen.Visibility = Visibility.Visible;
            ttSet.Visibility = Visibility.Visible;
            ttDisplayF.Visibility = Visibility.Visible;
        }

        // Demonstration of added files in "Favorites"
        private void menuShowFavorites_Checked(object sender, RoutedEventArgs e)
        {
            FavoriteConfiguration();

            imgPreview.Source = null;
            stack.Children.Clear();

            Favorites.ForEach(path =>
            {
                gallery.SetCommans(new DisplayOnCommand(albom, path));
                gallery.Execute();

                albom.border.MouseDown += Border_MouseDown;

                stack.Children.Add(albom.border);

            });
        }
        private void menuShowFavorites_Unchecked(object sender, RoutedEventArgs e)
        {

            UnFavoriteConfiguration();
            imgPreview.Source = null;
            stack.Children.Clear();


            list.ForEach(path =>
            {
                gallery.SetCommans(new DisplayOnCommand(albom, path));
                gallery.Execute();

                albom.border.MouseDown += Border_MouseDown;

                stack.Children.Add(albom.border);

            });

        }

        //Set up on the desktop
        private void ContextMenuSetWallpaper_Click(object sender, RoutedEventArgs e)
        {
            FullScreenWindow fsWin = new FullScreenWindow(list, stack.Children.IndexOf(albom.activeBorder));
            gallery.SetCommans(new SetWallpaperOnCommand(albom, list[fsWin.Id], 2, 0));
            gallery.Execute();
        }

        // Picture Information
        private void ContextPictureInformation_Click(object sender, RoutedEventArgs e)
        {

            FullScreenWindow fsWin = new FullScreenWindow(list, stack.Children.IndexOf(albom.activeBorder));

            BitmapFrame bitmapFrame = BitmapDecoder.Create(new Uri(list[fsWin.Id]), BitmapCreateOptions.None, BitmapCacheOption.Default).Frames[0];

            PictureInformation picture = new PictureInformation(bitmapFrame.PixelWidth, bitmapFrame.PixelHeight, list[fsWin.Id]);

            picture.ShowDialog();
        }

        // All theme variations
        private void MenuDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("DarkTheme.xaml", UriKind.Relative);

            if (System.Windows.Application.LoadComponent(uri) is ResourceDictionary dic)
            {
                Resources.Clear();
                Resources.MergedDictionaries.Add(dic);
            }
        }
        private void MenuLightTheme_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("LightTheme.xaml", UriKind.Relative);

            if (System.Windows.Application.LoadComponent(uri) is ResourceDictionary dic)
            {
                Resources.Clear();
                Resources.MergedDictionaries.Add(dic);
            }

        }
        private void MenuGradientTheme_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("GradientTheme.xaml", UriKind.Relative);

            if (System.Windows.Application.LoadComponent(uri) is ResourceDictionary dic)
            {
                Resources.Clear();
                Resources.MergedDictionaries.Add(dic);
            }

        }


        // Dark brightness 
        private void MySliderDark_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FullScreenWindow fsWin = new(list, stack.Children.IndexOf(albom.activeBorder));
            MySliderDark.Value = e.NewValue;
            DimmingChanger dimming = new DimmingChanger();
            dimming.Change(list[fsWin.Id], imgPreview, MySliderDark.Value);
        }


        // Light brightness 
        private void MySliderLight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FullScreenWindow fsWin = new(list, stack.Children.IndexOf(albom.activeBorder));
            MySliderLight.Minimum = 1; MySliderLight.Maximum = 10;
            MySliderLight.Value = e.NewValue;
            LighteningChanger lighteningVChanger = new LighteningChanger();
            lighteningVChanger.Change(list[fsWin.Id], imgPreview, MySliderLight.Value);
        }

        // Keybinding
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(bool)menuShowFavorites.IsChecked) 
            {
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.O) { MenuOpen_Click(sender, e); }
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.F) { MenuFullScreen_Click(sender, e); }
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.LWin) { ContextMenuSetWallpaper_Click(sender, e); }
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.I) { ContextPictureInformation_Click(sender, e); }
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.W) { ContextAddWaterMark_Click(sender, e); }


                if (e.Key == Key.F1) { MenuLightTheme_Click(sender, e); }
                if (e.Key == Key.F2) { MenuDarkTheme_Click(sender, e); }
                if (e.Key == Key.F3) { MenuGradientTheme_Click(sender, e); }
                if (e.Key == Key.Escape) { MenuExit_Click(sender, e); }
                if (e.Key == Key.Delete) { MenuDelete_Click(sender, e); }
                if (e.Key == Key.C) { menuShowFavorites_Checked(sender, e); }
                if (e.Key == Key.V) { menuShowFavorites_Unchecked(sender, e); }
            }

            if (e.Key == Key.F1) { MenuLightTheme_Click(sender, e); }
            if (e.Key == Key.F2) { MenuDarkTheme_Click(sender, e); }
            if (e.Key == Key.F3) { MenuGradientTheme_Click(sender, e); }
            if (e.Key == Key.Escape) { MenuExit_Click(sender, e); }
            if (e.Key == Key.V) { menuShowFavorites_Unchecked(sender, e); }

        }


        // Exit from application
        private void MenuExit_Click(object sender, RoutedEventArgs e) => gallery.Undo();

        private void menuFeedback_Click(object sender, RoutedEventArgs e)
        {
            string pathURL = "https://www.youtube.com/watch?v=8ifQ2mVUEcU";
            Process.Start(new ProcessStartInfo
            {
                FileName = pathURL,
                UseShellExecute = true
            });
        }
    }
}







