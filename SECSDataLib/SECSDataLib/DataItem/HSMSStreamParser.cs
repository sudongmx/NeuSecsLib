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
        PARSED_TOTLE_LEN,
        PARSED_HSMS_HEADER,
        PARSED_ITEM_TYPE,
        PARSED_ITEM_SIZE,
        PARSED_ITEM_VALUE,
        PARSE_END
    }
    public class HSMSStreamParser
    {
        public byte[] ReceiveBuffer { get; private set; }
        public int WriteStart { get; private set; }
        public int ReadStart { get; private set; }
        public int BufferLength => ReceiveBuffer.Length - WriteStart;
        public int RemainLength => WriteStart - ReadStart;
        public ParseStatus Status { get; private set; }

        private HandleReceivedMessage proccessMessage;
        private HSMSMessage message;
        private Stack<DataNode> storeStack;
        private DataNode curData;
        private int decodeCount;
        public HSMSStreamParser(HandleReceivedMessage handle)
        {
            ReceiveBuffer = new byte[8*1024];
            WriteStart = 0;
            ReadStart = 0;
            decodeCount = 0;
            Status = ParseStatus.PARSE_START;
            proccessMessage = handle;
            message = new HSMSMessage();
            storeStack = new Stack<DataNode>();
        }

        public bool ParseReceiveBuffer(int receiveLen)
        {
            WriteStart += receiveLen;
            bool bufferChanged = false;
            while (true)
            {
                int nextLen = 0;
                switch (Status)
                {
                    case ParseStatus.PARSE_START:
                        nextLen = ParseTotalLen();
                        break;
                    case ParseStatus.PARSED_TOTLE_LEN:
                        nextLen = ParseMessageHeader();
                        break;
                    case ParseStatus.PARSED_HSMS_HEADER:
                        nextLen = ParseItemType();
                        break;
                    case ParseStatus.PARSED_ITEM_TYPE:
                        nextLen = ParseItemSize();
                        break;
                    case ParseStatus.PARSED_ITEM_SIZE:
                        nextLen = ParseItemValue();
                        break;
                    case ParseStatus.PARSED_ITEM_VALUE:
                        if (storeStack.Count == 0)
                        {
                            Status = ParseStatus.PARSE_END;
                        }
                        break;
                    default:
                        break;
                }
                bufferChanged = ChangeBufferIndex(nextLen);
                if ((message.Length - 10) >= decodeCount)
                {
                    Status = ParseStatus.PARSE_END;
                }
                if (Status == ParseStatus.PARSE_END || nextLen > 0)
                {
                    break;
                }
            }
            if (Status == ParseStatus.PARSE_END)
            {
                if (proccessMessage != null)
                {
                    message.DataRoot = curData;
                    proccessMessage(message);
                }
                Reset();
            }
            return bufferChanged;
        }
        public void Reset()
        {
            Status = ParseStatus.PARSE_START;
            WriteStart = 0;
            ReadStart = 0;
            storeStack.Clear();
            message = null;
            message = new HSMSMessage();
            decodeCount = 0;
        }

        private int ParseTotalLen()
        {
            int retVal = 4;
            if (NeedWaitNextReceive(retVal) == 0)
            {
                message.Length = BitConverter.ToInt32(ReceiveBuffer, ReadStart);
                ReadStart += retVal;
                retVal = 0;
                Status = ParseStatus.PARSED_TOTLE_LEN;
            }
            return retVal;
        }
        private int ParseMessageHeader()
        {
            int retVal = 10;
            if (NeedWaitNextReceive(retVal) == 0)
            {
                message.Header.DeviceId = BitConverter.ToUInt16(ReceiveBuffer, ReadStart);
                message.Header.Reply = (ReceiveBuffer[ReadStart + 2] & 0x80) != 0;
                message.Header.S = (byte)(ReceiveBuffer[ReadStart + 2] & 0x7F);
                message.Header.F = ReceiveBuffer[ReadStart + 3];
                message.Header.SystemBytes = BitConverter.ToInt32(ReceiveBuffer, (ReadStart + 6));
                ReadStart += retVal;
                decodeCount += retVal;
                retVal = 0;
                Status = ParseStatus.PARSED_HSMS_HEADER;
                if (message.Length == 10)
                {
                    Status = ParseStatus.PARSE_END;
                }
            }
            return retVal;
        }
        private int ParseItemType()
        {
            int retVal = 1;
            if (NeedWaitNextReceive(retVal) == 0)
            {
                DataNodeType type = (DataNodeType)(ReceiveBuffer[ReadStart] >> 3);
                curData = DataNodeUtils.CreateDataNode(type);
                curData.Count = (ReceiveBuffer[ReadStart] & 0x03);
                ReadStart += retVal;
                decodeCount += retVal;
                retVal = 0;
                Status = ParseStatus.PARSED_ITEM_TYPE;
            }
            return retVal;
        }
        private int ParseItemSize()
        {
            int retVal = curData.Count;
            if (NeedWaitNextReceive(retVal) == 0)
            {
                for (int i = 1; i <= retVal; i++)
                {
                    curData.Length += ((int)ReceiveBuffer[ReadStart + retVal - i]) << ((i-1) * 8);
                }
                if (curData.DataType == DataNodeType.LIST)
                {
                    if (curData.Length > 0)
                    {
                        storeStack.Push(curData);
                    }
                    else
                    {
                        AddNodeToList();
                    }
                    Status = ParseStatus.PARSED_HSMS_HEADER;
                }
                else
                {
                    Status = ParseStatus.PARSED_ITEM_SIZE;
                }
                ReadStart += retVal;
                decodeCount += retVal;
                retVal = 0;
            }
            return retVal;
        }
        private int ParseItemValue()
        {
            int retVal = curData.Length;
            if (NeedWaitNextReceive(retVal) == 0)
            {
                byte[] value = new byte[retVal];
                Array.Copy(ReceiveBuffer, value, retVal);
                curData.SetRawValues(new ArraySegment<byte>(value));

                AddNodeToList();
                ReadStart += retVal;
                decodeCount += retVal;
                retVal = 0;
            }
            return retVal;
        }
        private int NeedWaitNextReceive(int parseNeed)
        {
            return RemainLength >= parseNeed ? 0 : parseNeed;
        }
        private bool ChangeBufferIndex(int nextLen)
        {
            if (ReadStart > 0)
            {
                Array.Copy(ReceiveBuffer, ReadStart, ReceiveBuffer, 0, RemainLength);
                WriteStart -= ReadStart;
                ReadStart -= ReadStart;
            }
            if (nextLen > BufferLength)
            {
                byte[] newBuffer = new byte[nextLen * 2];
                Array.Copy(ReceiveBuffer, 0, newBuffer, 0, RemainLength);
                ReceiveBuffer = newBuffer;
                return true;
            }
            return false;
        }
        private void AddNodeToList()
        {
            while (storeStack.Count > 0)
            {
                DataNode list = storeStack.Peek();
                list.GetChildren().Add(curData);
                if (list.Length == list.GetChildren().Count)
                {
                    curData = storeStack.Pop();
                }
                else
                {
                    break;
                }
            }
        }
    }
}
