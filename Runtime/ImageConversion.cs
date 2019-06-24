
using System;
using unitils.crunch;
using unitils.textures;
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
        /// </summary>
        /// <param name="tex">The texture to load the image into.</param>
        /// <param name="data">The byte array containing the image data to load.</param>
        /// <returns>Returns true if the data can be loaded, false otherwise.</returns>
        public static bool LoadDDSImage(byte[] data, out Texture2D tex)
        {
            // DDS File Format Specs:
            // https://docs.microsoft.com/en-us/windows/desktop/direct3ddds/dx-graphics-dds-pguide

            var ddsImporter = new DDSTextureImporter(data);

            try
            {
                tex = ddsImporter.GetTexture2D();
                tex.Apply();
                return true;
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                tex = null;
                return false;
            }
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

            var crnImporter = new CRNTextureImporter(data);

            try
            {
                tex = crnImporter.GetTexture2D();
                tex.Apply();
                return true;
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                tex = null;
                return false;
            }
        }

        /// <summary>
        /// Loads KTX image byte array into a texture.
        ///
        /// This function replaces texture contents with new image data.
        /// </summary>
        /// <param name="data">The byte array containing the image data to load.</param>
        /// <param name="tex">The texture to load the image into.</param>
        /// <returns>Returns true if the data can be loaded, false otherwise.</returns>
        public static bool LoadKTXImage(byte[] data, out Texture2D tex)
        {
            var ktxImporter = new KTXTextureImporter(data);

            try
            {
                tex = ktxImporter.GetTexture2D();
                tex.Apply();
                return true;
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                tex = null;
                return false;
            }
        }
    }
}
