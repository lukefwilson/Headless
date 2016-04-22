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

namespace Headless
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            List<Experience> experiences = new List<Experience>();
            experiences.Add(new Experience("link 1"));
            experiences.Add(new Experience("link 12"));
            experiences.Add(new Experience("link 13"));
            Library.ItemsSource = experiences;
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