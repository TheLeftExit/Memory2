internal unsafe struct MEMORY_BASIC_INFORMATION
{
    public static readonly nuint SIZE = (nuint)sizeof(MEMORY_BASIC_INFORMATION);

    public nuint BaseAddress;
    public nuint AllocationBase;
    public PAGE_PROTECTION_FLAGS AllocationProtect;
    public ushort PartitionId;
    public nuint RegionSize;
    public VIRTUAL_ALLOCATION_TYPE State;
    public PAGE_PROTECTION_FLAGS Protect;
    public PAGE_TYPE Type;
}

internal unsafe struct PROCESSENTRY32W
{
    public uint dwSize;
    public uint cntUsage;
    public uint th32ProcessID;
    public nuint th32DefaultHeapID;
    public uint th32ModuleID;
    public uint cntThreads;
    public uint th32ParentProcessID;
    public int pcPriClassBase;
    public uint dwFlags;
    public fixed char szExeFile[260];

    public static PROCESSENTRY32W Create() => new PROCESSENTRY32W { dwSize = (uint)sizeof(PROCESSENTRY32W) };

    public string szExeFile_ToString() { fixed (char* ptr = szExeFile) return new(ptr, 0, 260); }
}

internal unsafe struct MODULEENTRY32W
{
    public uint dwSize;
    public uint th32ModuleID;
    public uint th32ProcessID;
    public uint GlblcntUsage;
    public uint ProccntUsage;
    public nuint modBaseAddr;
    public uint modBaseSize;
    public IntPtr hModule;
    public fixed char szModule[256];
    public fixed char szExePath[260];

    public static MODULEENTRY32W Create() => new MODULEENTRY32W { dwSize = (uint)sizeof(MODULEENTRY32W) };
}