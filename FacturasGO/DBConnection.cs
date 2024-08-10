using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace FacturasGO
{
    internal class DBConnection
    {
        private SqlConnection connection = new SqlConnection("Server=almacendb;Database=ALMACENACCESORIOSBD;User=sa;Password=123;");
        public SqlConnection openConnection()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            return connection;
        }
        public SqlConnection closeConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
            return connection;
        }
    }
}
