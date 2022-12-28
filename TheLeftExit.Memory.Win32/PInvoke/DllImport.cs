using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: DisableRuntimeMarshalling]

internal partial class DllImport
{
    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public unsafe static partial bool ReadProcessMemory(IntPtr hProcess, nuint lpBaseAddress, void* lpBuffer, nuint nSize, void* lpNumberOfBytesRead = null);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public unsafe static partial bool WriteProcessMemory(IntPtr hProcess, nuint lpBaseAddress, void* lpBuffer, nuint nSize, void* lpNumberOfBytesWritten = null);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool IsWow64Process(IntPtr hProcess, [MarshalAs(UnmanagedType.I4)] out bool Wow64Process);

    [LibraryImport("Kernel32")]
    public static partial IntPtr OpenProcess(PROCESS_ACCESS_RIGHTS dwDesiredAccess, [MarshalAs(UnmanagedType.I4)] bool bInheritHandle, uint dwProcessId);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool CloseHandle(IntPtr hObject);

    [LibraryImport("Kernel32")]
    public static partial nuint VirtualQueryEx(IntPtr hProcess, nuint lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, nuint dwLength);

    [LibraryImport("Kernel32")]
    public static partial IntPtr CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS dwFlags, uint th32ProcessID);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Process32FirstW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Process32NextW(IntPtr hSnapshot, ref PROCESSENTRY32W lpme);

    [LibraryImport("Kernel32")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Module32FirstW(IntPtr hSnapshot, ref MODULEENTRY32W lpme);
}