
using System;

namespace unitils.crunch
{
    // https://github.com/DaemonEngine/crunch/blob/master/inc/crn_defs.h#L244
    [Flags]
    public enum CrnHeaderFlags
    {
        None = 0x0,
        /// <summary>
        /// If set, the compressed mipmap level data is not located after the file's base data.
        /// It will be separately managed by the user instead.
        /// </summary>
        Segmented = 0x1
    }

    // https://github.com/DaemonEngine/crunch/blob/master/inc/crn_defs.h#L249
    public class CrnHeader
    {
        // https://github.com/DaemonEngine/crunch/blob/master/inc/crn_defs.h#L250
        public const uint SigValue = ('H' << 8) | 'x';

        public ushort Size;

        public ushort CRC16;

        public uint DataSize;

        public ushort DataCRC16;

        public ushort Width;

        public ushort Height;

        public byte Levels;

        public byte Faces;

        public CrnFormat Format;

        public CrnHeaderFlags Flags;

        public uint UserData0;

        public uint UserData1;
    }
}
