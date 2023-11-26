using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LHCommonFunctions.Source
{
    public static class LHCryptoFunctions
    {
        /// <summary>
        /// This function encrypts a file using AES encryption.
        /// </summary>
        /// <param name="inputFile">Path to the file to encrypt</param>
        /// <param name="outputFile">The encrypted output file</param>
        /// <param name="password">The password for derivation of a key for AES encryption</param>
        /// <param name="salt">The salt for derivation of a key for AES encryption</param>
        /// <param name="numOfIterations">The num of iterations for derivation of a key for AES encryption</param>
        public static void AesEncrypt(String inputFile, String outputFile, String password, String salt, int numOfIterations)
        {

            Aes aes = Aes.Create("AesManaged");
            aes.KeySize = 256;
            aes.BlockSize = 128;

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, ASCIIEncoding.ASCII.GetBytes(salt), numOfIterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;

            FileStream inputStream = new FileStream(inputFile, FileMode.Open);
            FileStream outputStream = new FileStream(outputFile, FileMode.Create);
            CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            int data;
            while ((data = inputStream.ReadByte()) != -1)
            {
                cryptoStream.WriteByte((byte)data);
            }

            inputStream.Close();
            cryptoStream.Close();
            outputStream.Close();

        }

        /// <summary>
        /// This function decrypts a file using AES encryption.
        /// </summary>
        /// <param name="inputFile">Path to the file to encrypt</param>
        /// <param name="outputFile">The encrypted output file</param>
        /// <param name="password">The password for derivation of a key for AES encryption</param>
        /// <param name="salt">The salt for derivation of a key for AES encryption</param>
        /// <param name="numOfIterations">The num of iterations for derivation of a key for AES encryption</param>
        public static void AesDecrypt(String inputFile, String outputFile, String password, String salt, int numOfIterations)
        {
            Aes aes = Aes.Create("AesManaged");
            aes.KeySize = 256;
            aes.BlockSize = 128;

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, ASCIIEncoding.ASCII.GetBytes(salt), numOfIterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;

            FileStream inputStream = new FileStream(inputFile, FileMode.Open);
            CryptoStream cryptoStream = new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            FileStream outputStream = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cryptoStream.ReadByte()) != -1)
            {
                outputStream.WriteByte((byte)data);
            }

            outputStream.Close();
            cryptoStream.Close();
            inputStream.Close();
        }
    }
}
