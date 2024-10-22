using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace FacturasGO
{
    public class DBConnection
    {
        private MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ALMACENACCESORIOS;Uid=root;Pwd=12345;");

        public MySqlConnection openConnection()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            return connection;
        }

        public MySqlConnection closeConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
            return connection;
        }
    }
}
