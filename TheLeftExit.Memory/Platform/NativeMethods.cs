using System.Runtime.InteropServices;

public struct HANDLE
{
    public HANDLE(IntPtr handle)
    {
        Handle = handle;
    }
    public IntPtr Handle { get; }
}

// Method signatures mostly transcribed from CsWin32 output.
public unsafe class NativeMethods
{
    [DllImport("DbgHelp", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern uint UnDecorateSymbolName(void* name, void* outputString, uint maxStringLength, uint flags);

    [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool ReadProcessMemory(HANDLE hProcess, void* lpBaseAddress, void* lpBuffer, nuint nSize, out nuint lpNumberOfBytesRead);

    [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool WriteProcessMemory(HANDLE hProcess, void* lpBaseAddress, void* lpBuffer, nuint nSize, out nuint lpNumberOfBytesWritten);

    [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool IsWow64Process(HANDLE hProcess, out bool Wow64Process);

    [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern HANDLE OpenProcess(PROCESS_ACCESS_RIGHTS dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

    [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool CloseHandle(HANDLE hObject);

    [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern uint GetProcessId(HANDLE Process);

    [DllImport("KERNEL32.dll", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern unsafe nuint VirtualQueryEx(HANDLE hProcess, void* lpAddress, MEMORY_BASIC_INFORMATION* lpBuffer, nuint dwLength);
}

[Flags]
public enum PROCESS_ACCESS_RIGHTS : uint
{
    PROCESS_TERMINATE = 0x00000001,
    PROCESS_CREATE_THREAD = 0x00000002,
    PROCESS_SET_SESSIONID = 0x00000004,
    PROCESS_VM_OPERATION = 0x00000008,
    PROCESS_VM_READ = 0x00000010,
    PROCESS_VM_WRITE = 0x00000020,
    PROCESS_DUP_HANDLE = 0x00000040,
    PROCESS_CREATE_PROCESS = 0x00000080,
    PROCESS_SET_QUOTA = 0x00000100,
    PROCESS_SET_INFORMATION = 0x00000200,
    PROCESS_QUERY_INFORMATION = 0x00000400,
    PROCESS_SUSPEND_RESUME = 0x00000800,
    PROCESS_QUERY_LIMITED_INFORMATION = 0x00001000,
    PROCESS_SET_LIMITED_INFORMATION = 0x00002000,
    PROCESS_ALL_ACCESS = 0x001FFFFF,
    PROCESS_DELETE = 0x00010000,
    PROCESS_READ_CONTROL = 0x00020000,
    PROCESS_WRITE_DAC = 0x00040000,
    PROCESS_WRITE_OWNER = 0x00080000,
    PROCESS_SYNCHRONIZE = 0x00100000,
    PROCESS_STANDARD_RIGHTS_REQUIRED = 0x000F0000,
}

public unsafe struct MEMORY_BASIC_INFORMATION
{
    public void* BaseAddress;
    public void* AllocationBase;
    public PAGE_PROTECTION_FLAGS AllocationProtect;
    public ushort PartitionId;
    public nuint RegionSize;
    public VIRTUAL_ALLOCATION_TYPE State;
    public PAGE_PROTECTION_FLAGS Protect;
    public PAGE_TYPE Type;
}

[Flags]
public enum PAGE_PROTECTION_FLAGS : uint
{
    PAGE_NOACCESS = 0x00000001,
    PAGE_READONLY = 0x00000002,
    PAGE_READWRITE = 0x00000004,
    PAGE_WRITECOPY = 0x00000008,
    PAGE_EXECUTE = 0x00000010,
    PAGE_EXECUTE_READ = 0x00000020,
    PAGE_EXECUTE_READWRITE = 0x00000040,
    PAGE_EXECUTE_WRITECOPY = 0x00000080,
    PAGE_GUARD = 0x00000100,
    PAGE_NOCACHE = 0x00000200,
    PAGE_WRITECOMBINE = 0x00000400,
    PAGE_GRAPHICS_NOACCESS = 0x00000800,
    PAGE_GRAPHICS_READONLY = 0x00001000,
    PAGE_GRAPHICS_READWRITE = 0x00002000,
    PAGE_GRAPHICS_EXECUTE = 0x00004000,
    PAGE_GRAPHICS_EXECUTE_READ = 0x00008000,
    PAGE_GRAPHICS_EXECUTE_READWRITE = 0x00010000,
    PAGE_GRAPHICS_COHERENT = 0x00020000,
    PAGE_GRAPHICS_NOCACHE = 0x00040000,
    PAGE_ENCLAVE_THREAD_CONTROL = 0x80000000,
    PAGE_REVERT_TO_FILE_MAP = 0x80000000,
    PAGE_TARGETS_NO_UPDATE = 0x40000000,
    PAGE_TARGETS_INVALID = 0x40000000,
    PAGE_ENCLAVE_UNVALIDATED = 0x20000000,
    PAGE_ENCLAVE_MASK = 0x10000000,
    PAGE_ENCLAVE_DECOMMIT = 0x10000000,
    PAGE_ENCLAVE_SS_FIRST = 0x10000001,
    PAGE_ENCLAVE_SS_REST = 0x10000002,
    SEC_PARTITION_OWNER_HANDLE = 0x00040000,
    SEC_64K_PAGES = 0x00080000,
    SEC_FILE = 0x00800000,
    SEC_IMAGE = 0x01000000,
    SEC_PROTECTED_IMAGE = 0x02000000,
    SEC_RESERVE = 0x04000000,
    SEC_COMMIT = 0x08000000,
    SEC_NOCACHE = 0x10000000,
    SEC_WRITECOMBINE = 0x40000000,
    SEC_LARGE_PAGES = 0x80000000,
    SEC_IMAGE_NO_EXECUTE = 0x11000000,
}

[Flags]
public enum VIRTUAL_ALLOCATION_TYPE : uint
{
    MEM_COMMIT = 0x00001000,
    MEM_RESERVE = 0x00002000,
    MEM_RESET = 0x00080000,
    MEM_RESET_UNDO = 0x01000000,
    MEM_REPLACE_PLACEHOLDER = 0x00004000,
    MEM_LARGE_PAGES = 0x20000000,
    MEM_RESERVE_PLACEHOLDER = 0x00040000,
    MEM_FREE = 0x00010000,
}

[Flags]
public enum PAGE_TYPE : uint
{
    MEM_PRIVATE = 0x00020000,
    MEM_MAPPED = 0x00040000,
    MEM_IMAGE = 0x01000000,
}