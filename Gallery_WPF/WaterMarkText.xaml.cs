using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gallery_WPF
{
    /// <summary>
    /// Логика взаимодействия для WaterMarkText.xaml
    /// </summary>
    public partial class WaterMarkText : Window
    {
        
        public WaterMarkText()
        {
            InitializeComponent();
            

         
    
        }
        public string? MarkText;
        private void btnSave_Click(object sender, RoutedEventArgs e) 
        {
            MarkText = tbWaterMark.Text;

            if (string.IsNullOrEmpty(MarkText))
            {
                System.Windows.Forms.MessageBox.Show("No value entered", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
              
            }
            else
            {
                Close();
            }
        }


    }
}
