
using System;

namespace UnityEngine
{
    public static class ImageConversion
    {
        /// <summary>
        /// Loads DXT-Encoded image byte array into a texture.
        /// 
        /// This function replaces texture contents with new image data. After LoadImage, texture size and format might 
        /// change. DXT1 files are loaded into <see cref="TextureFormat.DXT1"/> format, DXT5 files are loaded into 
        /// <see cref="TextureFormat.DXT5"/> format.
        /// 
        /// This function support crunched textures. DXT1 Crunched files are loaded into 
        /// <see cref="TextureFormat.DXT1Crunched"/> format, DXT5 Crunched files are loaded into 
        /// <see cref="TextureFormat.DXT5Crunched"/> format.
        /// 
        /// Call <see cref="Texture2D.Apply"/> after setting image data to actually upload it to the GPU.
        /// </summary>
        /// <param name="tex">The texture to load the image into.</param>
        /// <param name="data">The byte array containing the image data to load.</param>
        /// <returns></returns>
        public static bool LoadDXTImage(this Texture2D tex, byte[] data)
        {
            // DDS File Format Specs: 
            // https://docs.microsoft.com/en-us/windows/desktop/direct3ddds/dx-graphics-dds-pguide

            if (data[4] != 124 || !(data[84] == 'D' && data[85] == 'X' && data[86] == 'T')) //this header byte should be 124 for DDS image files
            {
                Debug.LogError("Invalid DDS DXT texture. Unable to read");
                return false;
            }

            TextureFormat format;
            if(data[87] == '1')
            {
                format = TextureFormat.DXT1;
            }
            else if(data[87] == '5')
            {
                format = TextureFormat.DXT5;
            }
            else
            {
                Debug.LogError("Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");
                return false;
            }

            uint height = System.BitConverter.ToUInt32(data, 12);
            uint width = System.BitConverter.ToUInt32(data, 16);

            const int DDS_HEADER_SIZE = 128;
            byte[] dxtBytes = new byte[data.Length - DDS_HEADER_SIZE];
            Buffer.BlockCopy(data, DDS_HEADER_SIZE, dxtBytes, 0, data.Length - DDS_HEADER_SIZE);

            // tex.format = format;
            tex.width = checked((int) width);
            tex.height = checked((int) height);
            tex.LoadRawTextureData(dxtBytes);

            return true;
        }
    }
}