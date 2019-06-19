
using System;
using unitils.crunch;
using UnityEngine;

namespace unitils
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
        public static bool LoadDXTImage(byte[] data, out Texture2D tex)
        {
            // DDS File Format Specs:
            // https://docs.microsoft.com/en-us/windows/desktop/direct3ddds/dx-graphics-dds-pguide

            if (data[4] != 124 || !(data[84] == 'D' && data[85] == 'X' && data[86] == 'T')) //this header byte should be 124 for DDS image files
            {
                Debug.LogError("Invalid DDS DXT texture. Unable to read");
                tex = null;
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
                tex = null;
                return false;
            }

            int height = data[13] * 256 + data[12];
            int width = data[17] * 256 + data[16];

            const int DDS_HEADER_SIZE = 124 + 4;
            byte[] dxtBytes = new byte[data.Length - DDS_HEADER_SIZE];
            Buffer.BlockCopy(data, DDS_HEADER_SIZE, dxtBytes, 0, data.Length - DDS_HEADER_SIZE);
            tex = new Texture2D(width, height, format, true);
            tex.LoadRawTextureData(dxtBytes);
            tex.Apply();

            return true;
        }

        /// <summary>
        /// Loads CRN DXT-Encoded image byte array into a texture.
        ///
        /// This function replaces texture contents with new image data. After LoadImage, texture size and format might
        /// change. DXT1 Crunched files are loaded into
        /// <see cref="TextureFormat.DXT1Crunched"/> format, DXT5 Crunched files are loaded into
        /// <see cref="TextureFormat.DXT5Crunched"/> format, ETC1 Crunched files are loaded into
        /// <see cref="TextureFormat.ETC_RGB4Crunched"/> format, ETC2 Crunched files are loaded into
        /// <see cref="TextureFormat.ETC2_RGBA8Crunched"/> format.
        ///
        /// Call <see cref="Texture2D.Apply"/> after setting image data to actually upload it to the GPU.
        /// </summary>
        /// <param name="tex">The texture to load the image into.</param>
        /// <param name="data">The byte array containing the image data to load.</param>
        /// <returns>Returns true if the data can be loaded, false otherwise.</returns>
        public static bool LoadCrunchedImage(byte[] data, out Texture2D tex)
        {
            // CRN File Format Specs:
            // https://github.com/DaemonEngine/crunch/blob/master/inc/crn_defs.h
            // https://forum.unity.com/threads/loading-crunched-dxt-at-runtime.497861/#post-3238253

            byte formatByte = data[18];

            if(formatByte == (byte) CrnFormat.DXT1 || formatByte == (byte) CrnFormat.DXT5)
            {
                byte[] dxtData = unitils.UnityCRN.Transcode(data);
                if(LoadDXTImage(dxtData, out tex))
                {
                    return false;
                }
            }
            /*else if (formatByte == (byte) CrnFormat.ETC1) {
                format = TextureFormat.ETC_RGB4Crunched;
            }
            else if (formatByte == (byte) CrnFormat.ETC2) {
                format = TextureFormat.ETC2_RGBA8Crunched;
            }*/
            else
            {
                Debug.LogError("Invalid TextureFormat. Only Crunched DXT1, DXT5, ETC1 and ETC2 formats are supported by this method.");
                tex = null;
                return false;
            }

            return true;
        }
    }
}
