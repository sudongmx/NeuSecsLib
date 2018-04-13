using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECSDataLib.Core.DataItem
{
    public enum ParseStatus
    {
        PARSE_START,
        PARSE_TOTLE_LEN,
        PARSE_HSMS_HEADER,
        PARSE_HSMS_BODY,
        PARSE_END
    }
    public class HSMSStreamParser
    {
        public byte[] ReceiveBuffer { get; private set; }
        public int WriteStart { get; private set; }
        public int ReadStart { get; private set; }
        public int BufferLength => ReceiveBuffer.Length - WriteStart;
        public ParseStatus Status { get; private set; }
    }
}
