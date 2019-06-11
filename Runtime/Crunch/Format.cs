
using System;

namespace unitils.crunch
{
    public enum CrnFormat
    {
        Invalid = -1,

        DXT1 = 0,

        // CrnFormat.DXT3 is not currently supported when writing to CRN - only DDS.
        DXT3 = 1,

        DXT5 = 2,

        // Various DXT5 derivatives
        DXT5_CCxY = 3,  // Luma-chroma
        DXT5_xGxR = 4,  // Swizzled 2-component
        DXT5_xGBR = 5,  // Swizzled 3-component
        DXT5_AGBR = 6,  // Swizzled 4-component

        // ATI 3DC and X360 DXN
        DXN_XY = 7,
        DXN_YX = 8,

        // DXT5 alpha blocks only
        DXT5A = 9,

        ETC1 = 10,
        ETC2 = 11,
        ETC2A = 12,
        ETC1S = 13,
        ETC2AS = 14,
    }

    public static class CrnFormatExtensions
    {
        private static uint FourCC(char a, char b, char c, char d)
        {
            return (uint) (a | (b << 8) | (c  << 16) | (d << 24));
        }

        public static uint ToFourCC(this CrnFormat format)
        {
            switch (format) {
                case CrnFormat.DXT1:
                return FourCC('D', 'X', 'T', '1');
                case CrnFormat.DXT3:
                return FourCC('D', 'X', 'T', '3');
                case CrnFormat.DXT5:
                return FourCC('D', 'X', 'T', '5');
                case CrnFormat.DXN_XY:
                return FourCC('A', '2', 'X', 'Y');
                case CrnFormat.DXN_YX:
                return FourCC('A', 'T', 'I', '2');
                case CrnFormat.DXT5A:
                return FourCC('A', 'T', 'I', '1');
                case CrnFormat.DXT5_CCxY:
                return FourCC('C', 'C', 'x', 'Y');
                case CrnFormat.DXT5_xGxR:
                return FourCC('x', 'G', 'x', 'R');
                case CrnFormat.DXT5_xGBR:
                return FourCC('x', 'G', 'B', 'R');
                case CrnFormat.DXT5_AGBR:
                return FourCC('A', 'G', 'B', 'R');
                case CrnFormat.ETC1:
                return FourCC('E', 'T', 'C', '1');
                case CrnFormat.ETC2:
                return FourCC('E', 'T', 'C', '2');
                case CrnFormat.ETC2A:
                return FourCC('E', 'T', '2', 'A');
                case CrnFormat.ETC1S:
                return FourCC('E', 'T', '1', 'S');
                case CrnFormat.ETC2AS:
                return FourCC('E', '2', 'A', 'S');
                default:
                break;
            }
            return 0;
        }
    }
}
