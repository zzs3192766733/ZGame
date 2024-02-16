//========================================================
// 描述:游戏内存读写类
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/22 19:43:20
//========================================================

using System;
using System.IO;
using System.Text;
using UnityEngine;
namespace GameFramework.Common
{
	
	public class GameMemoryStream : MemoryStream
	{
		public GameMemoryStream()
    {

    }
    public GameMemoryStream(byte[] buffer) :base(buffer)
    {

    }
    #region Short
    public short ReadShort()
    {
        byte[] arr = new byte[2];
        base.Read(arr, 0, arr.Length);
        return BitConverter.ToInt16(arr, 0);
    }
    public void WriteShort(short value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr,0,arr.Length);
    }
    #endregion
    #region UShort
    public ushort ReadUShort()
    {
        byte[] arr = new byte[2];
        base.Read(arr, 0, arr.Length);
        return BitConverter.ToUInt16(arr, 0);
    }
    public void WriteUShort(ushort value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion
    #region Int
    public int ReadInt()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, arr.Length);
        return BitConverter.ToInt32(arr, 0);
    }
    public void WriteInt(int value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion
    #region UInt
    public uint ReadUInt()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, arr.Length);
        return BitConverter.ToUInt32(arr, 0);
    }
    public void WriteUInt(uint value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion
    #region Float
    public float ReadFloat()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, arr.Length);
        return BitConverter.ToSingle(arr, 0);
    }
    public void WriteFloat(float value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion
    #region Double
    public double ReadDouble()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, arr.Length);
        return BitConverter.ToDouble(arr, 0);
    }
    public void WriteDouble(double value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion
    #region Bool
    public bool ReadBool()
    {
        return base.ReadByte() == 1;
    }
    public void WriteBool(bool value)
    {
        base.WriteByte((byte)(value == true ? 1 : 0));
    }
    #endregion
    #region String
    public string ReadString()
    {
        ushort len = this.ReadUShort();
        byte[] arr = new byte[len];
        base.Read(arr, 0, len);
        return Encoding.UTF8.GetString(arr);
    }
    public void WriteString(string value)
    {
        byte[] arr = Encoding.UTF8.GetBytes(value);
        if (arr.Length> 65535)
        {
            throw new Exception("长度超出范围");
        }
        this.WriteUShort((ushort)arr.Length);
        base.Write(arr, 0, arr.Length);
    }
    #endregion
	}
}
