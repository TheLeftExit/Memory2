using System.Net.NetworkInformation;
using System.Text;

public static unsafe class DllImportHelper
{
    public readonly record struct MemoryRegionInfo(nuint BaseAddress, nuint Size);
    public readonly record struct ProcessInfo(uint ProcessId, string ProcessName);

    // todo: check if can be made easier with https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-zwqueryvirtualmemory
    public static MemoryRegionInfo[] GetMemoryRegions(HANDLE handle, bool is32Bit)
    {
        ulong stop = is32Bit ? uint.MaxValue : 0x7ffffffffffffffful;
        nuint size = (nuint)sizeof(MEMORY_BASIC_INFORMATION);

        List<MemoryRegionInfo> list = new();
        nuint address = 0;

        MEMORY_BASIC_INFORMATION mbi;

        while (address < stop && DllImport.VirtualQueryEx(handle, address, &mbi, size) > 0 && address + mbi.RegionSize > address)
        {
            if (mbi.State == VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT &&
                !mbi.Protect.HasFlag(PAGE_PROTECTION_FLAGS.PAGE_NOACCESS) &&
                !mbi.Protect.HasFlag(PAGE_PROTECTION_FLAGS.PAGE_GUARD) &&
                !mbi.Protect.HasFlag(PAGE_PROTECTION_FLAGS.PAGE_NOCACHE))
                list.Add(new(mbi.BaseAddress, mbi.RegionSize));
            address += mbi.RegionSize;
        }

        return list.ToArray();
    }

    public static ProcessInfo[] GetProcesses()
    {
        var snapshotHandle = DllImport.CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS.TH32CS_SNAPPROCESS, 0);
        if (snapshotHandle.IsNull)
        {
            throw new ApplicationException();
        }
        var processEntry = new PROCESSENTRY32();
        processEntry.dwSize = (uint)sizeof(PROCESSENTRY32);
        if(!DllImport.Process32First(snapshotHandle, &processEntry))
        {
            throw new ApplicationException();
        }

        var list = new List<ProcessInfo>();

        do
        {
            var processId = processEntry.th32ProcessID;

            var rawName = new Span<byte>(processEntry.szExeFile, 260);
            rawName = rawName.Slice(0, rawName.IndexOf<byte>(0));
            var processName = Encoding.UTF8.GetString(rawName);

            list.Add(new(processId, processName));
        }
        while (DllImport.Process32Next(snapshotHandle, &processEntry));

        DllImport.CloseHandle(snapshotHandle);

        return list.ToArray();
    }

    public static MemoryRegionInfo GetMainModuleInfo(uint processId)
    {
        var snapshotHandle = DllImport.CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS.TH32CS_SNAPMODULE, processId);
        if (snapshotHandle.IsNull)
        {
            throw new ApplicationException();
        }
        var moduleEntry = new MODULEENTRY32();
        moduleEntry.dwSize = (uint)sizeof(MODULEENTRY32);
        if (!DllImport.Module32First(snapshotHandle, &moduleEntry))
        {
            return default;
        }

        DllImport.CloseHandle(snapshotHandle);
        return new(moduleEntry.modBaseAddr, moduleEntry.modBaseSize);
    }
}