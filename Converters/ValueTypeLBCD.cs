﻿using System;
using System.Runtime.InteropServices;

namespace DataTypeConverters
{
    public sealed class DataTypeLBCD : IDataType
    {
        public int ByteSize { get; } = 4;

        public Type ReferenceType { get; } = typeof(uint);

        public double Decode(Span<byte> raw, DataType rawDataType, EndianType endian = EndianType.Big, int offset = 0)
        {
            if (raw.Length - offset < ByteSize) throw new ArgumentOutOfRangeException(nameof(offset), $"Size is too short to parsing data.");

            var bytes = raw.Slice(offset, ByteSize);

            bytes = ApplicationUtils.TransformEndian(bytes, endian);

            var bcd = MemoryMarshal.Read<uint>(bytes);

            return ApplicationUtils.BcdToNum(bcd);
        }

        public Span<byte> Encode(double value, EndianType endian = EndianType.Big)
        {
            var ret = BitConverter.GetBytes(((uint)value).NumToBcd());

            return ApplicationUtils.TransformEndian(ret.AsSpan(), endian);
        }

        public double GetRandomValue(Random random)
            => Convert.ToUInt32(random.Next(0, 99999999));
    }
}
