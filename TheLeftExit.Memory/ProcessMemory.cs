public unsafe sealed class ProcessMemory : IMemorySource
{
    private readonly HANDLE _handle;

    public ProcessMemory (HANDLE handle)
    {
        _handle = handle;
    }

    public bool TryRead(nuint address, nuint count, void* buffer)
    {
        return DllImport.ReadProcessMemory(_handle, address, buffer, count);
    }

    public unsafe bool TryWrite(nuint address, nuint count, void* buffer)
    {
        return DllImport.WriteProcessMemory(_handle, address, buffer, count);
    }
}
