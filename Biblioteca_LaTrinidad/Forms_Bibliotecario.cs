using Conexiones;
using System;
using System.Windows.Forms;

namespace Biblioteca_LaTrinidad
{
    public partial class Forms_Bibliotecario : Form
    {
        ConexionBD conexion;
        public Forms_Bibliotecario()
        {
            InitializeComponent();
            conexion = new ConexionBD("MSI");

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
        }

        private void BtnIngreso_Click(object sender, EventArgs e)
        {
            string nombres = TxtUser.Text;
            string apellidos = textBox1.Text;
            string direccion = textBox2.Text;
            string telefono = textBox3.Text;
            string usuario = textBox4.Text;
            string contraseña = TxtPass.Text;
            string rol = comboBox1.SelectedItem.ToString();

           
            string permisos = "Total";

       
            if (conexion.RegistrarBibliotecario(nombres, apellidos, direccion, telefono, usuario, contraseña, permisos))
            {
                MessageBox.Show("Bibliotecario registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al registrar usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
