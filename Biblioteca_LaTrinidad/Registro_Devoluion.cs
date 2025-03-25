using Conexiones;
using System;
using System.Data;
using System.Windows.Forms;
using static Biblioteca_LaTrinidad.Form1;

namespace Biblioteca_LaTrinidad
{
    public partial class Registro_Devoluion : Form
    {
       
        Prestamos prestamos;
        public Registro_Devoluion()
        {
            InitializeComponent();
            prestamos = new Prestamos("MSI");
            CargarClientes();
        }
        private void CargarClientes()
        {
            // Obtener los clientes desde la base de datos
            DataTable dtClientes = prestamos.ObtenerClientes();

            if (dtClientes != null && dtClientes.Rows.Count > 0)
            {
                // Limpiar el ComboBox antes de llenarlo
                comboBox1.Items.Clear();

                // Asignar la fuente de datos al ComboBox
                comboBox1.DataSource = dtClientes;

                // Mostrar el nombre en el ComboBox
                comboBox1.DisplayMember = "Nombres";  // Mostrar la columna 'Nombres'

                // Guardar el IDCliente internamente
                comboBox1.ValueMember = "IDCliente";  // Guardar el 'IDCliente' como valor
            }
            else
            {
                MessageBox.Show("No hay clientes registrados.");
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                int idCliente = Convert.ToInt32(((DataRowView)comboBox1.SelectedItem)["idCliente"]);

                // Mostrar los préstamos del cliente en dataGridView1
                dataGridView1.DataSource = prestamos.ObtenerDetallePrestamoPorCliente(idCliente);

              
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Verifica si hay una fila seleccionada
            {
                int idCliente = Convert.ToInt32(((DataRowView)comboBox1.SelectedItem)["idCliente"]);

                int idPrestamo = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IDPrestamo"].Value);
                int idLibro = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IDLibro"].Value);
                string estado = "Disponible"; // Estado actualizado del libro
                int idBibliotecario = SesionActual.IDBibliotecario;


                bool exito = prestamos.RegistrarDevolucion(idPrestamo, idLibro, estado,idBibliotecario);

                if (exito)
                {
                    MessageBox.Show("Devolución registrada correctamente.");
                    //ActualizarDataGrid(); // Refresca el DataGridView después de la devolución
                    dataGridView1.DataSource = prestamos.ObtenerDetallePrestamoPorCliente(idCliente);

                  
                }
                else
                {
                    MessageBox.Show("Error al registrar la devolución.");
                }
            }
            else
            {
                MessageBox.Show("Seleccione un préstamo para devolver.");
            }
        }
    }
}
