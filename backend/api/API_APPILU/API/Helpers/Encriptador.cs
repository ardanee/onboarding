using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace API.Helpers
{
    public class Encriptador
    {
        //string clave = "LaUnion2015";
        public Encriptador()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Encriptar
        /// <summary>
        /// Método para encriptar un texto plano usando el algoritmo (Rijndael).
        /// Este es el mas simple posible, muchos de los datos necesarios los
        /// definimos como constantes.
        /// </summary>
        /// <param name="textoQueEncriptaremos">texto a encriptar</param>
        /// <param name="clave">texto para la clave</param>
        /// <returns>Texto encriptado</returns>
        public static string Encriptar(string textoQueEncriptaremos, string clave = "LaUnion2015")
        {
            return Encriptar(textoQueEncriptaremos, clave, "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
        }

        /// <summary>
        /// Método para encriptar un texto plano usando el algoritmo (Rijndael)
        /// </summary>
        /// <param name="textoQueEncriptaremos">Texto que vamos a Encriptar</param>
        /// <param name="passBase">Clave</param>
        /// <param name="saltValue">Salto para Derivar la Clave</param>
        /// <param name="hashAlgorithm">Nombre del Algoritmo para Encriptar</param>
        /// <param name="passwordIterations">Numero de veces para Generar la Clave</param>
        /// <param name="initVector">Indicador de Vector para la Clave</param>
        /// <param name="keySize">Tamaño de la Clave</param>
        /// <returns>Texto encriptado</returns>
        public static string Encriptar(string textoQueEncriptaremos, string passBase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC };
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }
        #endregion

        #region Desencriptar
        /// <summary>
        /// Método para desencriptar un texto encriptado.
        /// </summary>
        /// <param name="textoEncriptado">texto a desencriptar</param>
        /// <param name="clave">texto para la clave</param>
        /// <returns>Texto desencriptado</returns>
        public static string Desencriptar(string textoEncriptado, string clave = "LaUnion2015")
        {
            return Desencriptar(textoEncriptado, clave, "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
        }

        /// <summary>
        /// Método para desencriptar un texto encriptado (Rijndael)
        /// </summary>
        /// <param name="textoEncriptado">Texto a Desencriptar</param>
        /// <param name="passBase">Clave</param>
        /// <param name="saltValue">Salto para Derivar la Clave</param>
        /// <param name="hashAlgorithm">Nombre del Algoritmo para Encriptar</param>
        /// <param name="passwordIterations">Numero de veces para Generar la Clave</param>
        /// <param name="initVector">Indicador de Vector para la Clave</param>
        /// <param name="keySize">Tamaño de la Clave</param>
        /// <returns>Texto desencriptado</returns>
        public static string Desencriptar(string textoEncriptado, string passBase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            string plainText;
            try
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);
                byte[] keyBytes = password.GetBytes(keySize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC };
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                throw new Exception("La cadena a Desencriptar no es valida, " + ex.Message);
            }
            return plainText;
        }
        #endregion
    }
}