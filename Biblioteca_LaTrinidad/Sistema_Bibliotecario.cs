using System;
using System.Windows.Forms;

namespace Biblioteca_LaTrinidad
{
    public partial class Sistema_Bibliotecario : Form
    {
        public Sistema_Bibliotecario()
        {
            InitializeComponent();
            panel4.Visible = false;


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
        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Form_Prestamo());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Registro_Devoluion());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Generar_Reporte());
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (panel4.Visible == false)
            {
                panel4.Visible = true;
            }
            else
            {
                panel4.Visible = false;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new FormAgregarLibro());
        }

        private void button6_Click(object sender, EventArgs e)
        {
         
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Form2_ActualizarInfo_libro());
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
