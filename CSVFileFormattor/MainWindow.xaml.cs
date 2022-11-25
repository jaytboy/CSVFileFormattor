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
                catch (Exception)
                {
                }
            }
        }

        private void bt_Process_Click(object sender, RoutedEventArgs e)
        {
            List<StringDictionary> rows = new List<StringDictionary>();
            using (TextFieldParser reader = Microsoft.VisualBasic.FileIO.FileSystem.OpenTextFieldParser(this.tb_FolderLocation.Text, ","))
            {
                if (reader != null)
                {
                    int i = 0;
                    string[]? currentRow = new string[8];
                    string[] headers = new string[8];

                    while (!reader.EndOfData)
                    {
                        try
                        {
                            StringDictionary row = new StringDictionary();
                            currentRow = reader.ReadFields();
                            if (i == 0)
                            {
                                for (int j = 0; j < currentRow.Length; j++)
                                {
                                    headers[j] = currentRow[j].ToString();
                                }
                            }
                            else
                            {
                                for (int j = 0; j < currentRow.Length; j++)
                                {
                                    row.Add(headers[j], currentRow[j].ToString());
                                }
                                rows.Add(row);
                            }
                        }
                        catch (MalformedLineException)
                        {
                            MessageBox.Show("Line " + e.Source + "is not valid and will be skipped.");
                            throw;
                        }
                        i++;
                    }
                }
                else
                {
                    MessageBox.Show("File contains no data!");
                }
            }

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string filemonth = Convert.ToDateTime(rows[0]["Post Date"]).ToString("MMM");
            string fileyear = Convert.ToDateTime(rows[0]["Post Date"]).ToString("yyyy");
            string filename = $"{folder}\\gb-{fileyear}{filemonth}Transactions.csv";

            if (!File.Exists(filename))
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("Date,Name,Amount");
                    foreach (StringDictionary row in rows)
                    {
                        if (row["Debit"] == "")
                        {
                            sw.WriteLine($"{row["Post Date"]},{row["Description"]},{row["Credit"]}");
                        }
                        else
                        {
                            sw.WriteLine($"{row["Post Date"]},{row["Description"]},-{row["Debit"]}");
                        }
                    }
                }
                MessageBox.Show("File Processed!");
                this.tb_FolderLocation.Text = "";
            } else
            {
                MessageBox.Show("File already exists!\nPlease select a new file.");
                this.tb_FolderLocation.Text = "";
            }
        }
    }
}
