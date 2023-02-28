using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.TileEngine
{
    public class TileFrame
    {
        public string Name { get; set; }
        public Texture2D Texture { get; set; }

        private TileFrame()
        {

        }

        public TileFrame(string name, Texture2D texture) 
        { 
            Name = name;
            Texture = texture;
        }
    }

    public class TileFrames
    {
        public List<TileFrame> Frames { get; private set; } = new();

        public void LoadContent(ContentManager Content)
        {
            Frames.Clear();

            try
            {
                string folder = string.Format("{0}/Tiles", Content.RootDirectory);
                string root = Content.RootDirectory;

                foreach (var r in Directory.GetFiles(folder))
                {
                    string path = Path.GetFileNameWithoutExtension(r);
                    
                    StringBuilder build = new StringBuilder()
                        .Append("Tiles/")
                        .Append(path);

                    TileFrame frame = new(
                        Path.GetFileNameWithoutExtension(r), 
                        Content.Load<Texture2D>(build.ToString()));
                    Frames.Add(frame);
                }
            }
            catch(Exception ex)
            {
            }
        }
    }
}
