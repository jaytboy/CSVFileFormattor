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
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Security.Permissions;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Principal;
using System.Globalization;
using CsvHelper;
using System.Windows.Media.Animation;
using CsvHelper.Configuration;

// TODO:
// - Add right click easter egg that gives random messages.

namespace CSVFileFormattor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bt_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
                Filter = "CSV Files|*.csv"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                try
                {
                    this.tb_FolderLocation.Text = System.IO.Path.GetFullPath(fileName);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }
        }
        private void bt_Process_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var reader = new StreamReader(tb_FolderLocation.Text);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Context.RegisterClassMap<TransactionMap>();
                var records = csv.GetRecords<Transaction>().ToList();

                string folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";

                string fileday = records[records.Count() - 1].PostDate.ToString("dd");
                string filemonth = records[records.Count() - 1].PostDate.ToString("MMM");
                string fileyear = records[records.Count() - 1].PostDate.ToString("yyyy");
                string filename = GetUniqueName(folder, $"gb-{fileyear}{filemonth}{fileday}Transactions.csv");

                using StreamWriter sw = new(filename);
                sw.WriteLine("Date,Name,Amount");
                foreach (Transaction record in records)
                {
                    if (record.Debit == null)
                    {
                        sw.WriteLine($"{record.PostDate:MM/dd/yyyy},{record.Description},{record.Credit}");
                    }
                    else
                    {
                        sw.WriteLine($"{record.PostDate:MM/dd/yyyy},{record.Description},-{record.Debit}");
                    }
                }
                this.tbl_Notification.Text = $"{System.IO.Path.GetFileName(filename)} - Created Successfully!";
                this.tb_FolderLocation.Text = "";
            }
            catch (CsvHelper.TypeConversion.TypeConverterException e1)
            {
                MessageBox.Show(e1.Message);
                throw;
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured!");
            }
        }
        private static string GetUniqueName(string folder, string name)
        {
            string validName = System.IO.Path.Combine(folder, name);
            string namewoExt = System.IO.Path.GetFileNameWithoutExtension(validName);
            string extension = System.IO.Path.GetExtension(validName);
            int copyNumber = 1;
            while (File.Exists(validName))
            {
                validName = $"{namewoExt}({copyNumber++}).{extension}";
            }
            return validName;
        }
    }
    public class Transaction
    {
        public string? AccountNumber { get; set; }
        public DateTime PostDate { get; set; }
        public string? Check { get; set; }
        public string? Description { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public string? Status { get; set; }
        public double? Balance { get; set; }
    }
    public sealed class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Map(m => m.AccountNumber).Name("Account Number");
            Map(m => m.PostDate).Name("Post Date");
            Map(m => m.Check);
            Map(m => m.Description);
            Map(m => m.Debit);
            Map(m => m.Credit);
            Map(m => m.Status).Ignore();
            Map(m => m.Balance).Ignore();
        }
    }
}