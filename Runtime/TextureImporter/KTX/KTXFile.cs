using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace unitils.ktx
{
    public class KTXFile
    {
        public KTXHeader Header { get; private set; } = null;

        public byte[] UnityTextureData { get; private set; } = null;

        public KTXFile(byte[] data, bool computeUnityTextureData = false)
        {
            Header = new KTXHeader();

            Header.Identifier = new byte[12];
            Buffer.BlockCopy(data, 0, Header.Identifier, 0, 12);

            Header.Endianess = BitConverter.ToUInt32(data, 12);
            Header.GlType = BitConverter.ToUInt32(data, 16);
            Header.GlTypeSize = BitConverter.ToUInt32(data, 20);
            Header.GlFormat = (KTXPixelFormat) BitConverter.ToUInt32(data, 24);
            Header.GlInternalFormat = (KTXPixelFormat) BitConverter.ToUInt32(data, 28);
            Header.GlBaseInternalFormat = (KTXPixelFormat) BitConverter.ToUInt32(data, 32);
            Header.Width = BitConverter.ToUInt32(data, 36);
            Header.Height = BitConverter.ToUInt32(data, 40);
            Header.Depth = BitConverter.ToUInt32(data, 44);
            Header.NumArrayElements = BitConverter.ToUInt32(data, 48);
            Header.NumFaces = BitConverter.ToUInt32(data, 52);
            Header.MipMapCount = BitConverter.ToUInt32(data, 56);

            var bytesOfKeyValueData = BitConverter.ToUInt32(data, 60);
            if(bytesOfKeyValueData > 0)
            {
                Header.KeyValuePairs = new Dictionary<string, byte[]>();

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

                    Header.KeyValuePairs.Add(key, value);
                }
            }

            Header.DataOffset = 64 + bytesOfKeyValueData;

            if(computeUnityTextureData)
            {
                UnityTextureData = new byte[data.Length - Header.MipMapCount * 4]; // TODO: Remove all paddings

                uint dstOffset = 0;
                uint srcOffset = Header.DataOffset;
                for(int mipmapIdx = 0; mipmapIdx < Header.MipMapCount; mipmapIdx++)
                {
                    var mipmapSize = BitConverter.ToUInt32(data, (int) srcOffset);

                    srcOffset += 4 + GetPadding(mipmapSize);

                    Buffer.BlockCopy(data, (int)srcOffset, UnityTextureData, (int)dstOffset, (int)mipmapSize);

                    // TODO: remove cube padding

                    srcOffset += mipmapSize;
                    dstOffset += mipmapSize;
                }
            }

        }

        private static uint GetPadding(uint arraySize, uint offset = 0)
        {
            return 3 - ((arraySize + 3) % 4);
        }
    }
}
