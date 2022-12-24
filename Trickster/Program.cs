using System.Diagnostics;
using System.Text;

unsafe
{
    var process = DllImportHelper.GetProcesses().Single(x => x.ProcessName.Contains("Frontiers"));
    var mainModuleInfo = DllImportHelper.GetMainModuleInfo(process.ProcessId);

    var handle = DllImport.OpenProcess(PROCESS_ACCESS_RIGHTS.PROCESS_ALL_ACCESS, false, process.ProcessId);
    var processMemory = new ProcessMemory(handle);

    BOOL is32Bit;
    DllImport.IsWow64Process(handle, &is32Bit);

    var snapshot = new MemorySnapshot(processMemory, mainModuleInfo.BaseAddress, mainModuleInfo.Size);
    List<(string Name, nuint Address, nuint Offset)> typeList = new();

    IClassNameProvider classNameProvider = is32Bit ? new Rtti32ClassNameProvider(snapshot) : new Rtti64ClassNameProvider(snapshot);
    nuint step = is32Bit ? 4u : 8u;
    for (nuint address = mainModuleInfo.BaseAddress; address < mainModuleInfo.BaseAddress + mainModuleInfo.Size; address += step)
    {
        if (classNameProvider.GetClassName(address, PointerDepth.VTable) is string className)
        {
            typeList.Add((className, address, address - mainModuleInfo.BaseAddress));

        }
    }

    ;
}