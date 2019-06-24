using UnityEngine;

namespace unitils.textures
{
    public interface ITextureImporter
    {
        byte[] RawTextureData
        {
            get;
        }

        TextureFormat TextureFormat
        {
            get;
        }

        Texture2D GetTexture2D();
    }
}
