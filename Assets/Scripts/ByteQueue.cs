using System;
using System.Runtime.InteropServices;
using System.Text;

public class ByteQueue
{
    private const int DATA_BUFFER = 10240;

    public byte[] data { get; private set; }

    public int queueCapacity { get; private set; }

    public int queueLength { get; private set; }

    public bool locked = false;

    public ByteQueue(int BufferSize = DATA_BUFFER)
    {
        data = new byte[BufferSize];

        queueCapacity = BufferSize;
    }

    public void CopyBuffer(ByteQueue source)
    {
        if (source.queueLength == 0)
        {
            RemoveData(queueLength);
            return;
        }

        queueCapacity = source.queueCapacity;

        data = new byte[queueCapacity];

        byte[] buf = new byte[source.queueLength];

        source.PeekBlock(ref buf, source.queueLength);

        queueLength = 0;

        WriteBlock(buf, source.queueLength);

    }

    public int RemoveData(int length)
    {
        int ret = System.Math.Min(length, queueLength);

        if (ret != queueCapacity)
        {
            Marshal.Copy(data, ret, Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), queueLength - ret);
        }

        queueLength = queueLength - ret;

        return ret;
    }

    //Reads a byte array from the beginning of the queue but DOES NOT remove it.
    public int PeekBlock(ref byte[] block, int dataLength)
    {
        if (dataLength <= 0)
        {
            throw new System.ArgumentException("ByteQueue PeekBlock(): tried to peek 0 data length.");
        }

        return ReadData(ref block, dataLength);
    }

    public int ReadBlock(ref byte[] block, int dataLength)
    {
        if (dataLength <= 0)
        {
            throw new System.ArgumentException("ByteQueue PeekBlock(): tried to read 0 data length.");
        }

        return RemoveData(ReadData(ref block, dataLength));
    }

    public int WriteBlock(byte[] block, int dataLength)
    {
        //Prevent from copying memory outside the array
        if (dataLength > block.Length || dataLength < 0)
        {
           dataLength = block.Length;
        }

        return WriteData(block, dataLength);
    }

    private int ReadData(ref byte[] buf, int dataLength)
    {
        if (dataLength > queueLength)
        {
            throw new System.ArgumentException("ByteQueue ReadData(): tried to read more data than this obect has.");
        }

        Marshal.Copy(data, 0, Marshal.UnsafeAddrOfPinnedArrayElement(buf, 0), dataLength);

        return dataLength;
    }

    private int WriteData(byte[] buf, int dataLength)
    {
        if (queueCapacity - queueLength - dataLength < 0)
        {
            throw new System.ArgumentException("ByteQueue WriteData(): There is no space to write data on buffer.");
        }

        Marshal.Copy(buf, 0, Marshal.UnsafeAddrOfPinnedArrayElement(data, queueLength), dataLength);

        queueLength += dataLength;

        return dataLength;
    }

    public int WriteByte(byte value)
    {
        byte[] buf = { value };

        return WriteData(buf, 1);
    }

    public int WriteInt16(short value)
    {
        byte[] buf = new byte[2];

        GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
        
        Marshal.Copy(handle.AddrOfPinnedObject(), buf, 0, 2);

        handle.Free();

        return WriteData(buf, 2);
    }

    public int WriteInteger(int value)
    {
        byte[] buf = new byte[4];

        GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);

        Marshal.Copy(handle.AddrOfPinnedObject(), buf, 0, 4);

        handle.Free();

        return WriteData(buf, 4);
    }

    public int WriteSingle(float value)
    {
        byte[] buf = new byte[4];

        GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);

        Marshal.Copy(handle.AddrOfPinnedObject(), buf, 0, 4);

        handle.Free();

        return WriteData(buf, 4);
    }

    public int WriteDouble(double value)
    {
        byte[] buf = new byte[8];

        GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);

        Marshal.Copy(handle.AddrOfPinnedObject(), buf, 0, 8);

        handle.Free();

        return WriteData(buf, 4);
    }

    public int WriteBoolean(bool value)
    {
        byte[] buf = new byte[1];

        buf[0] = (value ? (byte)1 : (byte)0);

        return WriteData(buf, 1);
    }

    public int WriteASCIIStringFixed(string Value)
    {
        byte[] buf = Encoding.ASCII.GetBytes(Value);

        return WriteData(buf, Value.Length);
    }

    public int WriteUnicodeStringFixed(string Value)
    {
        byte[] buf = Encoding.Unicode.GetBytes(Value);

        return WriteData(buf, Value.Length);
    }

    public int WriteASCIIString(string Value)
    {
        byte[] buf = new byte[Value.Length + 2];

        byte[] strData = Encoding.ASCII.GetBytes(Value);

        int strLength = Value.Length;

        GCHandle handle = GCHandle.Alloc(strLength, GCHandleType.Pinned);

        Marshal.Copy(handle.AddrOfPinnedObject(), buf, 0, 2);

        handle.Free();

        Marshal.Copy(strData, 0, Marshal.UnsafeAddrOfPinnedArrayElement(buf, 2), Value.Length);

        return WriteData(buf, Value.Length + 2);
    }

    public int WriteUnicodeString(string Value)
    {
        byte[] buf = new byte[Value.Length + 2];

        byte[] strData = Encoding.Unicode.GetBytes(Value);

        int strLength = Value.Length;

        GCHandle handle = GCHandle.Alloc(strLength, GCHandleType.Pinned);

        Marshal.Copy(handle.AddrOfPinnedObject(), buf, 0, 2);

        handle.Free();

        Marshal.Copy(strData, 0, Marshal.UnsafeAddrOfPinnedArrayElement(buf, 2), Value.Length);

        return WriteData(buf, Value.Length + 2);
    }

    public byte ReadByte()
    {
        byte[] buf = new byte[1];

        RemoveData(ReadData(ref buf, 1));

        return buf[0];
    }

    public short ReadInt16()
    {
        byte[] buf = new byte[2];

        RemoveData(ReadData(ref buf, 2));

        short ret = 0;

        unsafe
        {
            short* pRet = &ret;
            IntPtr retPtr = new IntPtr((void*)pRet);
            Marshal.Copy(buf, 0, retPtr, 2);
        }

        return ret;
    }

    public int ReadInteger()
    {
        byte[] buf = new byte[4];

        RemoveData(ReadData(ref buf, 4));

        int ret = 0;

        unsafe
        {
            int* pRet = &ret;
            IntPtr retPtr = new IntPtr((void*)pRet);
            Marshal.Copy(buf, 0, retPtr, 4);
        }

        return ret;
    }

    public float ReadSingle()
    {
        byte[] buf = new byte[4];

        RemoveData(ReadData(ref buf, 4));

        float ret = 0;

        unsafe
        {
            float* pRet = &ret;
            IntPtr retPtr = new IntPtr((void*)pRet);
            Marshal.Copy(buf, 0, retPtr, 4);
        }

        return ret;
    }

    public double ReadDouble()
    {
        byte[] buf = new byte[8];

        RemoveData(ReadData(ref buf, 8));

        double ret = 0;

        unsafe
        {
            double* pRet = &ret;
            IntPtr retPtr = new IntPtr((void*)pRet);
            Marshal.Copy(buf, 0, retPtr, 8);
        }

        return ret;
    }

    public bool ReadBoolean()
    {
        byte[] buf = new byte[1];

        RemoveData(ReadData(ref buf, 1));

        return buf[0] == 1;
    }

    public string ReadASCIIStringFixed(int length)
    {
        if (length <= 0)
        {
            return "";
        }

        if (queueLength >= length)
        {
            byte[] buf = new byte[length];

            RemoveData(ReadData(ref buf, length));

            return Encoding.ASCII.GetString(buf);
        }
        else
        {
            throw new System.ArgumentException("ByteQueue ReadASCIIStringFixed():there is not enough data to read.");
        }
    }

    //TODO all unicode functions might not be working
    public string ReadUnicodeStringFixed(int length)
    {
        if (length <= 0)
        {
            return "";
        }

        if (queueLength >= length * 2)
        {
            byte[] buf = new byte[length * 2];

            RemoveData(ReadData(ref buf, length * 2));

            return Encoding.Unicode.GetString(buf);
        }
        else
        {
            throw new System.ArgumentException("ByteQueue ReadASCIIStringFixed():there is not enough data to read.");
        }
    }

    public string ReadASCIIString()
    {
        byte[] buf = new byte[2];
        short length = 0;
       

        if (queueLength > 1)
        {
            ReadData(ref buf, 2);

            unsafe
            {
                short* pLength = &length;
                IntPtr lengthPtr = new IntPtr((void*) pLength);
                Marshal.Copy(buf, 0, lengthPtr, 2);
            }

            if (queueLength >= length + 2)
            {
                RemoveData(2);

                if(length > 0)
                {
                    byte[] strBuf = new byte[length];

                    RemoveData(ReadData(ref strBuf, length));

                    return Encoding.ASCII.GetString(strBuf);
                }
                else
                {
                    return "";
                }
                
            }
            else
            {
                throw new System.ArgumentException("ByteQueue ReadASCIIStringFixed():there is not enough data to read.");
            }            
        }
        else
        {
            throw new System.ArgumentException("ByteQueue ReadASCIIStringFixed():there is not enough data to read.");
        }
    }

    public string ReadUnicodeString()
    {
        byte[] buf = new byte[2];
        short length = 0;


        if (queueLength > 1)
        {
            ReadData(ref buf, 2);

            unsafe
            {
                short* pLength = &length;
                IntPtr lengthPtr = new IntPtr((void*)pLength);
                Marshal.Copy(buf, 0, lengthPtr, 2);
            }

            if (queueLength >= (length * 2) + 2)
            {
                RemoveData(2);

                if (length > 0)
                {
                    byte[] strBuf = new byte[length * 2];

                    RemoveData(ReadData(ref strBuf, length * 2));

                    return Encoding.Unicode.GetString(strBuf);
                }
                else
                {
                    return "";
                }

            }
            else
            {
                throw new System.ArgumentException("ByteQueue ReadASCIIStringFixed():there is not enough data to read.");
            }
        }
        else
        {
            throw new System.ArgumentException("ByteQueue ReadASCIIStringFixed():there is not enough data to read.");
        }
    }

    public byte PeekByte()
    {
        byte[] buf = new byte[1];

        ReadData(ref buf, 1);

        return buf[0];
    }

    public short PeekInt16()
    {
        byte[] buf = new byte[2];

        ReadData(ref buf, 2);

        short ret = 0;

        unsafe
        {
            short* pRet = &ret;
            IntPtr retPtr = new IntPtr((void*)pRet);
            Marshal.Copy(buf, 0, retPtr, 2);
        }

        return ret;
    }

    public int PeekInteger()
    {
        byte[] buf = new byte[4];

        ReadData(ref buf, 4);

        int ret = 0;

        unsafe
        {
            int* pRet = &ret;
            IntPtr retPtr = new IntPtr((void*)pRet);
            Marshal.Copy(buf, 0, retPtr, 4);
        }

        return ret;
    }

    public float PeekSingle()
    {
        byte[] buf = new byte[4];

        ReadData(ref buf, 4);

        float ret = 0;

        unsafe
        {
            float* pRet = &ret;
            IntPtr retPtr = new IntPtr((void*)pRet);
            Marshal.Copy(buf, 0, retPtr, 4);
        }

        return ret;
    }

    public double PeekDouble()
    {
        byte[] buf = new byte[8];

       ReadData(ref buf, 8);

        double ret = 0;

        unsafe
        {
            double* pRet = &ret;
            IntPtr retPtr = new IntPtr((void*)pRet);
            Marshal.Copy(buf, 0, retPtr, 8);
        };

        return ret;
    }

    public bool PeekBoolean()
    {
        byte[] buf = new byte[1];

        ReadData(ref buf, 1);

        return buf[0] == 1;
    }

    public string PeekASCIIStringFixed(int length)
    {
        if (length <= 0)
        {
            return "";
        }

        if (queueLength >= length)
        {
            byte[] buf = new byte[length];

            ReadData(ref buf, length);

            return Encoding.ASCII.GetString(buf);
        }
        else
        {
            throw new System.ArgumentException("ByteQueue PeekASCIIStringFixed():there is not enough data to read.");
        }
    }

    //TODO all unicode functions might not be working
    public string PeekUnicodeStringFixed(int length)
    {
        if (length <= 0)
        {
            return "";
        }

        if (queueLength >= length * 2)
        {
            byte[] buf = new byte[length * 2];

            ReadData(ref buf, length * 2);

            return Encoding.Unicode.GetString(buf);
        }
        else
        {
            throw new System.ArgumentException("ByteQueue PeekUnicodeStringFixed():there is not enough data to read.");
        }
    }

    public string PeekASCIIString()
    {
        byte[] buf = new byte[1];
        short length = 0;


        if (queueLength > 1)
        {
            ReadData(ref buf, 2);

            unsafe
            {
                short* pLength = &length;
                IntPtr lengthPtr = new IntPtr((void*)pLength);
                Marshal.Copy(buf, 0, lengthPtr, 2);
            }


            if (queueLength >= length + 2)
            {
                byte[] buf2 = new byte[length + 2];

                ReadData(ref buf2, length + 2);

                if (length > 0)
                {
                    byte[] strBuf = new byte[length];
   
                    Marshal.Copy(buf2, 2, Marshal.UnsafeAddrOfPinnedArrayElement(strBuf,0), length);

                    return Encoding.ASCII.GetString(strBuf);
                }
                else
                {
                    return "";
                }

            }
            else
            {
                throw new System.ArgumentException("ByteQueue PeekASCIIString():there is not enough data to read.");
            }
        }
        else
        {
            throw new System.ArgumentException("ByteQueue PeekASCIIString():there is not enough data to read.");
        }
    }

    public string PeekUnicodeString()
    {
        byte[] buf = new byte[1];
        short length = 0;


        if (queueLength > 1)
        {
            ReadData(ref buf, 2);

            unsafe
            {
                short* pLength = &length;
                IntPtr lengthPtr = new IntPtr((void*)pLength);
                Marshal.Copy(buf, 0, lengthPtr, 2);
            }


            if (queueLength >= (length * 2) + 2)
            {
                byte[] buf2 = new byte[(length * 2) + 2];

                ReadData(ref buf2, (length * 2) + 2);

                if (length > 0)
                {
                    byte[] strBuf = new byte[length * 2];

                    Marshal.Copy(buf2, 2, Marshal.UnsafeAddrOfPinnedArrayElement(strBuf, 0), length);

                    return Encoding.Unicode.GetString(strBuf);
                }
                else
                {
                    return "";
                }

            }
            else
            {
                throw new System.ArgumentException("ByteQueue PeekUnicodeString():there is not enough data to read.");
            }
        }
        else
        {
            throw new System.ArgumentException("ByteQueue PeekUnicodeString():there is not enough data to read.");
        }
    }
}
