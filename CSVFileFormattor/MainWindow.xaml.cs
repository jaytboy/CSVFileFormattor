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
using static System.Reflection.Metadata.BlobBuilder;
using System.Data;
using System.Diagnostics.Metrics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;

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
        private void bt_Tips_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(GetHint());
        }

        private static string GetHint()
        {
            Random rand = new Random();

            string[] hints = {"For where your treasure is, there your heart will be also.\n - Matthew 6:21",
                "Bring the whole tithe into the storehouse, that there may be food in my house.Test me in this,” says the LORD Almighty, “and see if I will not throw open the floodgates of heaven and pour out so much blessing that there will not be room enough to store it.\n - Malachi 3:10",
                "Whoever loves money never has enough; whoever loves wealth is never satisfied with their income.This too is meaningless.\n - Ecclesiastes 5:10",
                "Let no debt remain outstanding, except the continuing debt to love one another, for whoever loves others has fulfilled the law.\n - Romans 13:8",
                "Better the little that the righteous have than the wealth of many wicked; for the power of the wicked will be broken, but the LORD upholds the righteous.\n - Psalms 37:16 - 17",
                "Dishonest money dwindles away, but whoever gathers money little by little makes it grow.\n - Proverbs 13:11",
                "Keep your lives free from the love of money and be content with what you have, because God has said, “Never will I leave you; never will I forsake you.”\n - Hebrews 13:5",
                "“No one can serve two masters. Either you will hate the one and love the other, or you will be devoted to the one and despise the other.You cannot serve both God and money.\n - Matthew 6:24",
                "Then some soldiers asked him, “And what should we do?” He replied, “Don’t extort money and don’t accuse people falsely—be content with your pay.”\n - Luke 3:14",
                "For the love of money is a root of all kinds of evil.Some people, eager for money, have wandered from the faith and pierced themselves with many griefs.\n - 1 Timothy 6:10",
                "Command those who are rich in this present world not to be arrogant nor to put their hope in wealth, which is so uncertain, but to put their hope in God, who richly provides us with everything for our enjoyment. Command them to do good, to be rich in good deeds, and to be generous and willing to share.In this way they will lay up treasure for themselves as a firm foundation for the coming age, so that they may take hold of the life that is truly life.\n - 1 Timothy 6:17 - 19",
                "Sell your possessions and give to the poor.Provide purses for yourselves that will not wear out, a treasure in heaven that will never fail, where no thief comes near and no moth destroys.\n - Luke 12:33",
                "If anyone is poor among your fellow Israelites in any of the towns of the land the LORD your God is giving you, do not be hardhearted or tightfisted toward them.\n - Deuteronomy 15:7",
                "But when you give to the needy, do not let your left hand know what your right hand is doing, so that your giving may be in secret.Then your Father, who sees what is done in secret, will reward you.\n - Matthew 6:3 - 4",
                "Jesus sat down opposite the place where the offerings were put and watched the crowd putting their money into the temple treasury. Many rich people threw in large amounts. But a poor widow came and put in two very small copper coins, worth only a few cents.Calling his disciples to him, Jesus said, “Truly I tell you, this poor widow has put more into the treasury than all the others. They all gave out of their wealth; but she, out of her poverty, put in everything—all she had to live on.”\n - Mark 12:41 - 44",
                "Lazy hands make for poverty, but diligent hands bring wealth.\n - Proverbs 10:4",
                "“Suppose one of you wants to build a tower.Won’t you first sit down and estimate the cost to see if you have enough money to complete it?\n - Luke 14:28",
                "Wisdom is a shelter as money is a shelter, but the advantage of knowledge is this: Wisdom preserves those who have it.\n - Ecclesiastes 7:12",
                "“No one can serve two masters. Either you will hate the one and love the other, or you will be devoted to the one and despise the other.You cannot serve both God and money.” The Pharisees, who loved money, heard all this and were sneering at Jesus.He said to them, “You are the ones who justify yourselves in the eyes of others, but God knows your hearts. What people value highly is detestable in God’s sight.\n - Luke 16:13 - 15",
                "God said to Solomon, “Since this is your heart’s desire and you have not asked for wealth, possessions or honor, nor for the death of your enemies, and since you have not asked for a long life but for wisdom and knowledge to govern my people over whom I have made you king, therefore wisdom and knowledge will be given you.And I will also give you wealth, possessions and honor, such as no king who was before you ever had and none after you will have.”\n - 2 Chronicles 1:11 - 12",
                "The LORD will open the heavens, the storehouse of his bounty, to send rain on your land in season and to bless all the work of your hands.You will lend to many nations but will borrow from none.\n - Deuteronomy 28:12",
                "Honor the LORD with your wealth, with the firstfruits of all your crops;\n - Proverbs 3:9" };

            int tipNumber = rand.Next(hints.Length);

            return hints[tipNumber];
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