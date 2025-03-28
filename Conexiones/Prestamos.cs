using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Conexiones
{

    // Asegúrate de que la clase Inventario esté definida en el mismo espacio de nombres o importe el espacio de nombres correcto
   

    public class Prestamos
    {
        private string connectionString;
        public Prestamos(string servidor)
        {
            connectionString = $"Server={servidor};Database=BibliotecaDB;Integrated Security=True;";
        }
        private int ObtenerCantidadDisponible(int idLibro)
        {
            int cantidadDisponible = 0;
            string query = "SELECT CantidadDisponible FROM Libro1 WHERE IDLibro = @IDLibro";

            using (SqlConnection conn = ObtenerConexion())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDLibro", idLibro);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        cantidadDisponible = Convert.ToInt32(result);
                    }
                }
            }

            return cantidadDisponible;
        }

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(connectionString);
        }
        //insertar el id del bibliotecario w
        // Método para registrar un préstamo
        public int RegistrarPrestamo(int idCliente, int idBibliotecario, DateTime fechaPrestamo, DateTime fechaDevolucion, int idLibro, int cantidad)
        {
            int idPrestamo = 0;
            try
            {
                // Verificar la cantidad disponible antes de registrar el préstamo
                int cantidadDisponible = ObtenerCantidadDisponible(idLibro);
                if (cantidad > cantidadDisponible)
                {
                    throw new InvalidOperationException($"No hay suficientes libros disponibles. Disponibles: {cantidadDisponible}, Solicitados: {cantidad}");
                }

                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Registrar el préstamo
                            string query = "INSERT INTO Prestamo (IDCliente, IDBibliotecario, FechaPrestamo, FechaDevolucion) " +
                                           "OUTPUT INSERTED.IDPrestamo VALUES (@idCliente, @idBibliotecario, @fechaPrestamo, @fechaDevolucion)";
                            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                                cmd.Parameters.AddWithValue("@idBibliotecario", idBibliotecario);
                                cmd.Parameters.AddWithValue("@fechaPrestamo", fechaPrestamo);
                                cmd.Parameters.AddWithValue("@fechaDevolucion", fechaDevolucion);
                                idPrestamo = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // Registrar el detalle del préstamo
                            RegistrarDetallePrestamo(idPrestamo, idLibro, "Prestado", cantidad, conn, transaction);

                            // Descontar libros prestados en la tabla Libro1
                            string updateQuery = "UPDATE Libro1 SET CantidadDisponible = CantidadDisponible - @cantidad WHERE IDLibro = @idLibro";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@cantidad", cantidad);
                                updateCmd.Parameters.AddWithValue("@idLibro", idLibro);
                                updateCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar préstamo: " + ex.Message);
            }
            return idPrestamo;
        }

        public void RegistrarDetallePrestamo(int idPrestamo, int idLibro, string estadoDelLibro, int cantidad, SqlConnection conn, SqlTransaction transaction)
        {
            // Verificar si ya existe un registro con el mismo idPrestamo e idLibro
            string checkQuery = "SELECT COUNT(*) FROM DetallePrestamo WHERE IDPrestamo = @idPrestamo AND IDLibro = @idLibro";
            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn, transaction))
            {
                checkCmd.Parameters.AddWithValue("@idPrestamo", idPrestamo);
                checkCmd.Parameters.AddWithValue("@idLibro", idLibro);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    throw new InvalidOperationException("Ya existe un registro con el mismo IDPrestamo e IDLibro.");
                }
            }

            // Insertar el nuevo registro si no existe
            string query = "INSERT INTO DetallePrestamo (IDPrestamo, IDLibro, EstadoDelLibro, CantidadDeLibros) " +
                           "VALUES (@idPrestamo, @idLibro, @estadoDelLibro, @cantidad)";
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@idPrestamo", idPrestamo);
                cmd.Parameters.AddWithValue("@idLibro", idLibro);
                cmd.Parameters.AddWithValue("@estadoDelLibro", estadoDelLibro);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.ExecuteNonQuery();
            }
        }





        // Obtener préstamos por cliente
        public DataTable ObtenerPrestamosPorCliente(int idCliente)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                DataTable dt = new DataTable();
                conn.Open();
                string query = "SELECT * FROM Prestamo WHERE IDCliente = @idCliente";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@idCliente", idCliente);
                    da.Fill(dt);
                }
                return dt;
            }
        }

        // Obtener detalle de préstamo por cliente
        public DataTable ObtenerDetallePrestamoPorCliente(int idCliente)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                DataTable dt = new DataTable();
                conn.Open();
                string query = @"
            SELECT dp.IDPrestamo, dp.IDLibro, l.Titulo, dp.EstadoDelLibro, dp.CantidadDeLibros
            FROM DetallePrestamo dp
            INNER JOIN Prestamo p ON dp.IDPrestamo = p.IDPrestamo
            INNER JOIN Libro1 l ON dp.IDLibro = l.IDLibro
            WHERE p.IDCliente = @idCliente AND dp.EstadoDelLibro != 'Disponible'"; // Excluir los libros devueltos
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@idCliente", idCliente);
                    da.Fill(dt);
                }
                return dt;
            }
        }


        public DataTable ObtenerDetallesPrestamo()
        {
            string query = @"
                SELECT 
                    c.Nombres AS NombreCliente, 
                    l.Titulo AS NombreLibro, 
                    p.FechaPrestamo, 
                    p.FechaDevolucion, 
                    dp.EstadoDelLibro, 
                    dp.CantidadDeLibros
                FROM 
                    Prestamo p
                INNER JOIN 
                    Cliente c ON p.IDCliente = c.IDCliente
                INNER JOIN 
                    DetallePrestamo dp ON p.IDPrestamo = dp.IDPrestamo
                INNER JOIN 
                    Libro1 l ON dp.IDLibro = l.IDLibro
                ORDER BY 
                    p.FechaPrestamo";

            DataTable dt = new DataTable();

            using (SqlConnection conn = ObtenerConexion())
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener detalles del préstamo: " + ex.Message);
                }
            }

            return dt;
        }

        public DataTable ObtenerClientes()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = ObtenerConexion())
            {
                try
                {
                    // Corregir el nombre de la columna 'nombre' a 'Nombres'
                    string query = "SELECT IDCliente, Nombres FROM Cliente";
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
                    Console.WriteLine("Error al obtener clientes: " + ex.Message);
                }
            }

            return dt;
        }

        // Método para obtener todos los libros
        public List<Libro> ObtenerLibros()
        {
            List<Libro> libros = new List<Libro>();

            string query = "SELECT IDLibro, Titulo FROM Libro1";

            using (SqlConnection conn = ObtenerConexion())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        libros.Add(new Libro
                        {
                            IDLibro = reader.GetInt32(0),
                            Titulo = reader.GetString(1)
                        });
                    }
                }
            }

            return libros;
        }

        public List<Libro> ObtenerLibrosDisponibles()
        {
            List<Libro> librosDisponibles = new List<Libro>();

            string query = @"
        SELECT L.IDLibro, L.Titulo, 
               L.Cantidad - ISNULL(SUM(DP.CantidadDeLibros), 0) AS Disponibles
        FROM Libro1 L
        LEFT JOIN DetallePrestamo DP ON L.IDLibro = DP.IDLibro
        GROUP BY L.IDLibro, L.Titulo, L.Cantidad
        HAVING L.Cantidad - ISNULL(SUM(DP.CantidadDeLibros), 0) > 0";

            try
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            // Verificar si las columnas no están nulas
                            if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                            {
                                librosDisponibles.Add(new Libro
                                {
                                    IDLibro = reader.GetInt32(0),
                                    Titulo = reader.GetString(1),
                                    Disponibles = reader.GetInt32(2) // Añadir la cantidad disponible si la necesitas
                                });
                            }
                            else
                            {
                                // Si alguna columna es nula, puedes registrar un mensaje de log o manejar el caso
                                Console.WriteLine("Alerta: Datos nulos encontrados en la fila.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener libros disponibles: " + ex.Message);
            }

            return librosDisponibles;
        }

        public bool VerificarDisponibilidad(int idLibro)
        {
            bool disponible = false;

            string query = @"
        SELECT CantidadDisponible 
        FROM Libro1 
        WHERE IDLibro = @IDLibro";

            using (SqlConnection conn = ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDLibro", idLibro);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            int cantidadDisponible = Convert.ToInt32(result);
                            disponible = cantidadDisponible > 0;  // Si hay al menos 1 disponible, retorna true.
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al verificar disponibilidad: " + ex.Message);
                }
            }

            return disponible;
        }


        public class Libro
        {
            public int IDLibro { get; set; }
            public string Titulo { get; set; }
            public int Disponibles { get; set; }  // Agregar la propiedad para almacenar los libros disponibles
        }

        public bool RegistrarDevolucion(int idPrestamo, int idLibro, string nuevoEstadoLibro, int idBibliotecario)
        {
            bool resultado = false;

            using (SqlConnection conn = ObtenerConexion())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insertar en la tabla Devolucion
                    string queryInsertarDevolucion = @"
INSERT INTO Devolucion (FechaDeDevolucion, EstadoDelLibro, IDBibliotecario, IDPrestamo)
VALUES (GETDATE(), @estado, @idBibliotecario, @idPrestamo)";

                    using (SqlCommand cmdInsertarDevolucion = new SqlCommand(queryInsertarDevolucion, conn, transaction))
                    {
                        cmdInsertarDevolucion.Parameters.AddWithValue("@estado", nuevoEstadoLibro);
                        cmdInsertarDevolucion.Parameters.AddWithValue("@idBibliotecario", idBibliotecario);
                        cmdInsertarDevolucion.Parameters.AddWithValue("@idPrestamo", idPrestamo);
                        cmdInsertarDevolucion.ExecuteNonQuery();
                    }

                    // Actualizar el estado del libro en DetallePrestamo
                    string queryActualizarEstado = @"
UPDATE DetallePrestamo 
SET EstadoDelLibro = @estado 
WHERE IDPrestamo = @idPrestamo AND IDLibro = @idLibro";

                    using (SqlCommand cmdEstado = new SqlCommand(queryActualizarEstado, conn, transaction))
                    {
                        cmdEstado.Parameters.AddWithValue("@estado", nuevoEstadoLibro);
                        cmdEstado.Parameters.AddWithValue("@idPrestamo", idPrestamo);
                        cmdEstado.Parameters.AddWithValue("@idLibro", idLibro);
                        cmdEstado.ExecuteNonQuery();
                    }

                    // Actualizar la fecha de devolución en Prestamo
                    string queryActualizarFecha = @"
UPDATE Prestamo 
SET FechaDevolucion = GETDATE() 
WHERE IDPrestamo = @idPrestamo";

                    using (SqlCommand cmdFecha = new SqlCommand(queryActualizarFecha, conn, transaction))
                    {
                        cmdFecha.Parameters.AddWithValue("@idPrestamo", idPrestamo);
                        cmdFecha.ExecuteNonQuery();
                    }

                    // Obtener la cantidad de libros devueltos desde DetallePrestamo
                    string queryCantidadDevuelta = @"
SELECT CantidadDeLibros 
FROM DetallePrestamo 
WHERE IDPrestamo = @idPrestamo AND IDLibro = @idLibro";

                    int cantidadDevuelta = 0;
                    using (SqlCommand cmdCantidadDevuelta = new SqlCommand(queryCantidadDevuelta, conn, transaction))
                    {
                        cmdCantidadDevuelta.Parameters.AddWithValue("@idPrestamo", idPrestamo);
                        cmdCantidadDevuelta.Parameters.AddWithValue("@idLibro", idLibro);
                        var result = cmdCantidadDevuelta.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            cantidadDevuelta = Convert.ToInt32(result);
                        }
                    }

                    // Validar que la cantidad devuelta sea mayor a cero
                    if (cantidadDevuelta > 0)
                    {
                        // Actualizar la cantidad disponible en la tabla Libro1
                        string queryActualizarCantidadDisponible = @"
UPDATE Libro1
SET CantidadDisponible = CantidadDisponible + @cantidadDevuelta
WHERE IDLibro = @idLibro";

                        using (SqlCommand cmdActualizarCantidadDisponible = new SqlCommand(queryActualizarCantidadDisponible, conn, transaction))
                        {
                            cmdActualizarCantidadDisponible.Parameters.AddWithValue("@cantidadDevuelta", cantidadDevuelta);
                            cmdActualizarCantidadDisponible.Parameters.AddWithValue("@idLibro", idLibro);
                            cmdActualizarCantidadDisponible.ExecuteNonQuery();
                        }
                    }

                    // Confirmar la transacción
                    transaction.Commit();
                    resultado = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en la devolución: " + ex.Message);
                    transaction.Rollback();
                }
            }

            return resultado;
        }








    }
}
