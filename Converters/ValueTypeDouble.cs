using System;
using System.Runtime.InteropServices;

namespace DataTypeConverters
{
    public sealed class DataTypeDouble : IDataType
    {
        public int ByteSize { get; } = 8;

        public Type ReferenceType { get; } = typeof(double);

        public double Decode(Span<byte> raw, DataType rawDataType, EndianType endian = EndianType.Big, int offset = 0)
        {
            if (raw.Length - offset < ByteSize) throw new ArgumentOutOfRangeException(nameof(offset), $"Size is too short to parsing data.");

            var bytes = raw.Slice(offset, ByteSize);

            bytes = ApplicationUtils.TransformEndian(bytes, endian);

            return MemoryMarshal.Read<double>(bytes);
        }

        public Span<byte> Encode(double value, EndianType endian = EndianType.Big)
        {
            var ret = BitConverter.GetBytes((double)value);

            return ApplicationUtils.TransformEndian(ret.AsSpan(), endian);
        }

        public double GetRandomValue(Random random)
            => random.NextDouble();
    }
}
