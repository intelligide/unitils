using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace unitils.textures
{
    // Pixel formats (various internal, base, and base internal formats)
    public enum KTXPixelFormat : UInt32
    {
        R8 = 0x8229,
        R8UI = 0x8232,
        RGB8 = 0x8051,
        SRGB8 = 0x8C41,
        SRGB = 0x8C40,
        SRGB_ALPHA = 0x8C42,
        SRGB8_ALPHA8 = 0x8C43,
        RGBA8 = 0x8058,
        RED = 0x1903,
        GREEN = 0x1904,
        BLUE = 0x1905,
        ALPHA = 0x1906,
        RG = 0x8227,
        RGB = 0x1907,
        RGBA = 0x1908,
        BGR = 0x80E0,
        BGRA = 0x80E1,
        RED_INTEGER = 0x8D94,
        GREEN_INTEGER = 0x8D95,
        BLUE_INTEGER = 0x8D96,
        ALPHA_INTEGER = 0x8D97,
        RGB_INTEGER = 0x8D98,
        RGBA_INTEGER = 0x8D99,
        BGR_INTEGER = 0x8D9A,
        BGRA_INTEGER = 0x8D9B,
        LUMINANCE = 0x1909,
        LUMINANCE_ALPHA = 0x190A,
        RG_INTEGER = 0x8228,
        RG8 = 0x822B,
        ALPHA8 = 0x803C,
        LUMINANCE8 = 0x8040,
        LUMINANCE8_ALPHA8 = 0x8045,

        // Compressed pixel data formats: ETC1, DXT1, DXT3, DXT5
        ETC1_RGB8_OES = 0x8D64,
        COMPRESSED_R11_EAC = 0x9270,
        COMPRESSED_SIGNED_R11_EAC = 0x9271,
        COMPRESSED_RG11_EAC = 0x9272,
        COMPRESSED_SIGNED_RG11_EAC = 0x9273,
        COMPRESSED_RGB8_ETC2 = 0x9274,
        COMPRESSED_SRGB8_ETC2 = 0x9275,
        COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9276,
        COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9277,
        COMPRESSED_RGBA8_ETC2_EAC = 0x9278,
        RGB_S3TC = 0x83A0,
        RGB4_S3TC = 0x83A1,
        COMPRESSED_RGB_S3TC_DXT1_EXT = 0x83F0,
        COMPRESSED_RGBA_S3TC_DXT1_EXT = 0x83F1,
        COMPRESSED_SRGB_S3TC_DXT1_EXT = 0x8C4C,
        COMPRESSED_SRGB_ALPHA_S3TC_DXT1_EXT = 0x8C4D,
        RGBA_S3TC = 0x83A2,
        RGBA4_S3TC = 0x83A3,
        COMPRESSED_RGBA_S3TC_DXT3_EXT = 0x83F2,
        COMPRESSED_SRGB_ALPHA_S3TC_DXT3_EXT = 0x8C4E,
        COMPRESSED_RGBA_S3TC_DXT5_EXT = 0x83F3,
        COMPRESSED_SRGB_ALPHA_S3TC_DXT5_EXT = 0x8C4F,
        RGBA_DXT5_S3TC = 0x83A4,
        RGBA4_DXT5_S3TC = 0x83A5,
        COMPRESSED_RED_RGTC1_EXT = 0x8DBB,
        COMPRESSED_SIGNED_RED_RGTC1_EXT = 0x8DBC,
        COMPRESSED_RED_GREEN_RGTC2_EXT = 0x8DBD,
        COMPRESSED_SIGNED_RED_GREEN_RGTC2_EXT = 0x8DBE,
        COMPRESSED_LUMINANCE_LATC1_EXT = 0x8C70,
        COMPRESSED_SIGNED_LUMINANCE_LATC1_EXT = 0x8C71,
        COMPRESSED_LUMINANCE_ALPHA_LATC2_EXT = 0x8C72,
        COMPRESSED_SIGNED_LUMINANCE_ALPHA_LATC2_EXT = 0x8C73,
    };

    public class KTXTextureImporter : ITextureImporter
    {
        public byte[] Identifier { get; private set; }

        public UInt32 Endianess { get; private set; }

        public UInt32 GlType { get; private set; }

        public UInt32 GlTypeSize { get; private set; }

        public KTXPixelFormat GlFormat { get; private set; }

        public KTXPixelFormat GlInternalFormat { get; private set; }

        public KTXPixelFormat GlBaseInternalFormat { get; private set; }

        public UInt32 Width { get; private set; }

        public UInt32 Height { get; private set; }

        public UInt32 Depth { get; private set; }

        public UInt32 NumArrayElements { get; private set; }

        public UInt32 NumFaces { get; private set; }

        public UInt32 MipMapCount { get; private set; }

        public Dictionary<string, byte[]> KeyValuePairs { get; private set; }

        public UInt32 DataOffset { get; private set; }

        public byte[] RawTextureData { get; private set; }

        public TextureFormat TextureFormat
        {
            get {
                switch(GlInternalFormat)
                {
                    case KTXPixelFormat.ETC1_RGB8_OES:
                        return TextureFormat.ETC_RGB4;
                    case KTXPixelFormat.COMPRESSED_RGB8_ETC2:
                        return TextureFormat.ETC2_RGB;
                    case KTXPixelFormat.COMPRESSED_RGBA8_ETC2_EAC:
                        return TextureFormat.ETC2_RGBA8;
                    case KTXPixelFormat.COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2:
                        return TextureFormat.ETC2_RGBA1;
                    case KTXPixelFormat.COMPRESSED_R11_EAC:
                        return TextureFormat.EAC_R;
                    case KTXPixelFormat.COMPRESSED_SIGNED_R11_EAC:
                        return TextureFormat.EAC_R_SIGNED;
                    case KTXPixelFormat.COMPRESSED_RG11_EAC:
                        return TextureFormat.EAC_RG;
                    case KTXPixelFormat.COMPRESSED_SIGNED_RG11_EAC:
                        return TextureFormat.EAC_RG_SIGNED;
                    default:
                        throw new Exception("Invalid Texture Format. The texture format is not supported by Unity.");
                }
            }
        }

        public KTXTextureImporter(byte[] data)
        {
            Identifier = new byte[12];
            Buffer.BlockCopy(data, 0, Identifier, 0, 12);

            Endianess = BitConverter.ToUInt32(data, 12);
            GlType = BitConverter.ToUInt32(data, 16);
            GlTypeSize = BitConverter.ToUInt32(data, 20);
            GlFormat = (KTXPixelFormat) BitConverter.ToUInt32(data, 24);
            GlInternalFormat = (KTXPixelFormat) BitConverter.ToUInt32(data, 28);
            GlBaseInternalFormat = (KTXPixelFormat) BitConverter.ToUInt32(data, 32);
            Width = BitConverter.ToUInt32(data, 36);
            Height = BitConverter.ToUInt32(data, 40);
            Depth = BitConverter.ToUInt32(data, 44);
            NumArrayElements = BitConverter.ToUInt32(data, 48);
            NumFaces = BitConverter.ToUInt32(data, 52);
            MipMapCount = BitConverter.ToUInt32(data, 56);

            var bytesOfKeyValueData = BitConverter.ToUInt32(data, 60);
            if(bytesOfKeyValueData > 0)
            {
                KeyValuePairs = new Dictionary<string, byte[]>();

                uint curOffset = 64;
                while(bytesOfKeyValueData + 64 > curOffset)
                {
                    var keyAndValueByteSize = BitConverter.ToUInt32(data, (int) curOffset);
                    curOffset +=4;

                    string key = null;
                    uint valueOffset = 0;
                    for(uint i = 0; i < keyAndValueByteSize; i++)
                    {
                        if(data[i + curOffset] == 0x00)
                        {
                            key = Encoding.UTF8.GetString(data, (int) curOffset, (int) i);
                            valueOffset = i + 1;
                            break;
                        }
                    }
                    uint valueLen = keyAndValueByteSize - valueOffset - 1;
                    byte[] value = new byte[valueLen];
                    Buffer.BlockCopy(data, (int)(curOffset + valueOffset), value, 0, (int)(valueLen));
                    curOffset += keyAndValueByteSize;

                    curOffset += 3 - ((keyAndValueByteSize + 3) % 4); // padding

                    KeyValuePairs.Add(key, value);
                }
            }

            DataOffset = 64 + bytesOfKeyValueData;


            RawTextureData = new byte[data.Length - MipMapCount * 4]; // TODO: Remove all paddings

            uint dstOffset = 0;
            uint srcOffset = DataOffset;
            for(int mipmapIdx = 0; mipmapIdx < MipMapCount; mipmapIdx++)
            {
                var mipmapSize = BitConverter.ToUInt32(data, (int) srcOffset);

                srcOffset += 4 + GetPadding(mipmapSize);

                Buffer.BlockCopy(data, (int)srcOffset, RawTextureData, (int)dstOffset, (int)mipmapSize);

                // TODO: remove cube padding

                srcOffset += mipmapSize;
                dstOffset += mipmapSize;
            }
        }

        private static uint GetPadding(uint arraySize, uint offset = 0)
        {
            return 3 - ((arraySize + 3) % 4);
        }

        public Texture2D GetTexture2D()
        {
            var tex = new Texture2D((int)Width, (int)Height, TextureFormat, MipMapCount > 1);
            tex.LoadRawTextureData(RawTextureData);
            return tex;
        }
    }
}
