using Conexiones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using static Biblioteca_LaTrinidad.Form1;
using static Conexiones.Prestamos;

namespace Biblioteca_LaTrinidad
{
    public partial class Form_Prestamo : Form
    {
        Prestamos prestamos;

        public Form_Prestamo()
        {
            InitializeComponent();
            prestamos = new Prestamos("MSI");


        }

        private void Form_Prestamo_Load(object sender, System.EventArgs e)
        {
            CargarClientes();
            CargarLibros();
            CargarDetallePrestamo();
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

        private void CargarLibros()
        {
            // Obtener libros desde la base de datos
            List<Libro> libros = prestamos.ObtenerLibros();

            // Limpiar el ComboBox antes de llenarlo
            comboBox2.Items.Clear();

            // Llenar el ComboBox con los títulos de los libros
            foreach (var libro in libros)
            {
                // Agregar solo el título, pero con el IDLibro en el Tag
                var item = new ComboBoxItem
                {
                    Titulo = libro.Titulo,
                    IDLibro = libro.IDLibro
                };
                comboBox2.Items.Add(item);
            }
        }

        // Clase para representar los elementos del ComboBox
        public class ComboBoxItem
        {
            public string Titulo { get; set; }
            public int IDLibro { get; set; }

            // Para mostrar solo el título en el ComboBox
            public override string ToString()
            {
                return Titulo;
            }
        }
        private void simpleButton1_Click(object sender, System.EventArgs e)
        {

        }

        private void btnAgregar_Libro_Click(object sender, System.EventArgs e)
        {

            if (btnPrestamo.Enabled)
            {
                // Verificar que todos los campos estén llenos
                if (comboBox1.SelectedItem == null ||
                    comboBox2.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(textBox1.Text) ||
                    !int.TryParse(textBox1.Text, out int cantidad))
                {
                    MessageBox.Show("Por favor, complete todos los campos y asegúrese de que la cantidad sea un número válido.");
                    return;
                }

                try
                {
                    // Lógica para registrar el préstamo
                    int idCliente = Convert.ToInt32(((DataRowView)comboBox1.SelectedItem)["idCliente"]);
                    int idLibro = (int)((dynamic)comboBox2.SelectedItem).IDLibro;
                    // Suponiendo que el bibliotecario tiene un ID fijo, como ejemplo
                    int idBibliotecario = SesionActual.IDBibliotecario;
                    DateTime fechaPrestamo = dateTimePicker1.Value;
                    DateTime fechaDevolucion = dateTimePicker2.Value;
                    string estado = "Prestado";

                    int idPrestamo = prestamos.RegistrarPrestamo(idCliente, idBibliotecario, fechaPrestamo, fechaDevolucion, idLibro, cantidad);

                    MessageBox.Show("Préstamo registrado con éxito.");
                    CargarDetallePrestamo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Este libro no está disponible para préstamo.");
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Obtener el ítem seleccionado en comboBox2
            var itemSeleccionado = (ComboBoxItem)comboBox2.SelectedItem;

            if (itemSeleccionado != null)
            {
                // Acceder al ID del libro
                int idLibro = itemSeleccionado.IDLibro;

                // Verificar disponibilidad del libro
                bool disponible = prestamos.VerificarDisponibilidad(idLibro);

                // Actualizar el comboBox3 dependiendo de la disponibilidad
                if (disponible)
                {
                    // Si está disponible
                    btnPrestamo.Enabled = true;
                  //  MessageBox.Show("Este libro está disponible para préstamo.");

                    // Actualizar comboBox3 con "Disponible"
                    comboBox3.Items.Clear();
                    comboBox3.Items.Add("Disponible");

                }
                else
                {
                    // Si no está disponible
                    btnPrestamo.Enabled = false;
                  //  MessageBox.Show("Este libro no está disponible en stock.");

                    // Actualizar comboBox3 con "No disponible"
                    comboBox3.Items.Clear();
                    comboBox3.Items.Add("No disponible");
                }
                  comboBox3.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                int idCliente = Convert.ToInt32(((DataRowView)comboBox1.SelectedItem)["idCliente"]);
            }
        }

        private void CargarDetallePrestamo()
        {
            DataTable dt = prestamos.ObtenerDetallesPrestamo();

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No se encontraron detalles de préstamos.");
            }
        }
    }
}
