using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace FacturasGO
{
    public class CD_Operating
    {
        DBConnection connection = new DBConnection();

        MySqlDataReader read;
        DataTable table = new DataTable();
        MySqlCommand cmd = new MySqlCommand();

        // Mostrar todos los registros
        public DataTable Show()
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "ShowOperating";
            cmd.CommandType = CommandType.StoredProcedure;
            read = cmd.ExecuteReader();
            table.Load(read);
            connection.closeConnection();
            return table;
        }

        // Insertar registro
        public void Insert(string Place, string Client, DateTime Date, string Description, double Total)
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "InsertOperating";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Place", Place);
            cmd.Parameters.AddWithValue("p_Client", Client);
            cmd.Parameters.AddWithValue("p_Date", Date);
            cmd.Parameters.AddWithValue("p_Description", Description);
            cmd.Parameters.AddWithValue("p_Total", Total);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            connection.closeConnection();
        }

        // Editar registro
        public void Edit(int ID, string Place, string Client, DateTime Date, string Description, double Total)
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "UpdateOperating";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ID", ID);
            cmd.Parameters.AddWithValue("p_Place", Place);
            cmd.Parameters.AddWithValue("p_Client", Client);
            cmd.Parameters.AddWithValue("p_Date", Date);
            cmd.Parameters.AddWithValue("p_Description", Description);
            cmd.Parameters.AddWithValue("p_Total", Total);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            connection.closeConnection();
        }

        // Eliminar registro
        public void Delete(int ID)
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "DeleteOperating";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ID", ID);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            connection.closeConnection();
        }

        // Buscar por mes y año
        public DataTable Search(int Month, int Year)
        {
            cmd.Connection = connection.openConnection();
            cmd.CommandText = "SearchOperatingByMonthYear";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Month", Month);
            cmd.Parameters.AddWithValue("p_Year", Year);

            read = cmd.ExecuteReader();
            table.Load(read);
            cmd.Parameters.Clear();
            connection.closeConnection();
            return table;
        }
    }
}
