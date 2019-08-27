using System;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using java.lang;
using System.Diagnostics;
namespace TUF
{
    public partial class Form1 : Form
    {
        
       
        int check = 0;
       static ArrayList totalExtensions = new ArrayList(new string[] { "jpg", "png", "mp3", "mp4", "gif", "zip", "rar", "mpg", "msi", "wav", "doc", "docx", "ppt", "pptx", "pdf", "xls", "xlsx" });
        public static ArrayList history=new ArrayList();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Keyword.ReadOnly = true;
            type.ReadOnly = true;
            LinkID.ReadOnly = true;
            Browse.Enabled = false;
            Search.Enabled = false;
            download.Enabled = false;


        }

        private void Search_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            if (check == 0)
            {
                MessageBox.Show("Kindly Select a Type of Search");
            }
            string keyword = Keyword.Text.ToLower();
            if (check == 1)
            {

                crawler.crawlerinit();
                crawler.searchByKeyword(keyword, listBox1);
                LinkID.ReadOnly = false;
                Browse.Enabled = true;
                Keyword.ReadOnly = true;
                Search.Enabled = false;

                label6.Text = "Search Option";
            }
            if (check == 2)
            {

                string k = Keyword.Text.ToLower();
                string t = type.Text.ToLower();
                if (totalExtensions.GetRange(10, 7).Contains(t))
                {
                    crawler.crawlerinit();
                    crawler.searchByKeywordtype(k, t, listBox1);
                    LinkID.ReadOnly = false;
                    Browse.Enabled = true;
                    Keyword.ReadOnly = true;
                    Search.Enabled = false;
                    type.ReadOnly = true;
                    label6.Text = "Search Option";
                }
                else
                {
                    MessageBox.Show("Invalid Type to search with.");
                }



            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void keywordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            check = 1;
            label6.Text = "Search By Keyword";
            Keyword.ReadOnly = false;
            Search.Enabled = true;

        }

        private void keywordTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            check = 2;
           
            label6.Text = "Search By Keyword & Type";
            Keyword.ReadOnly = false;
            type.ReadOnly = false;
            Search.Enabled = true;
        }

        private void downloadTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            check = 3;
            type.ReadOnly = false;
            download.Enabled = true;
            label6.Text = "Download files of specified type";
        }

        private void download_Click(object sender, EventArgs e)
        {
            string t = type.Text.ToLower();
            crawler.crawlerinit();
            if (totalExtensions.GetRange(10, 7).Contains(t))
            {
                crawler.DownloadType(t);
            }
            else
            {
                MessageBox.Show("Invalid Type to search with.");
            }
            type.ReadOnly = true;
            download.Enabled = false;
            label6.Text = "Search Option";

        }

        private void Browse_Click(object sender, EventArgs e)
        {
            int linkNo = Convert.ToInt32(LinkID.Text);
            if (linkNo >= 0)
            {
                System.Diagnostics.Process.Start(crawler.hyperLinks[linkNo].ToString());
                history.Add(crawler.hyperLinks[linkNo].ToString());

            }
            else
            {
                MessageBox.Show("Invalid Number");
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
           
            

        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            History h = new History();
            h.Show();
        }
    }
}
