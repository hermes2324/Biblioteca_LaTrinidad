﻿using Conexiones;
using System;
using System.Windows.Forms;

namespace Biblioteca_LaTrinidad
{
    public partial class Form1 : Form
    {
        ConexionBD Conexion;
        public Form1()
        {
            InitializeComponent();
            Conexion = new ConexionBD("MSI");

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2)
            {
                TxtPass.Visible = false;
                label3.Visible = false;
                linkLabel2.Visible = false;
            }
            else
            {
                TxtPass.Visible = true;
                label3.Visible = true;
                linkLabel2.Visible = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forms_Bibliotecario sistema_Bibliotecario = new Forms_Bibliotecario();
            sistema_Bibliotecario.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 form1 = new Form1();
            Olvido_Contraseña olvido_Contraseña = new Olvido_Contraseña();
            olvido_Contraseña.Show();
        }

        private void BtnIngreso_Click(object sender, EventArgs e)
        {
            string usuario = TxtUser.Text;
            string contraseña = TxtPass.Text;

            string resultado = Conexion.IniciarSesion(usuario, contraseña);
            if (resultado != null)
            {
                string[] datos = resultado.Split(',');
                string nombre = datos[0];
                string rol = datos[1];
                string permisos = datos.Length > 2 ? datos[2] : "";

                MessageBox.Show($"Bienvenido {nombre}, Rol: {rol} - Permisos: {permisos}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (rol == "Administrador")
                {
                    new Form_Admin().Show();
                }
                else if (rol == "Bibliotecario")
                {
                    new Sistema_Bibliotecario().Show();
                }

                this.Hide();
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
