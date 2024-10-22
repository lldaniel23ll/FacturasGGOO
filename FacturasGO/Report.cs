using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight.Drawing;

namespace FacturasGO
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            CB_Operating objectCB = new CB_Operating();
            dataGridView1.DataSource = objectCB.searchOperating(cmbMonth.Text, cmbYear.Text);
            btnPDF.Enabled = true;
            btnExcel.Enabled = true;
        }

        private void Report_Load(object sender, EventArgs e)
        {
            cmbMonth.SelectedIndex = 0;
            cmbYear.SelectedIndex = 0;
            btnPDF.Enabled = false;
            btnExcel.Enabled = false;
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "Gastost Operativos del mes " + cmbMonth.Text + " y del año " + cmbYear.Text + ".pdf";
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";

                

                string HTML = Properties.Resources.pdf.ToString();


                string rows = string.Empty;
                decimal total = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    rows += "<tr>";
                    rows += "<td>" + row.Cells["Lugar"].Value.ToString() + "</td>";
                    rows += "<td>" + row.Cells["Cliente"].Value.ToString() + "</td>";

                    if (row.Cells["Fecha"].Value != null)
                    {
                        string fullDate = row.Cells["Fecha"].Value.ToString();
                        string[] hourdate = fullDate.Split(' ');
                        string date = hourdate[0];
                        string formattedDate = date.Contains("/") ? date : $"{date.Split('/')[0]}/{date.Split('/')[1]}/{date.Split('/')[2]}";
                        rows += "<td>" + formattedDate + "</td>";
                    }
                    else
                    {
                        rows += "<td></td>";
                    }

                    rows += "<td>" + row.Cells["Descripcion"].Value.ToString() + "</td>";
                    rows += "<td>" + row.Cells["Total"].Value.ToString() + "</td>";
                    rows += "</tr>";
                    total += decimal.Parse(row.Cells["Total"].Value.ToString());
                }


                HTML = HTML.Replace("@Filas", rows);
                HTML = HTML.Replace("@Total", total.ToString());

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(new Phrase(""));

                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Properties.Resources.LOGO_removebg, System.Drawing.Imaging.ImageFormat.Png);
                        image.ScaleToFit(80, 60);
                        image.Alignment = iTextSharp.text.Image.UNDERLYING;
                        image.SetAbsolutePosition(pdfDoc.LeftMargin, pdfDoc.Top - 40);
                        pdfDoc.Add(image);

                        using (StringReader reader = new StringReader(HTML))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                        }
                        pdfDoc.Close();
                        stream.Close();
                        MessageBox.Show("Datos exportados exitosamente!");
                    }
                        
                }
            }
            else
                MessageBox.Show("No hay datos para exportar.");
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "Gastos Operativos del mes " + cmbMonth.Text + " y del año " + cmbYear.Text + ".xlsx";
                saveFileDialog.Filter = "Excel Files|*.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SLDocument sl = new SLDocument();
                    SLPicture logoPic = new SLPicture("..\\..\\Resources\\LOGO-removebg-mini.png");
                    logoPic.SetPosition(0, 0);
                    sl.InsertPicture(logoPic);
                    sl.SetRowHeight(1, 60);

                    sl.MergeWorksheetCells("B1", "E1");
                    sl.SetCellValue("B1", "ALMACEN DE ACCESORIOS - GG.OO.");

                    // Cambiar los encabezados al nuevo orden
                    sl.SetCellValue(2, 1, "Fecha");
                    sl.SetCellValue(2, 2, "Cliente");
                    sl.SetCellValue(2, 3, "Lugar");
                    sl.SetCellValue(2, 4, "Descripcion");
                    sl.SetCellValue(2, 5, "Total");

                    sl.SetColumnWidth(1, 20);
                    sl.SetColumnWidth(2, 20);
                    sl.SetColumnWidth(3, 20);
                    sl.SetColumnWidth(4, 20);
                    sl.SetColumnWidth(5, 12);

                    SLStyle centeredStyle = new SLStyle();
                    centeredStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
                    centeredStyle.Alignment.Vertical = VerticalAlignmentValues.Center;
                    centeredStyle.Alignment.WrapText = true;

                    SLStyle borderStyle = new SLStyle();
                    borderStyle.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                    borderStyle.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                    borderStyle.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                    borderStyle.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;

                    SLStyle headerStyle = new SLStyle();
                    headerStyle.Font.FontSize = 20;
                    headerStyle.Font.Bold = true;
                    headerStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
                    headerStyle.Alignment.Vertical = VerticalAlignmentValues.Center;
                    headerStyle.Alignment.WrapText = true;
                    headerStyle.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Orange, System.Drawing.Color.Orange);
                    headerStyle.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                    headerStyle.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                    headerStyle.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                    headerStyle.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                    sl.SetCellStyle("B1", headerStyle);

                    SLStyle orangeBackgroundStyle = new SLStyle();
                    orangeBackgroundStyle.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Orange, System.Drawing.Color.Orange);

                    // Aplicar el estilo de fondo naranja, borde y centrado a las celdas combinadas y adyacentes
                    for (int col = 1; col <= 5; col++)
                    {
                        sl.SetCellStyle(1, col, orangeBackgroundStyle);
                        sl.SetCellStyle(1, col, borderStyle);
                        sl.SetCellStyle(1, col, centeredStyle);
                    }

                    // Crear un estilo para la fila de encabezados (negrita, centrado, borde)
                    SLStyle headerRowStyle = new SLStyle();
                    headerRowStyle.Font.Bold = true;
                    headerRowStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
                    headerRowStyle.Alignment.Vertical = VerticalAlignmentValues.Center;
                    headerRowStyle.Alignment.WrapText = true;
                    headerRowStyle.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                    headerRowStyle.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                    headerRowStyle.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                    headerRowStyle.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;

                    // Aplicar el estilo de borde y centrado a la fila de encabezados
                    for (int col = 1; col <= 5; col++)
                    {
                        sl.SetCellStyle(2, col, headerRowStyle);
                    }

                    int rowIndex = 3; // Comenzar en la fila 3 para los datos 
                    double totalTotal = 0; // Variable para la suma de Total

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["Fecha"].Value != null)
                        {
                            string fullDate = row.Cells["Fecha"].Value.ToString();
                            string[] hourdate = fullDate.Split(' ');
                            string date = hourdate[0];
                            string formattedDate = date.Contains("/") ? date : $"{date.Split('/')[0]}/{date.Split('/')[1]}/{date.Split('/')[2]}";
                            sl.SetCellValue(rowIndex, 1, formattedDate);
                            sl.SetCellStyle(rowIndex, 1, borderStyle);
                            sl.SetCellStyle(rowIndex, 1, centeredStyle);
                        }
                        if (row.Cells["Cliente"].Value != null)
                        {
                            sl.SetCellValue(rowIndex, 2, row.Cells["Cliente"].Value.ToString());
                            sl.SetCellStyle(rowIndex, 2, borderStyle);
                            sl.SetCellStyle(rowIndex, 2, centeredStyle);
                        }
                        if (row.Cells["Lugar"].Value != null)
                        {
                            sl.SetCellValue(rowIndex, 3, row.Cells["Lugar"].Value.ToString());
                            sl.SetCellStyle(rowIndex, 3, borderStyle);
                            sl.SetCellStyle(rowIndex, 3, centeredStyle);
                        }
                        if (row.Cells["Descripcion"].Value != null)
                        {
                            sl.SetCellValue(rowIndex, 4, row.Cells["Descripcion"].Value.ToString());
                            sl.SetCellStyle(rowIndex, 4, borderStyle);
                            sl.SetCellStyle(rowIndex, 4, centeredStyle);
                        }
                        if (row.Cells["Total"].Value != null)
                        {
                            double totalValue;
                            if (double.TryParse(row.Cells["Total"].Value.ToString(), out totalValue))
                            {
                                sl.SetCellValue(rowIndex, 5, totalValue);
                                sl.SetCellStyle(rowIndex, 5, borderStyle);
                                sl.SetCellStyle(rowIndex, 5, centeredStyle);
                                totalTotal += totalValue; // Sumar el valor al total
                            }
                        }

                        rowIndex++;
                    }

                    // Definir el estilo para las celdas del total
                    SLStyle totalStyle = new SLStyle();
                    totalStyle.Font.Bold = true;
                    totalStyle.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Yellow, System.Drawing.Color.Yellow);
                    totalStyle.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                    totalStyle.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                    totalStyle.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                    totalStyle.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                    totalStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
                    totalStyle.Alignment.Vertical = VerticalAlignmentValues.Center;
                    totalStyle.Alignment.WrapText = true;

                    // Escribir "Total:" en la primera columna y combinar las celdas de B a D
                    sl.SetCellValue(rowIndex, 1, "Total");
                    sl.SetCellStyle(rowIndex, 1, totalStyle); // Aplicar estilo
                    sl.MergeWorksheetCells(rowIndex, 2, rowIndex, 4); // Combinar celdas de B a D
                    sl.SetCellStyle(rowIndex, 2, totalStyle); // Aplicar estilo a las celdas combinadas
                    sl.SetCellStyle(rowIndex, 3, totalStyle); // Aplicar estilo a las celdas combinadas
                    sl.SetCellStyle(rowIndex, 4, totalStyle); // Aplicar estilo a las celdas combinadas
                    sl.SetCellValue(rowIndex, 5, totalTotal);
                    sl.SetCellStyle(rowIndex, 5, totalStyle); // Aplicar estilo

                    sl.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Datos exportados exitosamente!");
                }
            }
            else
            {
                MessageBox.Show("No hay datos para exportar.");
            }
        }

    }
}
