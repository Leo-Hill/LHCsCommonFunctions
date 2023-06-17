using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LHCommonFunctions.Source
{
    public static class LHCryptoFunctions
    {
        //This function encrypts a file using a password and a salt
        public static void vAesEncrypt(String qsInputFile, String qsOutputFile, String qsPassword, String qsSalt, int qiNumOfIterations)
        {

            Aes aes = Aes.Create("AesManaged");
            aes.KeySize = 256;
            aes.BlockSize = 128;

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(qsPassword, ASCIIEncoding.ASCII.GetBytes(qsSalt), qiNumOfIterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;

            FileStream FsOutput = new FileStream(qsOutputFile, FileMode.Create);
            CryptoStream CsOutput = new CryptoStream(FsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream FsInput = new FileStream(qsInputFile, FileMode.Open);

            int data;
            while ((data = FsInput.ReadByte()) != -1)
            {
                CsOutput.WriteByte((byte)data);
            }

            FsInput.Close();
            CsOutput.Close();
            FsOutput.Close();

        }

        //This function decrypts a file using a password and a salt
        public static void vAesDecrypt(String qsInputFile, String qsOutputFile, String qsPassword, String qsSalt, int qiNumOfIterations)
        {
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
            while ((data = CsInput.ReadByte()) != -1)
            {
                FsOutput.WriteByte((byte)data);
            }

            FsOutput.Close();
            CsInput.Close();
            FsInput.Close();
        }
    }
}
