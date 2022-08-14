using System;
using System.Runtime.InteropServices;

namespace DataTypeConverters
{
    public sealed class DataTypeInt64 : IDataType
    {
        public int ByteSize { get; } = 8;

        public Type ReferenceType { get; } = typeof(long);

        public double Decode(Span<byte> raw, DataType rawDataType, EndianType endian = EndianType.Big, int offset = 0)
        {
            if (raw.Length - offset < ByteSize) throw new ArgumentOutOfRangeException(nameof(offset), $"Size is too short to parsing data.");

            var bytes = raw.Slice(offset, ByteSize);

            bytes = ApplicationUtils.TransformEndian(bytes, endian);

            return MemoryMarshal.Read<long>(bytes);
        }

        public Span<byte> Encode(double value, EndianType endian = EndianType.Big)
        {
            var ret = BitConverter.GetBytes((long)value);

            return ApplicationUtils.TransformEndian(ret.AsSpan(), endian);
        }

        public double GetRandomValue(Random random)
            => Convert.ToInt64(random.Next());
    }
}
