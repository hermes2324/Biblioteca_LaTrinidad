using System.Data;
using System.Windows.Forms;
using Conexiones;

namespace Biblioteca_LaTrinidad
{
    public partial class Form_Admin : Form
    {
        ConexionCliente conexion;
        public Form_Admin()
        {
            InitializeComponent();
            conexion = new ConexionCliente("MSI");
        }

        //private void BtnIngreso_Click(object sender, System.EventArgs e)
        //{
        //    string nombres = TxtUser.Text;
        //    string apellidos = textBox1.Text;
        //    string direccion = textBox2.Text;
        //    string telefono = textBox3.Text;

        //    if (conexion.RegistrarCliente(nombres, apellidos, direccion, telefono))
        //    {
        //        MessageBox.Show("Cliente registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        this.Close();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error al registrar Cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            string nombres = TxtUser.Text;
            string apellidos = textBox1.Text;
            string direccion = textBox2.Text;
            string telefono = textBox3.Text;

            if (conexion.RegistrarCliente(nombres, apellidos, direccion, telefono))
            {
                MessageBox.Show("Cliente registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al registrar Cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            CargarClientes();
        }

        private void Form_Admin_Load(object sender, System.EventArgs e)
        {
            CargarClientes();
        }

        private void CargarClientes()
        {
            DataTable dt = conexion.ObtenerClientes();

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
    }
}
