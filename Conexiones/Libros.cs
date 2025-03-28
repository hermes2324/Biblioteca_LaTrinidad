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
        public bool AgregarLibro(string titulo, string autor, string editorial, DateTime anoDePublicacion, string categoria, int cantidad, int CantidadDisponible)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Libro1 (Titulo, Autor, Editorial, AñoDePublicacion, Categoria, Cantidad, CantidadDisponible) VALUES (@Titulo, @Autor, @Editorial, @AnoDePublicacion, @Categoria, @Cantidad, @CantidadDisponible)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Titulo", titulo);
                        cmd.Parameters.AddWithValue("@Autor", autor);
                        cmd.Parameters.AddWithValue("@Editorial", editorial);
                        cmd.Parameters.AddWithValue("@AnoDePublicacion", anoDePublicacion);
                        cmd.Parameters.AddWithValue("@Categoria", categoria);
                        cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                        cmd.Parameters.AddWithValue("@CantidadDisponible", CantidadDisponible);
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
        public bool ActualizarLibro(int idLibro, string titulo, string autor, string editorial, DateTime anoDePublicacion, string categoria, int nuevaCantidad)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1️⃣ Obtener la cantidad actual del libro
                    string queryObtenerCantidad = "SELECT Cantidad, CantidadDisponible FROM Libro1 WHERE IDLibro = @IDLibro";

                    int cantidadActual = 0;
                    int cantidadDisponibleActual = 0;

                    using (SqlCommand cmdCantidad = new SqlCommand(queryObtenerCantidad, conn, transaction))
                    {
                        cmdCantidad.Parameters.AddWithValue("@IDLibro", idLibro);
                        using (SqlDataReader reader = cmdCantidad.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cantidadActual = Convert.ToInt32(reader["Cantidad"]);
                                cantidadDisponibleActual = Convert.ToInt32(reader["CantidadDisponible"]);
                            }
                        }
                    }

                    // 2️⃣ Calcular la diferencia entre la nueva cantidad y la actual
                    int diferencia = nuevaCantidad - cantidadActual;

                    // 3️⃣ Actualizar el libro con la nueva cantidad
                    string queryActualizarLibro = @"
                UPDATE Libro1 
                SET Titulo = @Titulo, Autor = @Autor, Editorial = @Editorial, 
                    AñoDePublicacion = @AnoDePublicacion, Categoria = @Categoria, 
                    Cantidad = @NuevaCantidad 
                WHERE IDLibro = @IDLibro";

                    using (SqlCommand cmdActualizar = new SqlCommand(queryActualizarLibro, conn, transaction))
                    {
                        cmdActualizar.Parameters.AddWithValue("@IDLibro", idLibro);
                        cmdActualizar.Parameters.AddWithValue("@Titulo", titulo);
                        cmdActualizar.Parameters.AddWithValue("@Autor", autor);
                        cmdActualizar.Parameters.AddWithValue("@Editorial", editorial);
                        cmdActualizar.Parameters.AddWithValue("@AnoDePublicacion", anoDePublicacion);
                        cmdActualizar.Parameters.AddWithValue("@Categoria", categoria);
                        cmdActualizar.Parameters.AddWithValue("@NuevaCantidad", nuevaCantidad);
                        cmdActualizar.ExecuteNonQuery();
                    }

                    // 4️⃣ Actualizar CantidadDisponible si hubo cambios
                    if (diferencia != 0)
                    {
                        int nuevaCantidadDisponible = cantidadDisponibleActual + diferencia;

                        // Evitar que CantidadDisponible sea negativa
                        if (nuevaCantidadDisponible < 0)
                        {
                            nuevaCantidadDisponible = 0;
                        }

                        string queryActualizarCantidadDisponible = @"
                    UPDATE Libro1 
                    SET CantidadDisponible = @NuevaCantidadDisponible 
                    WHERE IDLibro = @IDLibro";

                        using (SqlCommand cmdActualizarDisponible = new SqlCommand(queryActualizarCantidadDisponible, conn, transaction))
                        {
                            cmdActualizarDisponible.Parameters.AddWithValue("@IDLibro", idLibro);
                            cmdActualizarDisponible.Parameters.AddWithValue("@NuevaCantidadDisponible", nuevaCantidadDisponible);
                            cmdActualizarDisponible.ExecuteNonQuery();
                        }
                    }

                    // ✅ Confirmar transacción
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error al actualizar libro: " + ex.Message);
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

        public DataTable ObtenerInventarioLibros()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = ObtenerConexion())
            {
                try
                {
                    string query = @"
                SELECT 
                    Titulo AS NombreLibro, 
                    Cantidad AS Total, 
                    Cantidad - ISNULL(SUM(DP.CantidadDeLibros), 0) AS Disponibles
                FROM 
                    Libro1 L
                LEFT JOIN 
                    DetallePrestamo DP ON L.IDLibro = DP.IDLibro
                GROUP BY 
                    L.Titulo, L.Cantidad";

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
