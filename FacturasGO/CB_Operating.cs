using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacturasGO
{
    public class CB_Operating
    {
        private CD_Operating objectCD = new CD_Operating();
        public DataTable showOperating()
        {
            DataTable table = new DataTable();
            table = objectCD.Show();
            return table;
        }
        public void insertOperating(string Place, string Client, string Date, string Description, string Total)
        {
            objectCD.Insert(Place, Client, Convert.ToDateTime(Date), Description, Convert.ToDouble(Total));
        }
        public void editOperating(string ID, string Place, string Client, string Date, string Description, string Total)
        {
            objectCD.Edit(Convert.ToInt32(ID), Place, Client, Convert.ToDateTime(Date), Description, Convert.ToDouble(Total));
        }
        public void deleteOperating(string ID)
        {
            objectCD.Delete(Convert.ToInt32(ID));
        }
        public DataTable searchOperating(string Month, string Year)
        {
            DataTable table = new DataTable();
            table = objectCD.Search(Convert.ToInt32(Month), Convert.ToInt32(Year));
            return table;
        }
    }
}
