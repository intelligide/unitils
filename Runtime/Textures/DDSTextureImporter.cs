using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace unitils.textures
{
    public enum DDSFormat : uint
    {
        Invalid = 0,
        DXT1 = 0x44585431,
        DXT2 = 0x44585432,
        DXT3 = 0x44585433,
        DXT4 = 0x44585434,
        DXT5 = 0x44585435,
        DX10 = 0x44583130,
    };

    [Flags]
    public enum DDSPixelFormatFlags : uint
    {
        None = 0x0,
        ALPHAPIXELS = 0x1,
        ALPHA = 0x2,
        FOURCC = 0x4,
        RGB = 0x40,
        YUV = 0x200,
        LUMINANCE = 0x20000,
    }

    public struct DDSPixelFormat
    {
        /// <summary>
        ///  Structure size
        ///
        /// It must be set to 32 (bytes).
        /// </summary>
        public uint Size;
        public DDSPixelFormatFlags Flags;
        public DDSFormat FourCC;
        public uint RGBBitCount;
        public uint RBitMask;
        public uint GBitMask;
        public uint BBitMask;
        public uint ABitMask;
    }

    [Flags]
    public enum DDSHeaderFlags : uint
    {
        None = 0x0,

        CAPS = 0x1,
        HEIGHT = 0x2,
        WIDTH = 0x4,
        PITCH = 0x8,
        PIXELFORMAT = 0x1000,
        MIPMAPCOUNT = 0x20000,
        LINEARSIZE = 0x80000,
        DEPTH = 0x800000,
    }

    public class DDSTextureImporter : ITextureImporter
    {
        public const uint DwMagic = 0x20534444;

        /// <summary>
        ///  Size of header.
        ///
        /// It must be set to 124.
        /// </summary>
        public uint HeaderSize;

        public DDSHeaderFlags Flags;

        public uint Height;

        public uint Width;

        public uint PitchOrLinearSize;

        public uint Depth;

        public uint MipMapCount;

        public DDSPixelFormat PixelFormat;

        public uint Caps;

        public uint Caps2;

        public byte[] RawTextureData { get; private set; }

        public TextureFormat TextureFormat
        {
            get {
                switch(PixelFormat.FourCC)
                {
                    case DDSFormat.DXT1:
                        return TextureFormat.DXT1;
                    case DDSFormat.DXT5:
                        return TextureFormat.DXT5;
                    default:
                        throw new Exception("Invalid Texture Format. The texture format is not supported by Unity.");
                }
            }
        }

        public DDSTextureImporter(byte[] data)
        {
            var converter = new EndianBitConverter();
            uint magic = converter.ToUInt32(data, 0);
            if(magic != DwMagic)
            {
                converter.InvertEndian = true;
                magic = converter.ToUInt32(data, 0);
                if(magic != DwMagic)
                {
                    throw new Exception("Invalid DDS Texture.");
                }
            }

            HeaderSize = converter.ToUInt32(data, 4);
            Flags = (DDSHeaderFlags) converter.ToUInt32(data, 8);
            Height = converter.ToUInt32(data, 12);
            Width = converter.ToUInt32(data, 16);
            PitchOrLinearSize = converter.ToUInt32(data, 20);
            Depth = converter.ToUInt32(data, 24);
            MipMapCount = converter.ToUInt32(data, 28);

            PixelFormat.Size = converter.ToUInt32(data, 76);
            PixelFormat.Flags = (DDSPixelFormatFlags) converter.ToUInt32(data, 80);
            converter.InvertEndian = true;
            PixelFormat.FourCC = (DDSFormat) converter.ToUInt32(data, 84);
            converter.InvertEndian = false;
            PixelFormat.RGBBitCount = converter.ToUInt32(data, 88);
            PixelFormat.RBitMask = converter.ToUInt32(data, 92);
            PixelFormat.GBitMask = converter.ToUInt32(data, 96);
            PixelFormat.BBitMask = converter.ToUInt32(data, 100);
            PixelFormat.ABitMask = converter.ToUInt32(data, 104);

            Caps = converter.ToUInt32(data, 108);
            Caps2 = converter.ToUInt32(data, 112);

            RawTextureData = new byte[data.Length - 128];
            Buffer.BlockCopy(data, 128, RawTextureData, 0, data.Length - 128);
        }

        public Texture2D GetTexture2D()
        {
            var tex = new Texture2D((int)Width, (int)Height, TextureFormat, MipMapCount > 0);
            tex.LoadRawTextureData(RawTextureData);
            return tex;
        }
    }
}
