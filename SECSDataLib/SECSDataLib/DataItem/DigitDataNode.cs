using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECSDataLib.Core.DataItem
{
    public class DigitDataNode : DataNode
    {
        public DigitDataNode() : base()
        {
            values = new List<ArraySegment<byte>>();
        }
    }
}
