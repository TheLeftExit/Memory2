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

public static class InteropHelper
{
    public static unsafe (nuint BaseAddress, nuint Size)[] ScanRegionInfoCore(HANDLE handle, bool is32Bit)
    {
        ulong stop = is32Bit ? uint.MaxValue : 0x7ffffffffffffffful;
        nuint size = (nuint)sizeof(MEMORY_BASIC_INFORMATION);

        List<(nuint BaseAddress, nuint Size)> list = new();

        MEMORY_BASIC_INFORMATION mbi;
        nuint address = 0;


        while (address < stop && NativeMethods.VirtualQueryEx(handle, (void*)address, &mbi, size) > 0 && address + mbi.RegionSize > address)
        {
            if (mbi.State == VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT &&
                !mbi.Protect.HasFlag(PAGE_PROTECTION_FLAGS.PAGE_NOACCESS) &&
                !mbi.Protect.HasFlag(PAGE_PROTECTION_FLAGS.PAGE_GUARD) &&
                !mbi.Protect.HasFlag(PAGE_PROTECTION_FLAGS.PAGE_NOCACHE))
                list.Add(((nuint)mbi.BaseAddress, mbi.RegionSize));
            address += mbi.RegionSize;
        }

        return list.ToArray();
    }
}