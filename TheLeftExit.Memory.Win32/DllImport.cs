using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: DisableRuntimeMarshalling]

public unsafe partial class DllImport
{
    [LibraryImport("DbgHelp")]
    public static partial uint UnDecorateSymbolName(void* name, void* outputString, uint maxStringLength, uint flags);
    
    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadProcessMemory(HANDLE hProcess, nuint lpBaseAddress, void* lpBuffer, nuint nSize, void* lpNumberOfBytesRead = null);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteProcessMemory(HANDLE hProcess, nuint lpBaseAddress, void* lpBuffer, nuint nSize, void* lpNumberOfBytesWritten = null);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool IsWow64Process(HANDLE hProcess, [MarshalAs(UnmanagedType.I4)] out bool Wow64Process);

    [LibraryImport("Kernel32")]
    public static partial HANDLE OpenProcess(PROCESS_ACCESS_RIGHTS dwDesiredAccess, [MarshalAs(UnmanagedType.I4)] bool bInheritHandle, uint dwProcessId);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool CloseHandle(HANDLE hObject);

    [LibraryImport("Kernel32")]
    public static partial uint GetProcessId(HANDLE Process);

    [LibraryImport("Kernel32")]
    public static partial nuint VirtualQueryEx(HANDLE hProcess, nuint lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, nuint dwLength);

    [LibraryImport("Kernel32")]
    public static partial HANDLE CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS dwFlags, uint th32ProcessID);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Process32First(HANDLE hSnapshot, ref PROCESSENTRY32 lppe);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Process32Next(HANDLE hSnapshot, ref PROCESSENTRY32 lppe);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Module32First(HANDLE hSnapshot, ref MODULEENTRY32 lpme);
}