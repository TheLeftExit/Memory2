using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

public static class SnapshotHelper
{
    public static T Translate<T>(T address, T sourceBase, T targetBase) where T : INumber<T>
    {
        return targetBase + (address - sourceBase);
    }

    public static bool CheckBoundsBaseSize<T>(T address, T size, T regionBase, T regionSize) where T : INumber<T>, IMinMaxValue<T>
    {
        return address >= regionBase
            && address + size <= regionBase + regionSize
            && T.MaxValue - size >= address;
    }

    public static bool CheckBoundsMinMax<T>(T address, T size, T minAddress, T maxAddress) where T : INumber<T>, IMinMaxValue<T>
    {
        return address >= minAddress
            && address + size <= maxAddress
            && T.MaxValue - size >= address;
    }

    public static T FindFirstGreaterOrEqual<T, TList>(TList array, T target) where T : INumber<T> where TList: IReadOnlyList<T>
    {
        var low = 0;
        var high = array.Count - 1;

        while (low <= high)
        {
            int mid = (low + high) / 2;
            if (array[mid] < target)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }
        return array[low];
    }
}
