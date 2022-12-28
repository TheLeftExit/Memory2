public class ProcessMemory : IMemorySource
{
    private ProcessHandle _handle;

    public ProcessMemory(ProcessHandle handle)
    {
        _handle = handle;
    }

    public unsafe bool TryRead(nuint address, nuint byteCount, void* buffer)
    {
        return _handle.TryRead(address, byteCount, buffer);
    }

    public unsafe bool TryWrite(nuint address, nuint byteCount, void* buffer)
    {
        return _handle.TryWrite(address, byteCount, buffer);
    }
}