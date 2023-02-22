using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessView.View.ResourcesManager
{
    public class TextureManager
    {
        private Dictionary<string, Texture> texturesDictionary;

        public event Action<string, Texture> TextureLoaded;

        public event Action<string> TextureUnloaded;

        public TextureManager()
        {
            this.texturesDictionary = new Dictionary<string, Texture>();
        }

        public Texture GetTexture(string path)
        {
            return this.texturesDictionary[path];
        }

        public void LoadTexture(string textureToLoadPath)
        {
            if (this.texturesDictionary.ContainsKey(textureToLoadPath) == false)
            {
                Texture texture = new Texture(textureToLoadPath);
                texture.Smooth = true;

                this.texturesDictionary.Add(textureToLoadPath, texture);

                this.NotifyTextureLoaded(textureToLoadPath, texture);
            }
        }

        public void UnloadTexture(string textureToUnloadPath)
        {
            if (this.texturesDictionary.ContainsKey(textureToUnloadPath))
            {
                this.texturesDictionary[textureToUnloadPath].Dispose();

                this.texturesDictionary.Remove(textureToUnloadPath);

                this.NotifyTextureUnloaded(textureToUnloadPath);
            }
        }

        private void NotifyTextureLoaded(string path, Texture texture)
        {
            if(this.TextureLoaded != null)
            {
                this.TextureLoaded(path, texture);
            }
        }

        private void NotifyTextureUnloaded(string path)
        {
            if (this.TextureUnloaded != null)
            {
                this.TextureUnloaded(path);
            }
        }
    }
}
