

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace unitils.textures
{
    internal class EndianBitConverter
    {
        public bool InvertEndian = false;

        private byte[] Subset(byte[] src, int startIndex, int byteCount)
        {
            byte[] res = new byte[byteCount];
            Buffer.BlockCopy(src, startIndex, res, 0, byteCount);
            return res;
        }

        public ushort ToUInt16(byte[] value, int startIndex)
        {
            if(InvertEndian)
            {
                var ext = Subset(value, startIndex, sizeof(ushort));
                Array.Reverse(ext);
                return BitConverter.ToUInt16(ext, 0);
            }
            else
            {
                return BitConverter.ToUInt16(value, startIndex);
            }
        }

        public uint ToUInt32(byte[] value, int startIndex)
        {
            if(InvertEndian)
            {
                var ext = Subset(value, startIndex, sizeof(uint));
                Array.Reverse(ext);
                return BitConverter.ToUInt32(ext, 0);
            }
            else
            {
                return BitConverter.ToUInt32(value, startIndex);
            }
        }
    }
}
