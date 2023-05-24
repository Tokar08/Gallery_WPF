using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gallery_WPF
{
    /// <summary>
    /// Логика взаимодействия для PictureInformation.xaml
    /// </summary>
    public partial class PictureInformation : Window
    {

        public PictureInformation(int Width, int Height, string Path)
        {
            InitializeComponent();

            DateTime date = GetImageCreationDate(Path);
            long size = GetImageSize(Path);

            PictureName.Content = "Picture path: " + Path;
            PictureWidth.Content = "Picture width: " + Width;
            PictureHeight.Content = "Picture height: " + Height;
            PictureDateCreation.Content = "Picture data: "+ date;
            PictureSize.Content = "Picture size: " + size + " B";

        }
        private DateTime GetImageCreationDate(string IPath)
        {
            FileInfo fileInfo = new FileInfo(IPath);
            DateTime creationDate = fileInfo.CreationTime;
            return creationDate;
        }

        private long GetImageSize(string imagePath)
        {
            FileInfo fileInfo = new FileInfo(imagePath);
            long sizeInBytes = fileInfo.Length;
            return sizeInBytes;
        }


        private void Button_Click(object sender, RoutedEventArgs e) => Close();

      
    }
}
