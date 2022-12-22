using System.Text;

public enum PointerDepth {
    VTable = 0,
    Instance = 1,
    Reference = 2
}

public interface IClassNameProvider
{
    string? GetClassName(nuint address, PointerDepth depth);
}

public abstract class RttiClassNameProviderBase
{
    protected const int BUFFER_SIZE = 60;

    protected readonly IReadOnlyMemorySource _source;
    protected RttiClassNameProviderBase(IReadOnlyMemorySource source)
    {
        _source = source;
    }

    protected unsafe string? GetDecoratedClassName(ulong address)
    {
        byte* buffer = stackalloc byte[BUFFER_SIZE];
        buffer[0] = (byte)'?';
        if (!_source.TryRead((nuint)address, BUFFER_SIZE - 1, buffer + 1)) return null;
        byte* target = stackalloc byte[BUFFER_SIZE];
        uint len = NativeMethods.UnDecorateSymbolName(buffer, target, BUFFER_SIZE, 0x1800);
        return len != 0 ? Encoding.UTF8.GetString(target, (int)len) : null;
    }
}

public sealed class Rtti32ClassNameProvider : RttiClassNameProviderBase, IClassNameProvider
{
    public Rtti32ClassNameProvider(IReadOnlyMemorySource source) : base(source) { }

    public unsafe string? GetClassName(nuint address, PointerDepth depth)
    {
        for (PointerDepth i = depth; i > PointerDepth.VTable; i--)
            if (!_source.TryRead(address, out address)) return null;

        if (!_source.TryRead(address - 0x08, out ulong object_locator)) return null;
        if (!_source.TryRead((nuint)object_locator + 0x14, out ulong base_offset)) return null;
        ulong base_address = object_locator - base_offset;
        if (!_source.TryRead((nuint)object_locator + 0x0C, out uint type_descriptor_offset)) return null;
        ulong class_name = base_address + type_descriptor_offset + 0x10 + 0x04;
        return GetDecoratedClassName(class_name);
    }
}

public sealed class Rtti64ClassNameProvider : RttiClassNameProviderBase, IClassNameProvider
{
    public Rtti64ClassNameProvider(IReadOnlyMemorySource source) : base(source) { }

    public unsafe string? GetClassName(nuint address, PointerDepth depth)
    {
        for (PointerDepth i = depth; i > PointerDepth.VTable; i--)
            if (!_source.TryRead(address, out address)) return null;

        if (!_source.TryRead(address - 0x08, out ulong object_locator)) return null;
        if (!_source.TryRead((nuint)object_locator + 0x14, out ulong base_offset)) return null;
        ulong base_address = object_locator - base_offset;
        if (!_source.TryRead((nuint)object_locator + 0x0C, out uint type_descriptor_offset)) return null;
        ulong class_name = base_address + type_descriptor_offset + 0x10 + 0x04;
        return GetDecoratedClassName(class_name);
    }
}