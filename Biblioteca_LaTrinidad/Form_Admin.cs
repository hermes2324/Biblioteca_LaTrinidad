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

        private void BtnIngreso_Click(object sender, System.EventArgs e)
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
        }
    }
}
