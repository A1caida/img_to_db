using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace img_to_db
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        db con = new db("127.0.0.1", "root", "", "a1caida");
        public MainWindow()
        {
            InitializeComponent();

            idimg.ItemsSource = con.getTableInfoo("SELECT id,  name FROM img_kek").AsDataView();
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public static System.Drawing.Image ByteArrayToImage(MemoryStream a)
        {
           
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(a);
            return returnImage;
        }

        public void add(object sender, RoutedEventArgs e)
        {
            
            con.up(flop.Text);
            idimg.ItemsSource = con.getTableInfoo("SELECT id,  name FROM img_kek").AsDataView();
        }
        public void view(object sender, RoutedEventArgs e)
        {

            System.Drawing.Image a = con.upp(Convert.ToInt32(idimg.SelectedValue));
            var bitmap = new Bitmap(a);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            bitmapSource.Freeze();
            pick.Source = bitmapSource;
        }
    }
}
