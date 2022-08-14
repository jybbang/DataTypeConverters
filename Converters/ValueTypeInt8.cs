using System;
using System.Runtime.InteropServices;

namespace DataTypeConverters
{
    public sealed class DataTypeInt8 : IDataType
    {
        public int ByteSize { get; } = 1;

        public Type ReferenceType { get; } = typeof(char);

        public double Decode(Span<byte> raw, DataType rawDataType, EndianType endian = EndianType.Big, int offset = 0)
        {
            if (raw.Length - offset < ByteSize) throw new ArgumentOutOfRangeException(nameof(offset), $"Size is too short to parsing data.");

            var bytes = raw.Slice(offset, ByteSize);

            return MemoryMarshal.Read<sbyte>(bytes);
        }

        public Span<byte> Encode(double value, EndianType endian = EndianType.Big)
        {
            return MemoryMarshal.Cast<sbyte, byte>(new[] { (sbyte)value });
        }

        public double GetRandomValue(Random random)
            => Convert.ToSByte(random.Next(sbyte.MinValue, sbyte.MaxValue));
    }
}
