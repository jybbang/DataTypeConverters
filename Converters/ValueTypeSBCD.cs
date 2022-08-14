using System;

namespace DataTypeConverters
{
    public sealed class DataTypeSBCD : IDataType
    {
        public int ByteSize { get; } = 1;

        public Type ReferenceType { get; } = typeof(byte);

        public double Decode(Span<byte> raw, DataType rawDataType, EndianType endian = EndianType.Big, int offset = 0)
        {
            if (raw.Length - offset < ByteSize) throw new ArgumentOutOfRangeException(nameof(offset), $"Size is too short to parsing data.");

            return ApplicationUtils.BcdToNum(raw[offset]);
        }

        public Span<byte> Encode(double value, EndianType endian = EndianType.Big)
        {
            return new byte[] { ((byte)value).NumToBcd() }.AsSpan();
        }

        public double GetRandomValue(Random random)
            => Convert.ToByte(random.Next(0, 99));
    }
}
