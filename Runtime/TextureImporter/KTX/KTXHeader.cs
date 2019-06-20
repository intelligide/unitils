
using System;
using System.Collections.Generic;

namespace unitils.ktx
{
    public class KTXHeader
    {
        public byte[] Identifier;

        public UInt32 Endianess;


        public UInt32 GlType;

        public UInt32 GlTypeSize;

        public KTXPixelFormat GlFormat;

        public KTXPixelFormat GlInternalFormat;

        public KTXPixelFormat GlBaseInternalFormat;

        public UInt32 Width;

        public UInt32 Height;

        public UInt32 Depth;

        public UInt32 NumArrayElements;

        public UInt32 NumFaces;

        public UInt32 MipMapCount;

        public Dictionary<string, byte[]> KeyValuePairs;

        public UInt32 DataOffset;
    }
}
