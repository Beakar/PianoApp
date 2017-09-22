using System;
using System.IO;
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
using System.Drawing;
using System.Resources;
using System.Reflection;

namespace PianoMusic
{
    /*
     * Controls the Main functionality of the program
     *
     * @author Ben Rumptz
     * @version 1.0
     * 
     * PianoMusic allows the user to randomly select a piece of sheet music from given files. Useful for keeping practice interesting.
     * 
     * #Additional functionality to implement:
     * #   Multiple categories to randomize from
     * #   Music Image automatically resizes to acceptable size
     * #   User supplied sheet music resources
     * #   Other potential ways to spice up practice as I come up with them
     */
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            JazzButton.Click += Button_Click;
            PopButton.Click += Button_Click;
            LickButton.Click += Button_Click;
        }

        /* Randomizes the displayed sheet music when a Click event is generated */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //find out what button was pressed
            string thisButton = ((Button)sender).Content.ToString().ToLower();
            RandomizePicture(thisButton);
        }

        /* Creates a Bitmap[] from all of the files of type Bitmap in Resources.resx */
        //TODO: Understand this function completely so that it can be used
        private Bitmap[] GetResourceImages(string name)
        {
            //get all PropertyInfo from Resources.resx
            PropertyInfo[] props = typeof(Properties.Resources).GetProperties(BindingFlags.NonPublic | BindingFlags.Static);
            //gets all the images with that correspond with the given name
            var images = props.Where(prop => prop.PropertyType == typeof(Bitmap) && prop.Name.Contains(name)).Select(prop => prop.GetValue(null, null) as Bitmap).ToArray();
            //return the list of Bitmaps
            return images;
        }

        /* Randomly selects an image in Resources.resx to be displayed by the Image obj musicSheet */
        private void RandomizePicture(string buttonName)
        {
            //generate a list of all Bitmap in Resources.resx
            Bitmap[] images = GetResourceImages(buttonName);
            //if we find none then exit
            if (images == null || images.Length == 0)
            {
                //Nothing to do here...
                return;
            }

            //Calculate available range to use and generate random number in that range
            int maxValue = images.Length;
            Random r = new Random();
            int idx = r.Next(maxValue);
            //convert Bitmap to something that can be displayed and then display it via the musicSheet Image obj.
            //using hardcoded numbers here so I can get the proof of concept done
            musicSheet.Height = 650;
            musicSheet.Width = 750;
            this.musicSheet.Source = ConvertToBitmapSource(images[idx]);
        }

        /* Used for converting Bitmaps to BitmapSource so that we can display it as an image */
        public BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
            (
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            return bitmapSource;
        }
    }
}
