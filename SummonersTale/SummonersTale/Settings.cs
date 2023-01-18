using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersTale
{
    public class Settings
    {
        public const int BaseWidth = 1280;
        public const int BaseHeight = 720;

        private static float musicVolume = 0.5f;
        private static float soundVolume = 0.5f;
        private static Point resolution = new(BaseWidth, BaseHeight);

        public static Vector2 Scale
        {
            get { return new((float)resolution.X / BaseWidth, (float)resolution.Y / BaseHeight); }
        }

        public static float MusicVolume
        {
            get
            {
                return musicVolume;
            }

            set
            {
                musicVolume = MathHelper.Clamp(value, 0, 1f);
            }
        }

        public static float SoundVolume
        {
            get
            {
                return soundVolume;
            }

            set
            {
                soundVolume = MathHelper.Clamp(value, 0, 1f);
            }
        }

        public static Point Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        public static void Save()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path += "/ASummonersTale/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += "settings.bin";

            using FileStream stream = new(path, FileMode.Create, FileAccess.Write);
            using BinaryWriter writer = new(stream);

            writer.Write(soundVolume);
            writer.Write(musicVolume);
            writer.Write(resolution.X);
            writer.Write(resolution.Y);
            writer.Close();
            stream.Close();
        }

        public static void Load()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path += @"/ASummonersTale/settings.bin";

            if (!File.Exists(path))
            {
                Save();
            }

            using FileStream stream = new(path, FileMode.Open, FileAccess.Read);
            using BinaryReader reader = new(stream);

            soundVolume = reader.ReadSingle();
            musicVolume = reader.ReadSingle();
            resolution = new(reader.ReadInt32(), reader.ReadInt32());
            reader.Close();
            stream.Close();
        }
    }
}
