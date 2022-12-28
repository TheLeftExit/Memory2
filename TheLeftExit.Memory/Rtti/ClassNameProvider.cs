using System.Runtime.InteropServices;
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

internal unsafe static partial class SymbolHelper
{
    [LibraryImport("DbgHelp")]
    private static partial uint UnDecorateSymbolName(void* name, void* outputString, uint maxStringLength, uint flags);
    
    private const int BUFFER_SIZE = 60;

    public static string? GetDecoratedClassName(IReadOnlyMemorySource source, ulong address)
    {
        byte* buffer = stackalloc byte[BUFFER_SIZE];
        buffer[0] = (byte)'?';
        if (!source.TryRead((nuint)address, BUFFER_SIZE - 1, buffer + 1)) return null;
        byte* target = stackalloc byte[BUFFER_SIZE];
        uint len = UnDecorateSymbolName(buffer, target, BUFFER_SIZE, 0x1800);
        return len != 0 ? Encoding.UTF8.GetString(target, (int)len) : null;
    }
}

public sealed class Rtti32ClassNameProvider :IClassNameProvider
{
    private readonly IReadOnlyMemorySource _source;
    
    public Rtti32ClassNameProvider(IReadOnlyMemorySource source)
    {
        _source = source;
    }

    public string? GetClassName(nuint address, PointerDepth depth)
    {
        for (PointerDepth i = depth; i > PointerDepth.VTable; i--)
            if (!_source.TryRead(address, out address)) return null;

        if (!_source.TryRead(address - 0x08, out ulong object_locator)) return null;
        if (!_source.TryRead((nuint)object_locator + 0x14, out ulong base_offset)) return null;
        ulong base_address = object_locator - base_offset;
        if (!_source.TryRead((nuint)object_locator + 0x0C, out uint type_descriptor_offset)) return null;
        ulong class_name = base_address + type_descriptor_offset + 0x10 + 0x04;
        return SymbolHelper.GetDecoratedClassName(_source, class_name);
    }
}

public sealed class Rtti64ClassNameProvider : IClassNameProvider
{
    private readonly IReadOnlyMemorySource _source;

    public Rtti64ClassNameProvider(IReadOnlyMemorySource source)
    {
        _source = source;
    }

    public string? GetClassName(nuint address, PointerDepth depth)
    {
        for (PointerDepth i = depth; i > PointerDepth.VTable; i--)
            if (!_source.TryRead(address, out address)) return null;

        if (!_source.TryRead(address - 0x08, out ulong object_locator)) return null;
        if (!_source.TryRead((nuint)object_locator + 0x14, out ulong base_offset)) return null;
        ulong base_address = object_locator - base_offset;
        if (!_source.TryRead((nuint)object_locator + 0x0C, out uint type_descriptor_offset)) return null;
        ulong class_name = base_address + type_descriptor_offset + 0x10 + 0x04;
        return SymbolHelper.GetDecoratedClassName(_source, class_name);
    }
}
