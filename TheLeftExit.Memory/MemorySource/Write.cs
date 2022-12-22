public unsafe interface IMemorySource : IReadOnlyMemorySource
{
    bool TryWrite(nuint address, nuint byteCount, void* buffer);
}

public static unsafe partial class MemorySourceExtensions
{
    public static bool TryWrite<T>(this IMemorySource source, nuint address, T value) where T : unmanaged
    {
        return source.TryWrite(address, (nuint)sizeof(T), &value);
    }
    public static bool TryWrite<T>(this IMemorySource source, nuint address, in T value) where T : unmanaged
    {
        fixed (void* ptr = &value)
        {
            return source.TryWrite(address, (nuint)sizeof(T), ptr);
        }
    }
    public static void Write<T>(this IMemorySource source, nuint address, T value) where T : unmanaged
    {
        if (!source.TryWrite(address, value))
        {
            throw new ApplicationException();
        }
    }
    public static void Write<T>(this IMemorySource source, nuint address, in T value) where T : unmanaged
    {
        if (!source.TryWrite(address, value))
        {
            throw new ApplicationException();
        }
    }

    public static bool TryWrite<T>(this IMemorySource source, nuint address, Span<T> buffer) where T : unmanaged
    {
        fixed (void* ptr = buffer)
        {
            return source.TryWrite(address, (nuint)buffer.Length * (nuint)sizeof(T), ptr);
        }
    }
}