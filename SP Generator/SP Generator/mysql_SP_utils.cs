﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Generator
{   
    class mysql_SP_utils
    {
        
        public dynamic mysql_FetchTableColumns(string conn, string table_name)
        {

            string cmd = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + table_name + "'";

            List<dynamic> columns = new List<dynamic>();

            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd, conn))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);


                //Loop and add CheckBoxes to FloyLayoutPanel.
                foreach (DataRow row in dt.Rows)
                {
                    dynamic data = new ExpandoObject();
                    data.name = row["COLUMN_NAME"];
                    //data.type = row["DATA_TYPE"];
                    data.type = row["COLUMN_TYPE"];

                    columns.Add(data);
                }
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(columns);

            return json;

        }

        public bool prepare_SP(string primary_key, string para_list, string col_list, string val_list, string updateParas, string table_name)
        {
            string content = Prepare_SP_Base();
            content = Prepare_paramerters(content, para_list);

            bool select = prepare_sp_select(content, col_list, table_name);
            bool insert = prepare_sp_insert(content, col_list, val_list, table_name);
            bool update = prepare_sp_update(primary_key, content, col_list, updateParas, table_name);
            bool delete = prepare_sp_delete(primary_key, content, col_list, table_name);

            return true;
        }



        private string Prepare_SP_Base()
        {
            string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

            string content = File.ReadAllText(project_path + @"\SP_Templates\mysql_sp_template.txt");            

            return content;
        }

        private string Prepare_paramerters(string content, string para_list)
        {
            content = content.Replace("{{sp_parameters}}", para_list);
            return content;
        }

        private bool prepare_sp_select(string content, string col_list, string tableName)
        {
            try
            {
                string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

                string select = content;
                select = select.Replace("{{sp_query}}", "-- SELECT " + col_list + " FROM " + tableName + " \nSELECT * FROM " + tableName+";");
                select = select.Replace("{{sp_name}}", "sp_" + tableName + "_FETCH");
                File.WriteAllText(project_path + @"\SP_Files\sp_" + tableName + "_FETCH.sql", select);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool prepare_sp_insert(string content, string col_list, string val_list, string tableName)
        {
            try
            {
                string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

                string insert = content;
                insert = insert.Replace("{{sp_query}}", "INSERT INTO " + tableName + "(" + col_list + ") VALUES(" + val_list + ");");
                insert = insert.Replace("{{sp_name}}", "sp_" + tableName + "_INSERT");
                File.WriteAllText(project_path + @"\SP_Files\sp_" + tableName + "_INSERT.sql", insert);

                return true;
            }
            catch
            {
                return false;
            }

        }

        private bool prepare_sp_delete(string primary_key, string content, string col_list, string tableName)
        {
            try
            {
                string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

                string delete = content;

                delete = delete.Replace("{{sp_query}}", "DELETE FROM  " + tableName + prepare_primary_where(primary_key) + ";");
                delete = delete.Replace("{{sp_name}}", "sp_" + tableName + "_DELETE");
                File.WriteAllText(project_path + @"\SP_Files\sp_" + tableName + "_DELETE.sql", delete);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool prepare_sp_update(string primary_key, string content, string col_list, string updateParas, string tableName)
        {
            try
            {
                string project_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

                string update = content;

                update = update.Replace("{{sp_query}}", "UPDATE " + tableName + " SET " + updateParas + prepare_primary_where(primary_key) + ";");

                update = update.Replace("{{sp_name}}", "sp_" + tableName + "_UPDATE");
                File.WriteAllText(project_path + @"\SP_Files\sp_" + tableName + "_UPDATE.sql", update);

                return true;
            }
            catch
            {
                return false;
            }

        }

        private string prepare_primary_where(string primary_key)
        {
            string where = "";

            if (primary_key != "NA")
                where = " WHERE " + primary_key + "=" + primary_key;

            return where;
        }
        
        public string mysql_GetPrimaryKey(string conn, string table_name)
        {            
            string cmd = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
                "WHERE TABLE_NAME = '"+ table_name + "' " +
                "AND COLUMN_KEY = 'PRI'";

            List<string> columns = new List<string>();

            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd, conn))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    columns.Add(row["COLUMN_NAME"].ToString());
                }
            }

            if (columns.Count > 0)
                return columns[0];
            else
                return "NA";

        }
    }
}