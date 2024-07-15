using BarLight.Common;
using BarLight.Model;
using BarLight.Pages;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace BarLight.Windows
{
    /// <summary>
    /// Логика взаимодействия для W_ImageConvert.xaml
    /// </summary>
    public partial class W_ImageConvert : Window
    {
        private ImageSource _image;
        public ImageSource Image { get => _image; set { _image = value; } }
        public W_ImageConvert()
        {
            InitializeComponent();
        }

        byte[] imageArray;

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                // Set filter for file extension and default file extension 
                DefaultExt = ".jpg",
                Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|Любые файлы|*.*"
            };


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result.HasValue && result.Value)
            {
                // Open document 
                string filename = dlg.FileName;
                txtBlockChoosenImageName.Text = filename;
                imageArray = File.ReadAllBytes(filename);


                imgConverted.Source = Common.ImageConverter.ImageFromBuffer(imageArray);

                //BitmapImage bitmap = new BitmapImage();
                //bitmap.BeginInit();
                //bitmap.UriSource = new Uri(dlg.FileName, UriKind.Relative);
                ////bitmap.CacheOption = BitmapCacheOption.OnLoad;
                //bitmap.EndInit();
                //Image = bitmap;
                //txtBoxByteArray.Text = Common.ImageConverter.BufferFromImage(bitmap).ToString();
            }
        }

        private void btnLoadToDB_Click(object sender, RoutedEventArgs e)
        {
            DbSet<FoodMenu> foodMenu = Manager.Context.FoodMenu;
            int id = int.Parse(txtBoxID.Text);
            try
            {
                foodMenu.Where(fm => fm.ID == id).FirstOrDefault().Image = imageArray;
                Manager.Context.SaveChanges();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());

            }

            imgConvertedFromDB.Source = Common.ImageConverter.ImageFromBuffer(foodMenu.Where(fm => fm.ID == id).FirstOrDefault().Image);
        }

        private void btnShowLoadedImage_Click(object sender, RoutedEventArgs e)
        {
            DbSet<FoodMenu> foodMenu = Manager.Context.FoodMenu;
            int id = int.Parse(txtBoxID.Text);
            imgShownFromDB.Source = Common.ImageConverter.ImageFromBuffer(foodMenu.Where(fm => fm.ID == id).FirstOrDefault().Image);
            txtBlockName.Text = foodMenu.Where(fm => fm.ID == id).FirstOrDefault().ID + "; " + foodMenu.Where(fm => fm.ID == id).FirstOrDefault().Name;
        }
    }
}
