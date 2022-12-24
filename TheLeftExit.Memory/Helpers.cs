using System.Runtime.CompilerServices;

public static class MathHelper
{
    public static nuint Translate(nuint address, nuint sourceBase, nuint targetBase)
    {
        return targetBase + (address - sourceBase);
    }

    public static bool CheckBounds(nuint address, nuint size, nuint regionBase, nuint regionSize)
    {
        return address >= regionBase
            && address + size < regionBase + regionSize
            && (nuint.MaxValue - address) >= regionSize;
    }
}
