
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
        /// This function support doesn't crunched textures. Use <see cref="ImageConversion.LoadCrunchedDXTImage"/> instead.
        ///
        /// Call <see cref="Texture2D.Apply"/> after setting image data to actually upload it to the GPU.
        /// </summary>
        /// <param name="tex">The texture to load the image into.</param>
        /// <param name="data">The byte array containing the image data to load.</param>
        /// <returns>Returns true if the data can be loaded, false otherwise.</returns>
        public static bool LoadDXTImage(this Texture2D tex, byte[] data)
        {
            // DDS File Format Specs:
            // https://docs.microsoft.com/en-us/windows/desktop/direct3ddds/dx-graphics-dds-pguide

            if (data[4] != 124 || !(data[84] == 'D' && data[85] == 'X' && data[86] == 'T')) //this header byte should be 124 for DDS image files
            {
                Debug.LogError("Invalid DDS DXT texture. Unable to read");
                return false;
            }

            byte formatByte = data[87];
            TextureFormat format;
            if(formatByte == '1')
            {
                format = TextureFormat.DXT1;
            }
            else if(formatByte == '5')
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

        /// <summary>
        /// Loads CRN DXT-Encoded image byte array into a texture.
        ///
        /// This function replaces texture contents with new image data. After LoadImage, texture size and format might
        /// change. DXT1 Crunched files are loaded into
        /// <see cref="TextureFormat.DXT1Crunched"/> format, DXT5 Crunched files are loaded into
        /// <see cref="TextureFormat.DXT5Crunched"/> format.
        ///
        /// Call <see cref="Texture2D.Apply"/> after setting image data to actually upload it to the GPU.
        /// </summary>
        /// <param name="tex">The texture to load the image into.</param>
        /// <param name="data">The byte array containing the image data to load.</param>
        /// <returns>Returns true if the data can be loaded, false otherwise.</returns>
        public static bool LoadCrunchedDXTImage(this Texture2D tex, byte[] data)
        {
            // CRN File Format Specs:
            // https://github.com/DaemonEngine/crunch/blob/master/inc/crn_defs.h
            // https://forum.unity.com/threads/loading-crunched-dxt-at-runtime.497861/#post-3238253


            ushort width = System.BitConverter.ToUInt16(data, 12);
            ushort height = System.BitConverter.ToUInt16(data, 14);


            byte formatByte = data[18];
            TextureFormat format;
            if(formatByte == 0)
            {
                format = TextureFormat.DXT1Crunched;
            }
            else if (formatByte == 2) {
                format = TextureFormat.DXT5Crunched;
            }
            else
            {
                Debug.LogError("Invalid TextureFormat. Only Crunched DXT1 and DXT5 formats are supported by this method.");
                return false;
            }

            // tex.format = format;
            tex.width = checked((int) width);
            tex.height = checked((int) height);
            tex.LoadRawTextureData(data);

            return true;
        }
    }
}
