using System.Runtime.InteropServices;

/*

1. Why CsWin32 isn't used.
   - It creates redundant wrappers for pointers (e.g PCSTR/PSTR structs for UnDecorateSymbolName), cluttering up my code.
   - It will not let me specify the namespace for structs/enums it generates.
2. Win32 API translation conventions.
   - All process handle parameters are HANDLE - the one pointer wrapper that actually makes sense.
   - All pointer parameters that do not refer to an address in the calling application are nuint.
   - Field types in structs are left as they are in CsWin32 (i.e. above rules do not apply) unless they start getting in the way.
*/

public unsafe class DllImport
{
    [DllImport("DbgHelp")]
    public static extern uint UnDecorateSymbolName(void* name, void* outputString, uint maxStringLength, uint flags);

    [DllImport("Kernel32")]
    public static extern BOOL ReadProcessMemory(HANDLE hProcess, nuint lpBaseAddress, void* lpBuffer, nuint nSize, nuint* lpNumberOfBytesRead = null);

    [DllImport("Kernel32")]
    public static extern BOOL WriteProcessMemory(HANDLE hProcess, nuint lpBaseAddress, void* lpBuffer, nuint nSize, nuint* lpNumberOfBytesWritten = null);

    [DllImport("Kernel32")]
    public static extern BOOL IsWow64Process(HANDLE hProcess, BOOL* Wow64Process);

    [DllImport("Kernel32")]
    public static extern HANDLE OpenProcess(PROCESS_ACCESS_RIGHTS dwDesiredAccess, BOOL bInheritHandle, uint dwProcessId);

    [DllImport("Kernel32")]
    public static extern bool CloseHandle(HANDLE hObject);

    [DllImport("Kernel32")]
    public static extern uint GetProcessId(HANDLE Process);

    [DllImport("Kernel32")]
    public static extern nuint VirtualQueryEx(HANDLE hProcess, nuint lpAddress, MEMORY_BASIC_INFORMATION* lpBuffer, nuint dwLength);

    [DllImport("Kernel32")]
    public static extern HANDLE CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS dwFlags, uint th32ProcessID);

    [DllImport("Kernel32")]
    public static extern BOOL Process32First(HANDLE hSnapshot, PROCESSENTRY32* lppe);

    [DllImport("Kernel32")]
    public static extern BOOL Process32Next(HANDLE hSnapshot, PROCESSENTRY32* lppe);

    [DllImport("Kernel32")]
    public static extern BOOL Module32First(HANDLE hSnapshot, MODULEENTRY32* lpme);
}