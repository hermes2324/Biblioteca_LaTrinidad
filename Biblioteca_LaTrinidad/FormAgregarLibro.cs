using System;
using System.Data;
using System.Windows.Forms;
using Conexiones;

namespace Biblioteca_LaTrinidad
{
    public partial class FormAgregarLibro : Form
    {
        GestionLibros conexion;
        private int idLibro; // Declarar idLibro como variable de clase

        public FormAgregarLibro()
        {
            InitializeComponent();
            conexion = new GestionLibros("MSI");
            idLibro = 0; // Inicializar idLibro
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool operacionExitosa;
            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad))
            {
                MessageBox.Show("Por favor, ingrese un número válido en el campo de cantidad.");
                return;
            }

            if (idLibro == 0)  // Agregar nuevo libro
            {
                operacionExitosa = conexion.AgregarLibro(
                    txtTitulo.Text,
                    txtAutor.Text,
                    txtEditorial.Text,
                    dtpAnoDePublicacion.Value,
                    txtCategoria.Text,
                    cantidad,
                    cantidad  // CantidadDisponible inicialmente igual a Cantidad
                );
            }
            else  // Actualizar libro existente
            {
                operacionExitosa = conexion.ActualizarLibro(
                    idLibro,
                    txtTitulo.Text,
                    txtAutor.Text,
                    txtEditorial.Text,
                    dtpAnoDePublicacion.Value,
                    txtCategoria.Text,
                    cantidad
                );
            }

            if (operacionExitosa)
            {
                string mensaje = idLibro == 0 ? "Libro agregado exitosamente." : "Libro actualizado exitosamente.";
                MessageBox.Show(mensaje);
                CargarLibros();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Hubo un error en la operación.");
            }
        }

        private void CargarLibros()
        {
            DataTable dt = conexion.ConsultarLibros();

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No hay libros disponibles.");
                dataGridView1.DataSource = null;
            }
        }

        private void LimpiarCampos()
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            txtEditorial.Clear();
            txtCategoria.SelectedIndex = -1;
            txtCantidad.Clear();
            dtpAnoDePublicacion.Value = DateTime.Now;
            idLibro = 0; // Reiniciar idLibro
        }

        private void FormAgregarLibro_Load(object sender, EventArgs e)
        {
            CargarLibros();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada
            if (dataGridView1.CurrentRow != null && dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    // Obtener los valores de la fila seleccionada
                    DataGridViewRow row = dataGridView1.CurrentRow;

                    // Asignar los valores a los TextBox 
                    // Ajusta los nombres de las columnas según tu base de datos
                    txtTitulo.Text = row.Cells["Titulo"].Value?.ToString() ?? "";
                    txtAutor.Text = row.Cells["Autor"].Value?.ToString() ?? "";
                    txtEditorial.Text = row.Cells["Editorial"].Value?.ToString() ?? "";
                    txtCategoria.Text = row.Cells["Categoria"].Value?.ToString() ?? "";
                    txtCantidad.Text = row.Cells["Cantidad"].Value?.ToString() ?? "";


                    // Para el DateTimePicker, necesitamos convertir el valor
                    if (row.Cells["AñoDePublicacion"].Value != null)
                    {
                        if (DateTime.TryParse(row.Cells["AñoDePublicacion"].Value.ToString(),
                           out DateTime fecha))
                        {
                            dtpAnoDePublicacion.Value = fecha;
                        }
                    }

                    // También necesitamos actualizar la variable idLibro para actualizaciones
                    if (row.Cells["IDLibro"].Value != null)
                    {
                        idLibro = Convert.ToInt32(row.Cells["IDLibro"].Value);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message);
                }
            }
        }

        private void FormAgregarLibro_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            dataGridView1.ClearSelection();
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {

        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int idLibro = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IDLibro"].Value);
                GestionLibros gestionLibros = new GestionLibros("MSI");

                if (gestionLibros.EliminarLibro(idLibro))
                {
                    MessageBox.Show("Libro eliminado exitosamente.");
                    // Actualiza la tabla después de eliminar el libro
                    dataGridView1.DataSource = gestionLibros.ConsultarLibros();
                }
                else
                {
                    MessageBox.Show("Hubo un error al eliminar el libro.");
                }
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridView1.HitTest(e.X, e.Y);
                dataGridView1.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dataGridView1.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dataGridView1, e.Location);
                }
            }
        }
    }
}
