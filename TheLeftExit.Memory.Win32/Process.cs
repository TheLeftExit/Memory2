public readonly record struct MemoryRegionInfo(nuint BaseAddress, nuint Size);

public sealed class Process
{
    public uint Id { get; }
    public string Name { get; }

    public MemoryRegionInfo MainModule => _mainModule ??= GetMainModuleInfo(Id);
    private MemoryRegionInfo? _mainModule;
    
    public bool Is32Bit => _is32Bit ??= IsWow64Process(Id);
    private bool? _is32Bit;

    public ProcessHandle OpenHandle()
    {
        var handle = DllImport.OpenProcess(PROCESS_ACCESS_RIGHTS.PROCESS_ALL_ACCESS, false, Id);
        return new(handle);
    }

    public MemoryRegionInfo[] GetMemoryRegions()
    {
        var processHandle = DllImport.OpenProcess(PROCESS_ACCESS_RIGHTS.PROCESS_QUERY_INFORMATION, false, Id);
        
        ulong stop = Is32Bit ? uint.MaxValue : (ulong)long.MaxValue;
        nuint size = MEMORY_BASIC_INFORMATION.SIZE;

        List<MemoryRegionInfo> list = new();
        nuint address = 0;

        while (address < stop && DllImport.VirtualQueryEx(processHandle, address, out var mbi, size) > 0 && mbi.RegionSize > 0)
        {
            if (mbi.State == VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT &&
                !mbi.Protect.HasFlag(PAGE_PROTECTION_FLAGS.PAGE_NOACCESS) &&
                !mbi.Protect.HasFlag(PAGE_PROTECTION_FLAGS.PAGE_GUARD) &&
                !mbi.Protect.HasFlag(PAGE_PROTECTION_FLAGS.PAGE_NOCACHE))
            {
                list.Add(new(mbi.BaseAddress, mbi.RegionSize));
            }
            address += mbi.RegionSize;
        }

        DllImport.CloseHandle(processHandle);

        return list.ToArray();
    }

    public static Process[] GetProcesses()
    {
        var snapshotHandle = DllImport.CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS.TH32CS_SNAPPROCESS, 0);
        var processEntry = PROCESSENTRY32W.Create();
        DllImport.Process32FirstW(snapshotHandle, ref processEntry);
        var list = new List<Process>();
        do
        {
            var processId = processEntry.th32ProcessID;
            var processName = processEntry.szExeFile_ToString();
            list.Add(new(processId, processName));
        }
        while (DllImport.Process32NextW(snapshotHandle, ref processEntry));
        DllImport.CloseHandle(snapshotHandle);
        return list.ToArray();
    }

    internal Process(uint id, string name)
    {
        Id = id;
        Name = name;
    }

    internal static MemoryRegionInfo GetMainModuleInfo(uint processId)
    {
        var snapshotHandle = DllImport.CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS.TH32CS_SNAPMODULE, processId);
        var moduleEntry = MODULEENTRY32W.Create();
        DllImport.Module32FirstW(snapshotHandle, ref moduleEntry);
        DllImport.CloseHandle(snapshotHandle);
        return new(moduleEntry.modBaseAddr, moduleEntry.modBaseSize);
    }

    internal static bool IsWow64Process(uint processId)
    {
        var processHandle = DllImport.OpenProcess(PROCESS_ACCESS_RIGHTS.PROCESS_QUERY_INFORMATION, false, processId);
        DllImport.IsWow64Process(processHandle, out var isWow64Process);
        DllImport.CloseHandle(processHandle);
        return isWow64Process;
    }
}

public unsafe sealed class ProcessHandle : IDisposable
{
    private readonly IntPtr _handle;
    internal ProcessHandle(IntPtr handle)
    {
        _handle = handle;
    }

    public bool TryRead(nuint address, nuint byteCount, void* buffer)
    {
        return DllImport.ReadProcessMemory(_handle, address, buffer, byteCount);
    }

    public bool TryWrite(nuint address, nuint byteCount, void* buffer)
    {
        return DllImport.WriteProcessMemory(_handle, address, buffer, byteCount);
    }

    public void Dispose()
    {
        DllImport.CloseHandle(_handle);
    }
}