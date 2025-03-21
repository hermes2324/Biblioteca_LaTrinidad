using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Conexiones
{
    public class ConexionBD
    {
        private string connectionString;

        public ConexionBD(string servidor)
        {
            connectionString = $"Server={servidor};Database=BibliotecaDB;Integrated Security=True;";
        }
        // Método para registrar un Bibliotecario (ya no permite Administradores)
        public bool RegistrarBibliotecario(string nombres, string apellidos, string direccion, string telefono, string usuario, string contraseña, string permisos)
        {
            try
            {
                string passwordHash = HashPassword(contraseña);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Obtener el único Administrador
                    int idAdmin = ObtenerIDAdministrador();
                    if (idAdmin == -1)
                    {
                        Console.WriteLine("Error: No hay un Administrador registrado.");
                        return false;
                    }

                    string query = "INSERT INTO Bibliotecario (IDAdministrador, Nombres, Apellidos, Direccion, Telefono, Usuario, Contraseña, Permisos) " +
                                   "VALUES (@IDAdministrador, @Nombres, @Apellidos, @Direccion, @Telefono, @Usuario, @Contraseña, @Permisos)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@IDAdministrador", idAdmin);
                        cmd.Parameters.AddWithValue("@Nombres", nombres);
                        cmd.Parameters.AddWithValue("@Apellidos", apellidos);
                        cmd.Parameters.AddWithValue("@Direccion", direccion);
                        cmd.Parameters.AddWithValue("@Telefono", telefono);
                        cmd.Parameters.AddWithValue("@Usuario", usuario);
                        cmd.Parameters.AddWithValue("@Contraseña", passwordHash);
                        cmd.Parameters.AddWithValue("@Permisos", permisos);

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

        // Método para iniciar sesión
        public string IniciarSesion(string usuario, string contraseña)
        {
            try
            {
                string passwordHash = HashPassword(contraseña);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT 'Administrador' AS Rol, Nombres FROM Administrador WHERE Usuario = @Usuario AND Contraseña = @Contraseña " +
                                   "UNION " +
                                   "SELECT 'Bibliotecario', Nombres FROM Bibliotecario WHERE Usuario = @Usuario AND Contraseña = @Contraseña";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Usuario", usuario);
                        cmd.Parameters.AddWithValue("@Contraseña", passwordHash);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return $"{reader["Nombres"]},{reader["Rol"]}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return null;
        }

        // Método para encriptar la contraseña con SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public int ObtenerIDAdministrador()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT IDAdministrador FROM Administrador";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }

    }
}
