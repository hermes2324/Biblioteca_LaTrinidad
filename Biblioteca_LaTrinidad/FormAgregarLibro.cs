using System;
using System.Data;
using System.Windows.Forms;
using Conexiones;

namespace Biblioteca_LaTrinidad
{
    public partial class FormAgregarLibro : Form
    {
        GestionLibros conexion;
        public FormAgregarLibro()
        {
            InitializeComponent();
            conexion = new GestionLibros("MSI");

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idLibro = 0;

            // Llamar al método correspondiente (Agregar o Actualizar)
            bool operacionExitosa;
            if (idLibro == 0)  // Si es 0, estamos agregando un nuevo libro
            {
                operacionExitosa = conexion.AgregarLibro(
                    txtTitulo.Text,
                    txtAutor.Text,
                    txtEditorial.Text,
                    dtpAnoDePublicacion.Value,
                    txtCategoria.Text,
                    Convert.ToInt32(txtCantidad.Text)
                );
            }
            else  // Si el ID es mayor a 0, actualizamos el libro
            {
                operacionExitosa = conexion.ActualizarLibro(
                    idLibro,
                    txtTitulo.Text,
                    txtAutor.Text,
                    txtEditorial.Text,
                    dtpAnoDePublicacion.Value,
                    txtCategoria.Text,
                    Convert.ToInt32(txtCantidad.Text)
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
            //txtTitulo.Clear();
            //txtAutor.Clear();
            //txtEditorial.Clear();
            //txtCategoria.Clear();
            //txtCantidad.Clear();
            //dtpAnoDePublicacion.Value = DateTime.Now;
            //txtIDLibro.Clear(); // Limpiar el campo de ID
        }
    }
}
