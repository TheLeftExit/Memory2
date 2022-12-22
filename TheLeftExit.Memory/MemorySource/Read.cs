public unsafe interface IReadOnlyMemorySource
{
    bool TryRead(nuint address, nuint byteCount, void* buffer);
}

public static unsafe partial class MemorySourceExtensions
{
    public static bool TryRead<T>(this IReadOnlyMemorySource source, nuint address, out T result) where T : unmanaged
    {
        fixed (void* ptr = &result)
        {
            return source.TryRead(address, (nuint)sizeof(T), ptr);
        }
    }
    public static bool TryRead<T>(this IReadOnlyMemorySource source, nuint address, Span<T> buffer) where T : unmanaged
    {
        fixed (void* ptr = buffer)
        {
            return source.TryRead(address, (nuint)buffer.Length * (nuint)sizeof(T), ptr);
        }
    }
    public static T Read<T>(this IReadOnlyMemorySource source, nuint address) where T : unmanaged
    {
        return source.TryRead(address, out T result) ? result : throw new ApplicationException();
    }
}