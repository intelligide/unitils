using System;
using UnityEngine;

namespace unitils.crunch
{
    public class CrnTexture
    {
        public CrnHeader Header { get; private set; } = null;

        /// <summary>
        /// Parse CRN image byte array.
        /// </summary>
        /// <param name="tex">The texture to load the image into.</param>
        /// <param name="data">The byte array containing the image data to load.</param>
        /// <returns>Returns true if the data can be loaded, false otherwise.</returns>
        public bool Parse(byte[] data)
        {
            if(((data[0] << 8) | data[1]) != CrnHeader.SigValue)
            {
                Debug.LogError("Invalid CRN Texture.");
                return false;
            }

            CrnHeader head = new CrnHeader();

            head.Size = BitConverter.ToUInt16(data, 2);
            if(data.Length < head.Size - 2)
            {
                return false;
            }


            head.CRC16 = BitConverter.ToUInt16(data, 4);
            head.DataSize = BitConverter.ToUInt32(data, 6);
            head.DataCRC16 = BitConverter.ToUInt16(data, 10);

            head.Width = BitConverter.ToUInt16(data, 12);
            head.Height = BitConverter.ToUInt16(data, 14);
            head.Levels = data[16];
            head.Faces = data[17];
            head.Format = (CrnFormat) data[18];
            head.Flags = (CrnHeaderFlags) BitConverter.ToUInt16(data, 19);

            head.UserData0 = BitConverter.ToUInt32(data, 25);
            head.UserData1 = BitConverter.ToUInt32(data, 29);

            Header = head;

            return true;
        }
    }
}
