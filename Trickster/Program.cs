using System.Diagnostics;

var process = Process.GetProcesses().Single(x => x.ProcessName.Contains("Frontiers"));
var mainModuleBaseAddress = (nuint)process.MainModule!.BaseAddress;
var mainModuleSize = (nuint)process.MainModule!.ModuleMemorySize;

var handle = NativeMethods.OpenProcess(PROCESS_ACCESS_RIGHTS.PROCESS_ALL_ACCESS, false, (uint)process.Id);
var processMemory = new ProcessMemory(handle);

NativeMethods.IsWow64Process(handle, out bool is32Bit);

var snapshot = new MemorySnapshot(processMemory, mainModuleBaseAddress, mainModuleSize);
List<(string Name, nuint Address, nuint Offset)> typeList = new();

IClassNameProvider classNameProvider = is32Bit ? new Rtti32ClassNameProvider(snapshot) : new Rtti64ClassNameProvider(snapshot);
nuint step = is32Bit ? 4u : 8u;
for(nuint address = mainModuleBaseAddress; address < mainModuleBaseAddress + mainModuleSize; address += step)
{
    if(classNameProvider.GetClassName(address, PointerDepth.VTable) is string className)
    {
        typeList.Add((className, address, address - mainModuleBaseAddress));
    }
}

;