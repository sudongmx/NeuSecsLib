using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECSDataLib.Core.DataItem
{
    public abstract class DataNode : IDataNode
    {

        protected ArraySegment<byte> values;
        protected List<IData> children;

        public DataNodeType DataType { get; private set }
        public int Count { get; set; }
        public int Length { get; set; }
        public DataNode(DataNodeType t)
        {
            values = new ArraySegment<byte>();
            children = new List<IData>();
            DataType = t;
            Count = 0;
            Length = 0;
        }

        public List<IData> GetChildren()
        {
            return children;
        }

        public ArraySegment<byte> GetRawValues()
        {
            return values;
        }

        public IData GetSingleChild(int i)
        {
            IData val = null;
            if (i < children.Count)
            {
                val = children.ElementAt<IData>(i);
            }
            return val;
        }

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
