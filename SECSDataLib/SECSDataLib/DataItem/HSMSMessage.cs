using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECSDataLib.Core.DataItem
{
    public class HSMSMessage : IData
    {
        public int Length { get; set; }
        public HSMSHeader Header { get; set; }
        public DataNode DataRoot { get; set; }

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
