using System;

namespace DataTypeConverters
{
    public interface IDataType
    {
        int ByteSize { get; }

        Type ReferenceType { get; }

        double Decode(Span<byte> raw, DataType rawDataType, EndianType endian = EndianType.Big, int offset = 0);

        Span<byte> Encode(double value, EndianType endian = EndianType.Big);

        double GetRandomValue(Random random);
    }
}
