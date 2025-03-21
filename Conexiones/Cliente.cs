using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexiones
{
    public class ConexionCliente
    {
        private string connectionString;

        public ConexionCliente(string servidor)
        {
            connectionString = $"Server={servidor};Database=BibliotecaDB;Integrated Security=True;";
        }

        // Método para registrar un Cliente
        public bool RegistrarCliente(string nombres, string apellidos, string direccion, string telefono)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = "INSERT INTO Cliente (Nombres, Apellidos, Direccion, Telefono) " +
                                   "VALUES (@Nombres, @Apellidos, @Direccion, @Telefono)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombres", nombres);
                        cmd.Parameters.AddWithValue("@Apellidos", apellidos);
                        cmd.Parameters.AddWithValue("@Direccion", direccion);
                        cmd.Parameters.AddWithValue("@Telefono", telefono);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        // Método para obtener la lista de Clientes
        public DataTable ObtenerClientes()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT IDCliente, Nombres, Apellidos, Direccion, Telefono FROM Cliente";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable clientes = new DataTable();
                        adapter.Fill(clientes);
                        return clientes;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        // Método para eliminar un Cliente
        public bool EliminarCliente(int idCliente)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Cliente WHERE IDCliente = @IDCliente";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@IDCliente", idCliente);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }

}
