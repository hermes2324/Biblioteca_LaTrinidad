using System;
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

            // Verificar si el ID del libro no está vacío para actualizar
            if (!string.IsNullOrEmpty(txtIDLibro.Text))
            {
                idLibro = Convert.ToInt32(txtIDLibro.Text);  // Obtener el ID del libro desde el TextBox
            }

            // Llamar al método correspondiente (Agregar o Actualizar)
            bool operacionExitosa;

            if (idLibro == 0)  // Si el ID es 0, significa que estamos agregando un nuevo libro
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
            else  // Si el ID es mayor a 0, significa que estamos actualizando el libro
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
                LimpiarCampos(); // Limpiar los campos después de agregar o actualizar el libro
            }
            else
            {
                MessageBox.Show("Hubo un error en la operación.");
            }
        }
        private void LimpiarCampos()
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            txtEditorial.Clear();
            txtCategoria.Clear();
            txtCantidad.Clear();
            dtpAnoDePublicacion.Value = DateTime.Now;
            txtIDLibro.Clear(); // Limpiar el campo de ID
        }
    }
}
