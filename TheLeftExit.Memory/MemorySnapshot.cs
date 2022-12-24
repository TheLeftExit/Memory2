using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public unsafe sealed class MemorySnapshot : IReadOnlyMemorySource, IDisposable
{
    private readonly nuint _baseAddress;
    private readonly nuint _size;
    private readonly void* _pointer;

    public nuint BaseAddress => _baseAddress;
    public nuint Size => _size;

    public MemorySnapshot(IReadOnlyMemorySource source, nuint baseAddress, nuint size)
    {
        _baseAddress = baseAddress;
        _size = size;
        _pointer = NativeMemory.Alloc(size);
        if(!source.TryRead(baseAddress, size, _pointer))
        {
            throw new ApplicationException();
        }
    }

    public bool TryRead(nuint address, nuint byteCount, void* buffer)
    {
        if(byteCount > uint.MaxValue)
        {
            return false;
        }
        nuint newAddress = MathHelper.Translate(address, _baseAddress, (nuint)_pointer);
        if (!MathHelper.CheckBounds(newAddress, byteCount, (nuint)_pointer, _size))
        {
            return false;
        }
        Unsafe.CopyBlock(buffer, (void*)newAddress, (uint)byteCount);
        return true;
    }

    public void Dispose()
    {
        NativeMemory.Free(_pointer);
    }
}