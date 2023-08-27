using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LHCommonFunctions {
    public static class Cryptography {
        /// <summary>
        /// This function encrypts a file using AES encryption
        /// The key for the encryption will be derived from the password and the salt
        /// </summary>
        /// <param name="inputFile">Absolute path of the input file</param>
        /// <param name="outputFile">Absolute path of the encrypted output file</param>
        /// <param name="password">The password to encrypt the file with</param>
        /// <param name="salt">The salt used for the derivation function</param>
        /// <param name="numOfIterations">Number of iterations for the key derivation</param>
        public static void AesEncrypt(String inputFile, String outputFile, String password, String salt, int numOfIterations) {

            Aes aes = Aes.Create("AesManaged");
            aes.KeySize = 256;
            aes.BlockSize = 128;

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, ASCIIEncoding.ASCII.GetBytes(salt), numOfIterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;

            FileStream FsOutput = new FileStream(outputFile, FileMode.Create);
            CryptoStream CsOutput = new CryptoStream(FsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream FsInput = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = FsInput.ReadByte()) != -1) {
                CsOutput.WriteByte((byte)data);
            }

            FsInput.Close();
            CsOutput.Close();
            FsOutput.Close();

        }

        /// <summary>
        /// This function decrypts a file using AES encryption
        /// The key for the decryption will be derived from the password and the salt
        /// </summary>
        /// <param name="inputFile">Absolute path of the encrypted input file</param>
        /// <param name="outputFile">Absolute path of the decrypted output file</param>
        /// <param name="password">The password to encrypt the file with</param>
        /// <param name="salt">The salt used for the derivation function</param>
        /// <param name="numOfIterations">Number of iterations for the key derivation</param>
        public static void vAesDecrypt(String qsInputFile, String qsOutputFile, String qsPassword, String qsSalt, int qiNumOfIterations) {
            Aes aes = Aes.Create("AesManaged");
            aes.KeySize = 256;
            aes.BlockSize = 128;

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(qsPassword, ASCIIEncoding.ASCII.GetBytes(qsSalt), qiNumOfIterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;

            FileStream FsInput = new FileStream(qsInputFile, FileMode.Open);
            CryptoStream CsInput = new CryptoStream(FsInput, aes.CreateDecryptor(), CryptoStreamMode.Read);
            FileStream FsOutput = new FileStream(qsOutputFile, FileMode.Create);

            int data;
            while ((data = CsInput.ReadByte()) != -1) {
                FsOutput.WriteByte((byte)data);
            }

            FsOutput.Close();
            CsInput.Close();
            FsInput.Close();
        }
    }
}
