using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP_Generator
{
    public partial class Form1 : Form
    {
        private SP_utils obj_utils = new SP_utils();

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

        }

        private List<string> tables = new List<string>();


        private void CheckBox_Checked(object sender, EventArgs e)
        {
            CheckBox chk = (sender as CheckBox);
            chk.Enabled = false;
            if (chk.Checked)
            {
                tables.Add(chk.Text);
                //MessageBox.Show("You selected: " + chk.Text);
            }
        }

        

        private void btn_generateSP_Click(object sender, EventArgs e)
        {
            string str = "";
            string conn = TB_connection_str1.Text.ToString();

            foreach (var table_name in tables)
            {
                dynamic json_table_columns = obj_utils.FetchTableColumns(conn, table_name);

                dynamic column_props = JsonConvert.DeserializeObject<dynamic>(json_table_columns);

                string primary_key = obj_utils.GetPrimaryKey(conn, table_name);

                string para_list = "@IU_Flag char \n,";
                string col_list = "";
                string val_list = "";
                string updateParas = "";

                foreach (var column_prop in column_props)
                {
                    para_list += "@" + column_prop["name"] + " " + column_prop["type"] + ",\n";

                    if (column_prop["name"]!= primary_key && column_prop["type"] != "timestamp" && column_prop["type"] != "time" && column_prop["type"] != "smalldatetime")
                    {
                        col_list += column_prop["name"] + ",";
                        val_list += "@" + column_prop["name"] + ",";
                        updateParas += column_prop["name"] + " = @" + column_prop["name"] + ",";
                    }
                }

                para_list = para_list.TrimEnd('\n');
                para_list = para_list.TrimEnd(',');
                col_list = col_list.TrimEnd(',');
                val_list = val_list.TrimEnd(',');
                updateParas = updateParas.TrimEnd(',');


                obj_utils.prepare_SP(primary_key, para_list, col_list,val_list, updateParas, table_name);


                //str += table_name + ",";


            }

            //MessageBox.Show("Selected Tables : " + str);
            string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            MessageBox.Show("Procedures location : " + project_path + "/SP_Files", "Message");
        }

        private void btn_connectDB_Click(object sender, EventArgs e)
        {
            string conn = TB_connection_str1.Text.ToString();
            string cmd = "SELECT * FROM INFORMATION_SCHEMA.TABLES";

            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd, conn))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    //Loop and add CheckBoxes to FloyLayoutPanel.
                    foreach (DataRow row in dt.Rows)
                    {
                        CheckBox chk = new CheckBox();
                        chk.Width = 200;
                        chk.Text = row["TABLE_NAME"].ToString();
                        chk.CheckedChanged += new EventHandler(CheckBox_Checked);
                        flowLayoutPanel1.Controls.Add(chk);
                    }

                }
                btn_generateSP.Visible = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Connecting Error Message");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }

    }
}
