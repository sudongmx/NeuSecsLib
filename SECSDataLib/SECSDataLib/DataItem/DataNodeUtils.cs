using System;
using System.Collections.Generic;
using System.Text;

namespace SECSDataLib.Core.DataItem
{
    public class DataNodeUtils
    {
        public static Dictionary<DataNodeType, int> unitLenDic = new Dictionary<DataNodeType, int>
        {
            { DataNodeType.LIST, 0},
            { DataNodeType.BINARY, sizeof(byte) },
            { DataNodeType.BOOL, sizeof(bool) },
            { DataNodeType.F4, sizeof(float) },
            { DataNodeType.F8, sizeof(double) },
            { DataNodeType.I1, sizeof(sbyte) },
            { DataNodeType.I2, sizeof(short) },
            { DataNodeType.I4, sizeof(int) },
            { DataNodeType.I8, sizeof(long) },
            { DataNodeType.ASCII, sizeof(byte) },
            { DataNodeType.JIS8, sizeof(byte) },
            { DataNodeType.U1, sizeof(byte) },
            { DataNodeType.U2, sizeof(ushort) },
            { DataNodeType.U4, sizeof(uint) },
            { DataNodeType.U8, sizeof(ulong) },
        };
        public static string GetStringValue(ArraySegment<byte> valueArr, DataNodeType type)
        {
            if (type == DataNodeType.ASCII)
            {
                return Encoding.ASCII.GetString(valueArr.Array);
            }
            else if (type == DataNodeType.JIS8)
            {
                return Encoding.GetEncoding(50222).GetString(valueArr.Array);
            }
            return string.Empty;
        }
        public static int GetI4Value(ArraySegment<byte> valueArr)
        {
            int val = 0;
            if (valueArr.Count < sizeof(int))
            {
                throw new FormatException();
            }
            val = BitConverter.ToInt32(valueArr.Array, 0);
            return val;
        }
        public static List<int> GetI4Values(ArraySegment<byte> valueArr)
        {
            List<int> vals = new List<int>();
            if (valueArr.Count < sizeof(int))
            {
                throw new FormatException();
            }
            for(int i = 0; i < valueArr.Count; )
            {
                vals.Add(BitConverter.ToInt32(valueArr.Array, i));
                i += sizeof(int);
            }

            return vals;
        }
        public static DataNode CreateDataNode(DataNodeType type)
        {
            DataNode node = null;
            switch (type)
            {
                case DataNodeType.ASCII:
                case DataNodeType.JIS8:
                    node = new StringDataNode(type);
                    break;
                case DataNodeType.LIST:
                    node = new ListDataNode();
                    break;
                default:
                    node = new DigitDataNode(type);
                    break;
            }
            return node;
        }
    }
}
