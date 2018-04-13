using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECSDataLib.Core.DataItem
{
    public enum HSMSMessageType : byte
    {
        DATA              = 0x00,
        SELECT_REQUEST    = 0x01,
        SELECT_RESPONSE   = 0x02,
        DESELECT_REQUEST  = 0x03,
        DESELECT_RESPONSE = 0x04,
        LINKTEST_REQUEST  = 0x05,
        LINKTEST_RESPONSE = 0x06,
        SEPERATE_REQUEST  = 0x09
    }
}
