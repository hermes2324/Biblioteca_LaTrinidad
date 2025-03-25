using System;
using System.Data;
using System.Windows.Forms;
using Conexiones;
namespace Biblioteca_LaTrinidad
{
    public partial class Devolver_Libro : Form
    {
        ConexionCliente conexion;
        Prestamos prestamos;
        public Devolver_Libro()
        {
            InitializeComponent();
            conexion = new ConexionCliente("MSI");
            CargarClientes();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, System.EventArgs e)
        {

        }

        private void btnBuscarPrestamo_Click(object sender, System.EventArgs e)
        {

        }

        private void button1_Click(object sender, System.EventArgs e)
        {

        }
        private void CargarClientes()
        {
            DataTable dtClientes = conexion.ObtenerClientes(); // Método que obtiene los clientes desde la BD

            if (dtClientes != null && dtClientes.Rows.Count > 0)
            {
                comboBox1.DataSource = dtClientes;
                comboBox1.DisplayMember = "nombre";  // Mostrar el nombre del cliente
                comboBox1.ValueMember = "idCliente"; // Guardar el ID internamente
            }
            else
            {
                MessageBox.Show("No hay clientes registrados.");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                int idCliente = Convert.ToInt32(comboBox1.SelectedValue);

                // Mostrar los préstamos del cliente en dataGridView1
                dataGridView1.DataSource = prestamos.ObtenerPrestamosPorCliente(idCliente);

                // Mostrar los detalles del préstamo en dataGridView2
                dataGridView2.DataSource = prestamos.ObtenerDetallePrestamoPorCliente(idCliente);
            }
        }
    }
}
