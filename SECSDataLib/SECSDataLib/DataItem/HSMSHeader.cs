using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECSDataLib.Core.DataItem
{
    public class HSMSHeader : IData
    {
        public ushort DeviceId { get; set; }
        public bool Reply { get; set; }
        public byte S { get; set; }
        public byte F { get; set; }
        public HSMSMessageType Type { get; set; }
        public int SystemBytes { get; set; }

        public void WriteSecsByte(List<ArraySegment<byte>> list)
        {
            throw new NotImplementedException();
        }

        public void WriteToSml(StringWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
