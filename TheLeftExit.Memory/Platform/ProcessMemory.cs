public unsafe sealed class ProcessMemory : IMemorySource
{
    private readonly HANDLE _handle;

    public ProcessMemory (HANDLE handle)
    {
        _handle = handle;
    }

    public unsafe bool TryRead(nuint address, nuint count, void* buffer)
    {
        return NativeMethods.ReadProcessMemory(_handle, (void*)address, buffer, count, out _);
    }

    public unsafe bool TryWrite(nuint address, nuint count, void* buffer)
    {
        return NativeMethods.WriteProcessMemory(_handle, (void*)address, buffer, count, out _);
    }
}
