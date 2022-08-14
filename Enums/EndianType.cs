namespace DataTypeConverters
{
    public enum EndianType
    {
        // ABCD
        Big,
        // DCBA
        Little,
        // BADC
        BigSwap,
        // CDAB
        LittleSwap,
    }
}
