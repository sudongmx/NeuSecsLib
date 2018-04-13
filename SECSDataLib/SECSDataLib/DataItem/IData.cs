﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SECSDataLib.Core.DataItem
{
    public interface IData
    {
        void WriteSecsByte(List<ArraySegment<byte>> list);
        void WriteToSml(StringWriter writer);

    }
}
