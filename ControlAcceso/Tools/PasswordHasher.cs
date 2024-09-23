namespace ControlAcceso.Tools
{
   using System;
using System.Security.Cryptography;
using System.Text;

class PasswordHasher
{
    // Configuración de los parámetros para el hash
    private const int SaltSize = 16; // Tamaño del salt en bytes
    private const int HashSize = 20; // Tamaño del hash en bytes
    private const int Iterations = 10000; // Cantidad de iteraciones

    // Método para hashear una contraseña
    public static string HashPassword(string password)
    {
        // Generar un salt aleatorio
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);

            // Generar el hash usando PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Combinar el salt y el hash en un solo arreglo
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convertir el resultado a una cadena Base64 para almacenarlo
            return Convert.ToBase64String(hashBytes);
        }
    }

    // Método para verificar si una contraseña coincide con un hash almacenado
    public static bool VerifyPassword(string password, string storedHash)
    {
        // Convertir la cadena Base64 de vuelta a un arreglo de bytes
        byte[] hashBytes = Convert.FromBase64String(storedHash);

        // Extraer el salt del arreglo
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        // Generar un nuevo hash con la contraseña proporcionada y el salt almacenado
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Comparar el nuevo hash con el hash almacenado byte por byte
        for (int i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}
}