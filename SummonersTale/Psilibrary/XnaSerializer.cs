﻿using System;
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
