using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP_Generator
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void MySQL_Click(object sender, EventArgs e)
        {
            MySQL MySQL_ = new MySQL();
            MySQL_.Show();
        }

        private void SQL_Click(object sender, EventArgs e)
        {
            Form1 SQL_ = new Form1();
            SQL_.Show();
        }
    }
}
