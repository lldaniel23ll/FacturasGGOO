using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using System.Diagnostics;
using System.Net;
using System.Runtime.Remoting.Contexts;

namespace FacturasGO
{
    public class CD_Operating
    {
        DBConnection connection = new DBConnection();

        SqlDataReader read;
        DataTable table = new DataTable();
        SqlCommand cmd = new SqlCommand();

        public DataTable Show()
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "ShowOperatingAll";
            cmd.CommandType = CommandType.StoredProcedure;
            read = cmd.ExecuteReader();
            table.Load(read);
            connection.closeConnection();
            return table;
        }
        public void Insert(string Place, string Client, DateTime Date, string Description, double Total)
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "InsertOperating";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Place", Place);
            cmd.Parameters.AddWithValue("@Client", Client);
            cmd.Parameters.AddWithValue("@Date", Date);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Total", Total);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        public void Edit(int ID, string Place, string Client, DateTime Date, string Description, double Total)
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "UpdateOperating";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@Place", Place);
            cmd.Parameters.AddWithValue("@Client", Client);
            cmd.Parameters.AddWithValue("@Date", Date);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Total", Total);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        public void Delete(int ID)
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "DeleteOperating";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", ID);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        public DataTable Search(int Month, int Year)
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "SearchOperatingByMonthYear";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Month", Month);
            cmd.Parameters.AddWithValue("@Year", Year);

            read = cmd.ExecuteReader();
            table.Load(read);
            cmd.Parameters.Clear();
            connection.closeConnection();
            return table;
        }
    }
}
