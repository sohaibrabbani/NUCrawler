using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TUF
{
    public partial class History : Form
    {
        public History()
        {
            InitializeComponent();
            if (Form1.history != null)
            {
                foreach (string str in Form1.history)
                {
                    listBox1.Items.Add(str);
                }
            }
            else {
                MessageBox.Show("No previous History Found");
            }
        }

        private void History_Load(object sender, EventArgs e)
        {

        }
    }
}
