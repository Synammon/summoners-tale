using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Security.Cryptography;

namespace ConversationEditor
{
    public static class XnaSerializer
    {
        public static void Serialize<T>(string filename, T data)
        {
            XmlWriterSettings settings = new()
            {
                Indent = true
            };

            using XmlWriter writer = XmlWriter.Create(filename, settings);
            IntermediateSerializer.Serialize<T>(writer, data, null);
        }

        public static void SerializeEncrypted<T>(string filename, T data)
        {
            StringBuilder sb = new();

            XmlWriter xmlWriter = XmlWriter.Create(sb);
            using (XmlWriter writer = xmlWriter)
            {
                IntermediateSerializer.Serialize<T>(writer, data, null);
            }

            string str = sb.ToString();
            byte[] bytes;

            byte[] IV = new byte[]
            {
                067, 197, 032, 010, 211, 090, 192, 076,
                054, 154, 111, 023, 243, 071, 132, 090
            };

            byte[] Key = new byte[]
            {
                067, 090, 197, 043, 049, 029, 178, 211,
                127, 255, 097, 233, 162, 067, 111, 022,
            };

            bytes = EncryptString(str, IV, Key);

            using (Stream writer = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using BinaryWriter binaryWriter = new(writer);
                binaryWriter.Write(bytes, 0, bytes.Length);
            }
        }

        private static byte[] EncryptString(string str, byte[] iv, byte[] key)
        {
            byte[] bytes;

            using (Aes aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = key;

                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

                using MemoryStream stream = new();
                using CryptoStream cryptoStream = new(
                    stream,
                    encryptor,
                    CryptoStreamMode.Write);
                StreamWriter streamWriter = new(cryptoStream);
                using (StreamWriter writer = streamWriter)
                {
                    writer.Write(str);
                }

                bytes = stream.ToArray();
            }

            return bytes;
        }

        private static string DecryptByteArray(byte[] bytes, byte[] iv, byte[] key)
        {
            string decrypted;

            using (Aes aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = key;

                ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream stream = new(bytes);
                using CryptoStream cryptoStream = new(
                    stream,
                    encryptor,
                    CryptoStreamMode.Read);
                using StreamReader reader = new(cryptoStream);
                decrypted = reader.ReadToEnd();
            }

            return decrypted;
        }

        public static T DeserializeDecrypted<T>(string filename)
        {
            T data;

            byte[] bytes;

            using (Stream reader = new FileStream(filename, FileMode.Open))
            {
                using BinaryReader binaryReader = new(reader);
                bytes = new byte[reader.Length];
                reader.Read(bytes, 0, (int)reader.Length);
            }


            byte[] IV = new byte[]
            {
                067, 197, 032, 010, 211, 090, 192, 076,
                054, 154, 111, 023, 243, 071, 132, 090
            };

            byte[] Key = new byte[]
            {
                067, 090, 197, 043, 049, 029, 178, 211,
                127, 255, 097, 233, 162, 067, 111, 022,
            };

            string decrypted = DecryptByteArray(bytes, IV, Key);
            bytes = Encoding.Unicode.GetBytes(decrypted);

            using (MemoryStream stream = new(bytes))
            {
                using XmlReader reader = XmlReader.Create(stream);
                reader.Read();
                data = IntermediateSerializer.Deserialize<T>(reader, null);
            }

            return data;
        }

        public static T Deserialize<T>(string filename)
        {
            T data = default;

            try
            {
                using FileStream stream = new(filename, FileMode.Open);
                using XmlReader reader = XmlReader.Create(stream);
                reader.Read();
                data = IntermediateSerializer.Deserialize<T>(reader, null);
            }
            catch (Exception)
            {

            }
            return data;
        }
    }
}
