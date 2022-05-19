using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using Newtonsoft.Json;

namespace SP_Generator
{
    public partial class MySQL : Form
    {
        private mysql_SP_utils obj_utils = new mysql_SP_utils();
        private List<string> tables = new List<string>();

        public MySQL()
        {
            InitializeComponent();
        }

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

        private void ConnectDB_Click(object sender, EventArgs e)
        {
            string server = txt_server.Text.ToString();
            string database = txt_database.Text.ToString();
            string username = txt_username.Text.ToString();
            string password = txt_password.Text.ToString();
            string port = txt_port.Text.ToString();

            string connStr = "server="+server+";user="+username+";database="+database+";port="+port+";password="+password;
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // Perform database operations
                string query = "SELECT table_name FROM information_schema.tables WHERE table_type = 'base table' AND table_schema='" + database + "'";



                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                    
                        CheckBox chk = new CheckBox();
                        chk.Width = 200;
                        chk.Text = rdr[0].ToString();
                        chk.CheckedChanged += new EventHandler(CheckBox_Checked);
                        flowLayoutPanel1.Controls.Add(chk);
                    
                }
                rdr.Close();

                txt_connection_str.Text = connStr;
                btn_generateSP.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Connecting Error Message");
            }
            finally
            {
                conn.Close();
                //MessageBox.Show("Done.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
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

        private void btn_generateSP_Click(object sender, EventArgs e)
        {
            string str = "";
            string conn = txt_connection_str.Text.ToString();
            //conn = "Data Source=localhost;Initial Catalog=ampmusicbank_mercedes;User ID=root;Password=;Connect Timeout=300; pooling=true; Max Pool Size=200;";

            foreach (var table_name in tables)
            {
                dynamic json_table_columns = obj_utils.mysql_FetchTableColumns(conn, table_name);

                dynamic column_props = JsonConvert.DeserializeObject<dynamic>(json_table_columns);

                string primary_key = obj_utils.mysql_GetPrimaryKey(conn, table_name);

                string para_list = "IN IU_Flag char \n,";
                string col_list = "";
                string val_list = "";
                string updateParas = "";

                foreach (var column_prop in column_props)
                {
                    para_list += "IN " + column_prop["name"] + " " + column_prop["type"] + ",\n";

                    if (column_prop["name"] != primary_key && column_prop["type"] != "timestamp" && column_prop["type"] != "time" && column_prop["type"] != "smalldatetime")
                    {
                        col_list += "`"+column_prop["name"] + "`,";
                        val_list += "" + column_prop["name"] + ",";
                        updateParas += "`"+column_prop["name"] + "` =  " + column_prop["name"] + ",";
                    }
                }

                para_list = para_list.TrimEnd('\n');
                para_list = para_list.TrimEnd(',');
                col_list = col_list.TrimEnd(',');
                val_list = val_list.TrimEnd(',');
                updateParas = updateParas.TrimEnd(',');


                obj_utils.prepare_SP(primary_key, para_list, col_list, val_list, updateParas, table_name);


                //str += table_name + ",";


            }

            //MessageBox.Show("Selected Tables : " + str);
            string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            MessageBox.Show("Procedures location : " + project_path + "/SP_Files", "Message");
        }
    }
}
