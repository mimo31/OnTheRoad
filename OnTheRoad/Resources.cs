using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace OnTheRoad
{
    public static class Resources
    {
        public static readonly string RootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OnTheRoad";
        public static readonly string ResourceDirectory = RootDirectory + "\\Resources";
        private static List<Texture> loadedTextures = new List<Texture>();
        private static List<Texture> scaledTextures = new List<Texture>();

        public static Bitmap GetTexture(string name)
        {
            int onIndex = -1;
            for (int i = 0; i < loadedTextures.Count; i++)
            {
                if (loadedTextures[i].Name.Equals(name))
                {
                    onIndex = i;
                    break;
                }
            }
            if (onIndex == -1)
            {
                string path = ResourceDirectory + "\\" + name;
                if (File.Exists(path))
                {
                    Bitmap bitmap = new Bitmap(path);
                    Texture texture = new Texture()
                    {
                        Image = bitmap,
                        Name = name
                    };
                    loadedTextures.Add(texture);
                    return bitmap;
                }
                else
                {
                    throw new IOException("Resource \"" + name + "\" doesn't exist.");
                }
            }
            else
            {
                return loadedTextures[onIndex].Image;
            }
        }

        public static Bitmap GetTexture(string name, Size size)
        {
            for (int i = 0; i < scaledTextures.Count; i++)
            {
                if (scaledTextures[i].Name.Equals(name))
                {
                    if (!(scaledTextures[i].Image.Width == size.Width) || !(scaledTextures[i].Image.Height == size.Height))
                    {
                        scaledTextures[i].Image = new Bitmap(GetTexture(name), size);
                    }
                    return scaledTextures[i].Image;
                }
            }
            Bitmap bitmap = new Bitmap(GetTexture(name), size);
            Texture texture = new Texture()
            {
                Image = bitmap,
                Name = name
            };
            scaledTextures.Add(texture);
            return bitmap;
        }

        public static Bitmap GetTexture(string name, float width)
        {
            if (width >= 1)
            {
                Bitmap original = GetTexture(name);
                return GetTexture(name, new Size((int)width, (int)(original.Height / (float)original.Width * width)));
            }
            else
            {
                return new Bitmap(1, 1);
            }
        }
    }

    internal class Texture
    {
        public Bitmap Image { get; set; }
        public string Name { get; set; }
    }
}
