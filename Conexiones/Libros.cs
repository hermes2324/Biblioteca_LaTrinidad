using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexiones
{
    public class GestionLibros
    {
        private string connectionString;

        public GestionLibros(string servidor)
        {
            connectionString = $"Server={servidor};Database=BibliotecaDB;Integrated Security=True;";
        }

        private SqlConnection ObtenerConexion()
        {
            return new SqlConnection(connectionString);
        }

        // Método para agregar un nuevo libro
        public bool AgregarLibro(string titulo, string autor, string editorial, DateTime anoDePublicacion, string categoria, int cantidad)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Libro1 (Titulo, Autor, Editorial, AñoDePublicacion, Categoria, Cantidad) VALUES (@Titulo, @Autor, @Editorial, @AnoDePublicacion, @Categoria, @Cantidad)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Titulo", titulo);
                        cmd.Parameters.AddWithValue("@Autor", autor);
                        cmd.Parameters.AddWithValue("@Editorial", editorial);
                        cmd.Parameters.AddWithValue("@AnoDePublicacion", anoDePublicacion);
                        cmd.Parameters.AddWithValue("@Categoria", categoria);
                        cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        // Método para actualizar los detalles de un libro existente
        public bool ActualizarLibro(int idLibro, string titulo, string autor, string editorial, DateTime anoDePublicacion, string categoria, int cantidad)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Libro1 SET Titulo = @Titulo, Autor = @Autor, Editorial = @Editorial, AñoDePublicacion = @AnoDePublicacion, Categoria = @Categoria, Cantidad = @Cantidad WHERE IDLibro = @IDLibro";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDLibro", idLibro);
                        cmd.Parameters.AddWithValue("@Titulo", titulo);
                        cmd.Parameters.AddWithValue("@Autor", autor);
                        cmd.Parameters.AddWithValue("@Editorial", editorial);
                        cmd.Parameters.AddWithValue("@AnoDePublicacion", anoDePublicacion);
                        cmd.Parameters.AddWithValue("@Categoria", categoria);
                        cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        // Método para eliminar un libro
        public bool EliminarLibro(int idLibro)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Libro1 WHERE IDLibro = @IDLibro";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDLibro", idLibro);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        // Método para consultar todos los libros
        public DataTable ConsultarLibros()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = ObtenerConexion())
            {
                if (conn == null)
                {
                    Console.WriteLine("La conexión es nula.");
                    return dt;
                }

                try
                {
                    string query = "SELECT * FROM Libro1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return dt;
        }

    }
}
