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
using System.Net;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;

using System.Data;
using System.Drawing;
using System.Diagnostics;
using LiteDB;
using System.Runtime.InteropServices;

namespace Headless
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Process vrApp;
        public Process experience;

        public MainWindow()
        {
            InitializeComponent();

            List<Experience> experiences = new List<Experience>();
            experiences.Add(new Experience("link 1"));
            experiences.Add(new Experience("link 12"));
            experiences.Add(new Experience("link 13"));
            Library.ItemsSource = experiences;

        }




        private void P_Exited(object sender, EventArgs e)
        {

            Console.WriteLine("Experience Closed");
            BringToFront(this.vrApp);

        }

        private void P_VRExited(object sender, EventArgs e)
        {

            Console.WriteLine("VR App closed");

        }


        private void URLField_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void DownloadProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        private void SearchExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Downloading " + e.Parameter);

            Experience download = new Experience(e.Parameter.ToString());

            e.Handled = true;
        }

        private void Library_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Start_VR_App_Click(object sender, RoutedEventArgs e)
        {
            this.vrApp = Process.Start("C:/Users/hwray/Downloads/ovr_sdk_win_1.3.0_public/OculusSDK/Samples/OculusRoomTiny/OculusRoomTiny (DX11)/Bin/Windows/Win32/Debug/VS2015/OculusRoomTiny (DX11).exe");
            this.vrApp.Exited += P_VRExited;
            this.vrApp.EnableRaisingEvents = true;
        }

        private void Start_Experience_Click(object sender, RoutedEventArgs e)
        {
            this.experience = Process.Start("C:/Users/hwray/Documents/Visual Studio 2015/Projects/Headless/Headless/bin/Debug/experiences/het/WindowsNoEditor/TestProject.exe");
            this.experience.Exited += P_Exited;
            this.experience.EnableRaisingEvents = true;
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private void BringToFront(Process pTemp)
        {
            SetForegroundWindow(pTemp.MainWindowHandle);
        }
    }
}

/*

            // Open database (or create if not exits)
            using (var db = new LiteDatabase(@"data.db"))
            {
                // Get customer collection
                var customers = db.GetCollection<Customer>("customers");

                // Create your new customer instance
                var customer = new Customer
                {
                    Name = "John Doe",
                    Phones = new string[] { "8000-0000", "9000-0000" },
                    IsActive = true
                };

                // Insert new customer document (Id will be auto-incremented)
                customers.Insert(customer);

                // Update a document inside a collection
                customer.Name = "Joana Doe";

                customers.Update(customer);

                // Index document using a document property
                customers.EnsureIndex(x => x.Name);

                // Use Linq to query documents
                var results = customers.Find(x => x.Name.StartsWith("Jo"));
            }
*/