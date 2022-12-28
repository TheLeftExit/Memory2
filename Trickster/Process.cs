public class Process : IDisposable
{
    private readonly ProcessInfo _processInfo;
    private readonly MemoryRegionInfo _mainModule;
    private readonly HANDLE _handle;
    private readonly bool _is32Bit;
    private readonly ProcessMemory _memory;

    public uint Id => _processInfo.ProcessId;
    public string Name => _processInfo.ProcessName;
    public MemoryRegionInfo MainModule => _mainModule;
    public HANDLE Handle => _handle;
    public bool Is32Bit => _is32Bit;
    public ProcessMemory Memory => _memory;

    public Process(ProcessInfo processInfo, PROCESS_ACCESS_RIGHTS processAccessRights)
    {
        _processInfo = processInfo;
        _mainModule = DllImportHelper.GetMainModuleInfo(_processInfo.ProcessId);
        _handle = DllImport.OpenProcess(processAccessRights, false, _processInfo.ProcessId);
        _memory = new(_handle);
        DllImport.IsWow64Process(_handle, out _is32Bit);
    }

    public void Dispose()
    {
        DllImport.CloseHandle(_handle);
    }
}
