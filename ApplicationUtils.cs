using System;

namespace DataTypeConverters
{
    public static class ApplicationUtils
    {
        private static readonly IDataType _int8DataType = new DataTypeInt8();
        private static readonly IDataType _byteDataType = new DataTypeByte();
        private static readonly IDataType _int16DataType = new DataTypeInt16();
        private static readonly IDataType _wordDataType = new DataTypeWord();
        private static readonly IDataType _int32DataType = new DataTypeInt32();
        private static readonly IDataType _dwordDataType = new DataTypeDWord();
        private static readonly IDataType _int64DataType = new DataTypeInt64();
        private static readonly IDataType _floatDataType = new DataTypeFloat();
        private static readonly IDataType _doubleDataType = new DataTypeDouble();
        private static readonly IDataType _sbcdDataType = new DataTypeSBCD();
        private static readonly IDataType _bcdDataType = new DataTypeBCD();
        private static readonly IDataType _lbcdDataType = new DataTypeLBCD();

        public static IDataType Converter(this DataType dataType)
        {
            return dataType switch
            {
                DataType.Int8 => _int8DataType,
                DataType.Byte => _byteDataType,
                DataType.Int16 => _int16DataType,
                DataType.Word => _wordDataType,
                DataType.Int32 => _int32DataType,
                DataType.Dword => _dwordDataType,
                DataType.Int64 => _int64DataType,
                DataType.Float => _floatDataType,
                DataType.Double => _doubleDataType,
                DataType.Sbcd => _sbcdDataType,
                DataType.Bcd => _bcdDataType,
                DataType.Lbcd => _lbcdDataType,
                _ => throw new NotSupportedException(),
            };
        }

        internal static Span<byte> TransformEndian(this Span<byte> bytes, EndianType endianType)
        {
            switch (endianType)
            {
                case EndianType.Big:
                    {
                        return bytes;
                    }
                case EndianType.Little:
                    {
                        bytes.Reverse();
                        return bytes;
                    }
                case EndianType.BigSwap:
                    {
                        return ByteSwap(bytes);
                    }
                case EndianType.LittleSwap:
                    {
                        bytes.Reverse();
                        return ByteSwap(bytes);
                    }
                default:
                    throw new NotSupportedException();
            }

            static Span<byte> ByteSwap(Span<byte> bytes)
            {
                if (bytes.Length == 1)
                {
                    return bytes;
                }
                if (bytes.Length == 2)
                {
                    var arrays = new byte[bytes.Length];

                    arrays[0] = bytes[1];
                    arrays[1] = bytes[0];

                    return arrays;
                }
                else if (bytes.Length == 4)
                {
                    var arrays = new byte[bytes.Length];

                    arrays[0] = bytes[1];
                    arrays[1] = bytes[0];
                    arrays[2] = bytes[3];
                    arrays[3] = bytes[2];

                    return arrays;
                }
                else if (bytes.Length == 8)
                {
                    var arrays = new byte[bytes.Length];

                    arrays[0] = bytes[1];
                    arrays[1] = bytes[0];
                    arrays[2] = bytes[3];
                    arrays[3] = bytes[2];
                    arrays[4] = bytes[5];
                    arrays[5] = bytes[4];
                    arrays[6] = bytes[7];
                    arrays[7] = bytes[6];

                    return arrays;
                }
                else
                {
                    throw new NotSupportedException("The raw bytes length should be under 8");
                }
            }
        }

        internal static byte NumToBcd(this byte numeric)
        {
            if (numeric > 99)
            {
                throw new ArgumentOutOfRangeException(nameof(numeric), "The bcd number should be under 99.");
            }

            return Convert.ToByte(((numeric / 10) << 4) + (numeric % 10));
        }

        internal static ushort NumToBcd(this ushort numeric)
        {
            if (numeric > 9999)
            {
                throw new ArgumentOutOfRangeException(nameof(numeric), "The bcd number should be under 9999.");
            }

            var ret = 0;
            var shift = 0;

            while (numeric > 0)
            {
                ret |= (numeric % 10) << (shift++ << 2);
                numeric /= 10;
            }

            return Convert.ToUInt16(ret);
        }

        internal static uint NumToBcd(this uint numeric)
        {
            if (numeric > 99999999)
            {
                throw new ArgumentOutOfRangeException(nameof(numeric), "The bcd number should be under 99999999.");
            }

            uint ret = 0;
            var shift = 0;

            while (numeric > 0)
            {
                ret |= (numeric % 10u) << (shift++ << 2);
                numeric /= 10;
            }

            return Convert.ToUInt32(ret);
        }

        internal static byte BcdToNum(this byte bcd)
        {
            return byte.Parse(bcd.ToString("X"));
        }

        internal static ushort BcdToNum(this ushort bcd)
        {
            return ushort.Parse(bcd.ToString("X"));
        }

        internal static uint BcdToNum(this uint bcd)
        {
            return uint.Parse(bcd.ToString("X"));
        }
    }
}
