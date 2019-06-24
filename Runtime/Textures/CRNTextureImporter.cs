using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace unitils.textures
{
    [Flags]
    public enum CRNHeaderFlags
    {
        None = 0x0,
        /// <summary>
        /// If set, the compressed mipmap level data is not located after the file's base data.
        /// It will be separately managed by the user instead.
        /// </summary>
        Segmented = 0x1
    }

    public enum CRNFormat
    {
        Invalid = -1,
        DXT1,
        DXT3,
        DXT5,

        // Various DXT5 derivatives
        DXT5_CCxY,  // Luma-chroma
        DXT5_xGxR,  // Swizzled 2-component
        DXT5_xGBR,  // Swizzled 3-component
        DXT5_AGBR,  // Swizzled 4-component

        // ATI 3DC and X360 DXN
        DXN_XY,
        DXN_YX,

        // DXT5 alpha blocks only
        DXT5A,

        ETC1,
        ETC2,
        ETC2A,
        ETC1S,
        ETC2AS,
    };

    public static class CRNFormatExtensions
    {
        private static uint FourCC(char a, char b, char c, char d)
        {
            return (uint) (a | (b << 8) | (c  << 16) | (d << 24));
        }

        public static uint ToFourCC(this CRNFormat format)
        {
            switch (format) {
                case CRNFormat.DXT1:
                return FourCC('D', 'X', 'T', '1');
                case CRNFormat.DXT3:
                return FourCC('D', 'X', 'T', '3');
                case CRNFormat.DXT5:
                return FourCC('D', 'X', 'T', '5');
                case CRNFormat.DXN_XY:
                return FourCC('A', '2', 'X', 'Y');
                case CRNFormat.DXN_YX:
                return FourCC('A', 'T', 'I', '2');
                case CRNFormat.DXT5A:
                return FourCC('A', 'T', 'I', '1');
                case CRNFormat.DXT5_CCxY:
                return FourCC('C', 'C', 'x', 'Y');
                case CRNFormat.DXT5_xGxR:
                return FourCC('x', 'G', 'x', 'R');
                case CRNFormat.DXT5_xGBR:
                return FourCC('x', 'G', 'B', 'R');
                case CRNFormat.DXT5_AGBR:
                return FourCC('A', 'G', 'B', 'R');
                case CRNFormat.ETC1:
                return FourCC('E', 'T', 'C', '1');
                case CRNFormat.ETC2:
                return FourCC('E', 'T', 'C', '2');
                case CRNFormat.ETC2A:
                return FourCC('E', 'T', '2', 'A');
                case CRNFormat.ETC1S:
                return FourCC('E', 'T', '1', 'S');
                case CRNFormat.ETC2AS:
                return FourCC('E', '2', 'A', 'S');
                default:
                break;
            }
            return 0;
        }
    }

    public class CRNTextureImporter : ITextureImporter
    {
        public const uint SigValue = ('H' << 8) | 'x';

        public ushort HeaderSize;

        public ushort CRC16;

        /// <note>
        /// With header size
        /// </note>
        public uint DataSize;

        public ushort DataCRC16;

        public ushort Width;

        public ushort Height;

        public byte Levels;

        public byte Faces;

        public CRNFormat Format;

        public CRNHeaderFlags Flags;

        public uint UserData0;

        public uint UserData1;

        public byte[] RawTextureData { get; private set; }

        public TextureFormat TextureFormat
        {
            get {
                switch(Format)
                {
                    case CRNFormat.DXT1:
                        return TextureFormat.DXT1;
                    case CRNFormat.DXT5:
                        return TextureFormat.DXT5;
                    default:
                        throw new Exception("Invalid Texture Format. The texture format is not supported by Unity.");
                }
            }
        }

        public CRNTextureImporter(byte[] data)
        {
            var converter = new EndianBitConverter();
            uint sig = converter.ToUInt16(data, 0);

            if(sig != SigValue)
            {
                converter.InvertEndian = true;
                sig = converter.ToUInt16(data, 0);
                if(sig != SigValue)
                {
                    throw new Exception("Invalid CRN Texture.");

                }
            }

            HeaderSize = converter.ToUInt16(data, 2);
            CRC16 = converter.ToUInt16(data, 4);
            DataSize = converter.ToUInt32(data, 6);
            DataCRC16 = converter.ToUInt16(data, 10);

            Width = converter.ToUInt16(data, 12);
            Height = converter.ToUInt16(data, 14);
            Levels = data[16];
            Faces = data[17];
            Format = (CRNFormat) data[18];
            Flags = (CRNHeaderFlags) converter.ToUInt16(data, 19);

            UserData0 = converter.ToUInt32(data, 25);
            UserData1 = converter.ToUInt32(data, 29);

            byte[] dxtData = unitils.UnityCRN.Transcode(data);
            Texture2D tex; // Bad, prefer direct conversion instead of using Texture2D
            if(ImageConversion.LoadDDSImage(dxtData, out tex))
            {
                RawTextureData = tex.GetRawTextureData();
            }

        }

        public Texture2D GetTexture2D()
        {
            var tex = new Texture2D((int)Width, (int)Height, TextureFormat, Levels > 1);
            tex.LoadRawTextureData(RawTextureData);
            return tex;
        }
    }
}
