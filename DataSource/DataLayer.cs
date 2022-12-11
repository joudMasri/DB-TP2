using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataSource
{
    class DataLayer
    {
        string server_name;
        string database_name;
        SqlConnection con;
        public bool Valid = false;
        public DataLayer(string ServerName, string DataBaseName)
        {
            server_name = ServerName;
            database_name = DataBaseName;
            VerifyConnection();
        }

        private void VerifyConnection()
        {
            con = new SqlConnection(@"Data Source=" + server_name + ";Initial Catalog=" + database_name + ";Integrated Security=True");
            try
            {
                con.Open();
                con.Close();
                Valid = true;
            }
            catch
            {
                Valid = false;
            }
        }

        internal bool IsValid()
        {
            throw new NotImplementedException();
        }

        public void SetServerName(string ServerName)
        {
            server_name = ServerName;
            VerifyConnection();
        }
        public string GetServerName()
        {
            return server_name;
        }
        public void SetDataBaseName(string DataBaseName)
        {
            database_name = DataBaseName;
            VerifyConnection();
        }
        public string GetDataBaseName()
        {
            return database_name;
        }
        
        public int ExecuteActionCommand(string CommandText)
        {
            int rep = 0;
            
            if ((Valid) && (CommandText.Length>0))
            {
                con.Open();                
                SqlCommand com = new SqlCommand(CommandText, con);
                try
                {
                    rep = com.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message);
                }
                con.Close();
            }
            return rep;
        }
        public object GetValue(string SqlText)
        {
            if ((Valid) &&(SqlText.Length>0))
            {
                object v=null;
                SqlCommand com = new SqlCommand(SqlText, con);
                con.Open();
                try
                {
                    v = com.ExecuteScalar();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message);
                }
                con.Close();
                return v;
            }
            else return null;
        }
        public DataTable GetData(string SqlText, string name)
        {
            DataTable dt = new DataTable(); 
            if (Valid)
            {
                SqlCommand com = new SqlCommand(SqlText, con);
                com.CommandType = CommandType.Text;
                SqlDataAdapter data_adapter = new SqlDataAdapter(com);
                con.Open();
                try
                {
                    data_adapter.Fill(dt);
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message);
                    dt = null;
                }
                con.Close();
                dt.TableName = name;
                return dt;
            }
            else return null;
        }
    }
}
