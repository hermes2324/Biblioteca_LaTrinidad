using System;
using System.Windows.Forms;

namespace Biblioteca_LaTrinidad
{
    public partial class Sistema_usuario : Form
    {
        // private System.Windows.Forms.Panel panel2;

        public Sistema_usuario()
        {
            InitializeComponent();

        }
        private void AbrirFormEnPanel(object Formhijo)
        {
            if (this.panel2.Controls.Count > 0)
                this.panel2.Controls.RemoveAt(0);

            Form fh = Formhijo as Form;
            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(fh);
            this.panel2.Tag = fh;
            fh.Show();
        }


        private void TxtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Buscar_Libro());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Solicitar_Prestamo());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Devolver_Libro());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Consultar_Disponibilidad_Libro());
        }

        private void Sistema_Biblioteca_Load(object sender, EventArgs e)
        {

        }
    }
}
