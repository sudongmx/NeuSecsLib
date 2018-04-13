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
        protected int len;
        protected DataNodeType type;
        protected List<ArraySegment<byte>> values;
        protected List<IData> children;

        public DataNode()
        {
            values = new List<ArraySegment<byte>>();
            children = new List<IData>();
        }

        public List<IData> GetChildren()
        {
            return children;
        }

        public List<ArraySegment<byte>> GetRawValues()
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
