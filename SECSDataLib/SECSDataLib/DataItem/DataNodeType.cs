using System.Collections.Generic;

namespace SECSDataLib.Core.DataItem
{
    public enum DataNodeType : byte
    {
        LIST   = 0x00,
        BINARY = 0x08,
        BOOL   = 0x09,
        ASCII  = 0x10,
        JIS8   = 0x11,
        I8     = 0x18,
        I1     = 0x19,
        I2     = 0x1A,
        I4     = 0x1C,
        F8     = 0x20,
        F4     = 0x24,
        U8     = 0x28,
        U1     = 0x29,
        U2     = 0x2A,
        U4     = 0x2C
    }
}
