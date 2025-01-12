﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace GMap.NET.Internals
{
    /// <summary>
    ///     etc functions...
    /// </summary>
    internal class Stuff
    {
        public static string EnumToString(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int X, int Y);

        public static readonly Random random = new Random();

        public static void Shuffle<T>(List<T> deck)
        {
            int N = deck.Count;

            for (int i = 0; i < N; ++i)
            {
                int r = i + random.Next(N - i);
                var t = deck[r];
                deck[r] = deck[i];
                deck[i] = t;
            }
        }

        public static MemoryStream CopyStream(Stream inputStream, bool SeekOriginBegin)
        {
            const int readSize = 32 * 1024;
            var buffer = new byte[readSize];
            var ms = new MemoryStream();
            {
                int count;
                while ((count = inputStream.Read(buffer, 0, readSize)) > 0)
                {
                    ms.Write(buffer, 0, count);
                }
            }

            if (SeekOriginBegin)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public static bool IsRunningOnVistaOrLater()
        {
            var os = Environment.OSVersion;

            if (os.Platform == PlatformID.Win32NT)
            {
                var vs = os.Version;

                if (vs.Major >= 6 && vs.Minor >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsRunningOnWin7orLater()
        {
            var os = Environment.OSVersion;

            if (os.Platform == PlatformID.Win32NT)
            {
                var vs = os.Version;

                if (vs.Major >= 6 && vs.Minor > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static void RemoveInvalidPathSymbols(ref string url)
        {
#if !PocketPC
            var ilg = Path.GetInvalidFileNameChars();
#else
            char[] ilg = new char[41];
            for(int i = 0; i < 32; i++)
               ilg[i] = (char) i;

            ilg[32] = '"';
            ilg[33] = '<';
            ilg[34] = '>';
            ilg[35] = '|';
            ilg[36] = '?';
            ilg[37] = ':';
            ilg[38] = '/';
            ilg[39] = '\\';
            ilg[39] = '*';
#endif
            foreach (char c in ilg)
            {
                url = url.Replace(c, '_');
            }
        }

        #region -- encryption --

        static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;

            using (var HashProvider = new SHA1CryptoServiceProvider())
            {
                var TDESKey = HashProvider.ComputeHash(Encoding.UTF8.GetBytes(Passphrase));
                Array.Resize(ref TDESKey, 16);

                using (var TDESAlgorithm = new TripleDESCryptoServiceProvider())
                {
                    TDESAlgorithm.Key = TDESKey;
                    TDESAlgorithm.Mode = CipherMode.ECB;
                    TDESAlgorithm.Padding = PaddingMode.PKCS7;

                    var DataToEncrypt = Encoding.UTF8.GetBytes(Message);

                    // Step 5. Attempt to encrypt the string
                    try
                    {
                        using (var Encryptor = TDESAlgorithm.CreateEncryptor())
                        {
                            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
                        }
                    }
                    finally
                    {
                        // Clear the TripleDes and Hashprovider services of any sensitive information
                        TDESAlgorithm.Clear();
                        HashProvider.Clear();
                    }
                }
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;

            using (var HashProvider = new SHA1CryptoServiceProvider())
            {
                var TDESKey = HashProvider.ComputeHash(Encoding.UTF8.GetBytes(Passphrase));
                Array.Resize(ref TDESKey, 16);

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                using (var TDESAlgorithm = new TripleDESCryptoServiceProvider())
                {
                    // Step 3. Setup the decoder
                    TDESAlgorithm.Key = TDESKey;
                    TDESAlgorithm.Mode = CipherMode.ECB;
                    TDESAlgorithm.Padding = PaddingMode.PKCS7;

                    // Step 4. Convert the input string to a byte[]
                    var DataToDecrypt = Convert.FromBase64String(Message);

                    // Step 5. Attempt to decrypt the string
                    try
                    {
                        using (var Decryptor = TDESAlgorithm.CreateDecryptor())
                        {
                            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                        }
                    }
                    finally
                    {
                        // Clear the TripleDes and Hashprovider services of any sensitive information
                        TDESAlgorithm.Clear();
                        HashProvider.Clear();
                    }
                }
            }

            // Step 6. Return the decrypted string in UTF8 format
            return Encoding.UTF8.GetString(Results, 0, Results.Length);
        }

        public static string EncryptString(string Message)
        {
            return EncryptString(Message, manifesto);
        }

        public static string GString(string Message)
        {
            string ret = DecryptString(Message, manifesto);

            return ret;
        }

        static readonly string manifesto =
            "GMap.NET is great and Powerful, Free, cross platform, open source .NET control.";

        #endregion
    }

#if PocketPC
   static class Monitor
   {
      static readonly OpenNETCF.Threading.Monitor2 wait = new OpenNETCF.Threading.Monitor2();

      public static void Enter(Stack<LoadTask> tileLoadQueue)
      {
         wait.Enter();
      }

      public static void Exit(Stack<LoadTask> tileLoadQueue)
      {
         wait.Exit();
      }

      public static void Wait(Stack<LoadTask> tileLoadQueue)
      {
         wait.Wait();
      }

      public static bool Wait(Stack<LoadTask> tileLoadQueue, int WaitForTileLoadThreadTimeout, bool p)
      {
         wait.Wait();
         return true;
      }

      internal static void PulseAll(Stack<LoadTask> tileLoadQueue)
      {
         wait.PulseAll();
      }
   }
#endif
}
