using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteca_LaTrinidad
{
    public partial class Gestion_Admin : Form
    {
        public Gestion_Admin()
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


        private void button1_Click_1(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Form_Admin());
        }
    }
}
