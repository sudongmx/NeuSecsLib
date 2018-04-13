using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECSDataLib.Core.DataItem
{
    public interface IDataNode : IData
    {
        List<ArraySegment<byte>> GetRawValues();
        List<IData> GetChildren();
        IData GetSingleChild(int i);
    }
}
