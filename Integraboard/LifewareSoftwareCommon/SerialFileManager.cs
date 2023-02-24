using Lifeware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.DirectoryServices;
using System.Text;
using System.Threading.Tasks;

namespace Lifeware.SoftwareCommon
{

    public static class SerialFileManager
    {
        const string SERIAL_FILENAME = "serial.lis";

        private static string AppDataPath;

        private static string SerialPath;

        static SerialFileManager()
        {
            SerialFileManager.AppDataPath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "\\", GlobalConfig.AppName, "\\");
            SerialFileManager.SerialPath = string.Concat(SerialFileManager.AppDataPath, SERIAL_FILENAME);
            Directory.CreateDirectory(SerialFileManager.AppDataPath);
        }


        public static void DeleteSerialFile()
        {
            if(SerialFileExist())
                File.Delete(SerialFileManager.SerialPath);
        }

        public static Lifeware.Serial ReadSerial()
        {
            return Lifeware.Serial.ReadFromFile(SerialFileManager.SerialPath);
        }

        public static void SaveSerial(Serial s)
        {
            s.SaveToFile(SerialFileManager.SerialPath);
        }

        public static void SaveSerialFile(string sourcePath)
        {
            File.Copy(sourcePath, SerialFileManager.SerialPath);
        }

        public static bool SerialFileExist()
        {
            return File.Exists(SerialFileManager.SerialPath);
        }

        /// <summary>
        /// Verifica que el Serial del software sea válido
        /// </summary>
        /// <returns>True en caso de que el serial sea válido</returns>
        public static async Task<bool> IsValidSerial()
        {
            if (SerialFileExist())
                return CheckIsSerialValidated(ReadSerial(), await GetComputerSidAsync());
            else
                return false;
        }

        private async static Task<string> GetComputerSidAsync()
        {
            return await Task.Factory.StartNew<string>(() => { return GetComputerSid(); });
        }
        public static string GetComputerSid()
        {
            return new SecurityIdentifier((byte[])new DirectoryEntry(string.Format("WinNT://{0},Computer", Environment.MachineName)).Children.Cast<DirectoryEntry>().First().InvokeGet("objectSID"), 0).AccountDomainSid.Value;
        }
        public static bool CheckIsSerialValidated(Serial serial, string sid)
        {
            try
            {
                RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
                RSAParameters rsaParam;
                rsaCSP.FromXmlString(serial.PublicKey);
                rsaParam = rsaCSP.ExportParameters(false);
                rsaCSP.ImportParameters(rsaParam);

                byte[] UserData = System.Text.Encoding.UTF8.GetBytes(serial.SerialNumber + sid + serial.SoftwareGuid); //datos de licencia en base 64
                byte[] signature = Convert.FromBase64String(serial.ActivationCode);
                AsymmetricSignatureDeformatter ASD = new RSAPKCS1SignatureDeformatter(rsaCSP);
                HashAlgorithm ha = new SHA1Managed();
                ASD.SetHashAlgorithm(ha.ToString());
                byte[] HashedUserData;
                string HashedUserDataString;
                HashedUserData = ha.ComputeHash(UserData);
                HashedUserDataString = BitConverter.ToString(HashedUserData);

                bool verificado = ASD.VerifySignature(ha, signature);
                return verificado;
            }
            catch
            {
                return false;
            }

        }

    }
}
