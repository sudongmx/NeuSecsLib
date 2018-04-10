using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SECSDataLib
{
    public interface IData
    {
        void WriteSecsByte(ref byte[] outArray, int arrayLen);
        void WriteToSml(StringWriter writer);
    }
}
