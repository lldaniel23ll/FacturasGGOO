using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FacturasGO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string idOperating = null;
        private bool editOperating = false;
        private void clearForm()
        {
            txtPlace.Clear();
            txtClient.Clear();
            txtDescription.Clear();
            txtTotal.Clear();
        }
        private void Insert()
        {
            CB_Operating objectCB = new CB_Operating();
            if (editOperating == false)
            {
                try
                {
                    DateTime getDate = dateTimePicker1.Value;
                    objectCB.insertOperating(txtPlace.Text, txtClient.Text, getDate.ToString(), txtDescription.Text, txtTotal.Text);
                    MessageBox.Show("Se inserto correctamente!");
                    showOperating();
                    txtPlace.Focus();
                    clearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo insertar!\n" + ex.Message);
                }
            }
            if (editOperating == true)
            {
                try
                {
                    DateTime getDate = dateTimePicker1.Value;
                    objectCB.editOperating(idOperating, txtPlace.Text, txtClient.Text, getDate.ToString(), txtDescription.Text, txtTotal.Text);
                    MessageBox.Show("Se edito correctamente!");
                    showOperating();
                    txtPlace.Focus();
                    editOperating = false;
                    clearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo editar!\n" + ex.Message);
                }
            }
            
        }
        private void showOperating()
        {
            CB_Operating obj = new CB_Operating();
            dataGridView1.DataSource = obj.showOperating();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            showOperating();
            txtPlace.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Insert();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                editOperating = true;
                idOperating = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                txtPlace.Text = dataGridView1.CurrentRow.Cells["Lugar"].Value.ToString();
                txtClient.Text = dataGridView1.CurrentRow.Cells["Cliente"].Value.ToString();
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    object dateCellValue = dataGridView1.CurrentRow.Cells["Fecha"].Value;
                    if (dateCellValue != null && dateCellValue != DBNull.Value)
                    {
                        if (DateTime.TryParse(dateCellValue.ToString(), out DateTime dateSelected))
                            dateTimePicker1.Value = dateSelected;
                        else
                            MessageBox.Show("La fecha no tiene un formato válido.");
                    }
                    else
                        MessageBox.Show("La celda de fecha está vacía o no contiene un valor válido.");
                }
                txtDescription.Text = dataGridView1.CurrentRow.Cells["Descripcion"].Value.ToString();

                txtTotal.Text = dataGridView1.CurrentRow.Cells["Total"].Value.ToString();
                
            }
            else
                MessageBox.Show("Seleccione toda la fila");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                CB_Operating objectCB = new CB_Operating();
                idOperating = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                objectCB.deleteOperating(idOperating);
                MessageBox.Show("Se elimino correctamente!");
                showOperating();
            }
            else
                MessageBox.Show("Seleccione toda la fila");

            txtPlace.Focus();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.Show();
        }
    }
}
