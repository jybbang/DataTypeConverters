using System;
using System.Runtime.InteropServices;

namespace DataTypeConverters
{
    public sealed class DataTypeBCD : IDataType
    {
        public int ByteSize { get; } = 2;

        public Type ReferenceType { get; } = typeof(ushort);

        public double Decode(Span<byte> raw, DataType rawDataType, EndianType endian = EndianType.Big, int offset = 0)
        {
            if (raw.Length - offset < ByteSize) throw new ArgumentOutOfRangeException(nameof(offset), $"Size is too short to parsing data.");

            var bytes = raw.Slice(offset, ByteSize);

            bytes = ApplicationUtils.TransformEndian(bytes, endian);

            var bcd = MemoryMarshal.Read<ushort>(bytes);

            return ApplicationUtils.BcdToNum(bcd);
        }

        public Span<byte> Encode(double value, EndianType endian = EndianType.Big)
        {
            var ret = BitConverter.GetBytes(((ushort)value).NumToBcd());

            return ApplicationUtils.TransformEndian(ret.AsSpan(), endian);
        }

        public double GetRandomValue(Random random)
            => Convert.ToUInt16(random.Next(0, 9999));
    }
}
