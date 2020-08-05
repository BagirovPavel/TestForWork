using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Data.Common;

namespace ProjectLife
{
    public class DBConnect
    {
        SqlConnection sc = new SqlConnection();

        public DBConnect()
        {
            sc.ConnectionString = ConnectionString();
        }

        public bool CheckConnection()
        {
            try
            {
                sc.Open();
                sc.Close();
                return true;
            }
            catch (Exception e)
            {
                sc.Close();
                return false;
            }
        }

        public void Close()
        {
            sc.Close();
        }

        public void Open()
        {
            sc.Open();
        }

        private string ConnectionString()
        {
            string connectionString = @"Data Source=127.0.0.1;Initial Catalog=ProjectLife;Integrated Security=True; User ID=Test; Password=123";
            return connectionString;
        }

        public bool SaveLog(string str, string operType)
        {
            try
            {
                SqlCommand scm = new SqlCommand("exec iud_query_log '" + str + "', '" + operType + "'", sc);
                //sc.Open();
                scm.ExecuteNonQuery();
                //sc.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SaveGame(string str, Guid guid, int generation)
        {
            try
            {
                SqlCommand scm = new SqlCommand("exec iud_game_save 'I', '" + str + "', '" + guid + "', " + generation, sc);
                //sc.Open();
                scm.ExecuteNonQuery();
                //sc.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteGame(Guid guid)
        {
            try
            {
                SqlCommand scm = new SqlCommand("exec iud_game_save 'D', null, '" + guid + "', null", sc);
                //sc.Open();
                scm.ExecuteNonQuery();
                //sc.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public SqlDataReader LoadGame()
        {
            try
            {
                SqlCommand scm = new SqlCommand("select sgs.id, sgs.sequence, sgs.guid, sgs.[current_date], sgs.generation from sel_game_save() sgs", sc);
                //sc.Open();
                SqlDataReader reader = scm.ExecuteReader();
                //sc.Close();
                return reader;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SqlDataReader CheckSave(string sequence)
        {
            try
            {
                SqlCommand scm = new SqlCommand("select scs.code, scs.message from sel_check_save('" + sequence + "') scs", sc);
                //sc.Open();
                SqlDataReader reader = scm.ExecuteReader();
                //sc.Close();
                return reader;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SqlDataReader GetLogs()
        {
            try
            {
                SqlCommand scm = new SqlCommand("select sql.oper_type, sql.[current_date] from sel_query_log() sql", sc);
                //sc.Open();
                SqlDataReader reader = scm.ExecuteReader();
                //sc.Close();
                return reader;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
