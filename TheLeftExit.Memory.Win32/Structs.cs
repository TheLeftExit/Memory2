using System.Runtime.CompilerServices;

public struct MEMORY_BASIC_INFORMATION
{
    public nuint BaseAddress;
    public nuint AllocationBase;
    public PAGE_PROTECTION_FLAGS AllocationProtect;
    public ushort PartitionId;
    public nuint RegionSize;
    public VIRTUAL_ALLOCATION_TYPE State;
    public PAGE_PROTECTION_FLAGS Protect;
    public PAGE_TYPE Type;
}

public unsafe struct PROCESSENTRY32
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
    public fixed byte szExeFile[260];
}

public unsafe struct MODULEENTRY32
{
    public uint dwSize;
    public uint th32ModuleID;
    public uint th32ProcessID;
    public uint GlblcntUsage;
    public uint ProccntUsage;
    public nuint modBaseAddr;
    public uint modBaseSize;
    public HANDLE hModule;
    public fixed char szModule[256];
    public fixed char szExePath[260];
}