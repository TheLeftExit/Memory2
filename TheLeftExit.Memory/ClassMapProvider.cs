public class ClassMapProvider
{
    private readonly IReadOnlyMemorySource _source;
    private readonly nuint _mainModuleBaseAddress;
    private readonly nuint _mainModuleSize;
    private readonly bool _is32Bit;

    public ClassMapProvider(IReadOnlyMemorySource source, nuint mainModuleBaseAddress, nuint mainModuleSize, bool is32Bit)
    {
        _source = source;
        _mainModuleBaseAddress = mainModuleBaseAddress;
        _mainModuleSize = mainModuleSize;
        _is32Bit = is32Bit;
    }

    public 
}