using System.Runtime.InteropServices;
using System.Text;

public unsafe class Program
{
    public static void Main(string[] args)
    {
        var processInfo = DllImportHelper.GetProcesses().Single(x => x.ProcessName.Contains("Frontiers"));
        var process = new Process(processInfo, PROCESS_ACCESS_RIGHTS.PROCESS_ALL_ACCESS);
        var mainModuleInfo = process.MainModule;

        var snapshot = new MemorySnapshot(process.Memory, mainModuleInfo.BaseAddress, mainModuleInfo.Size);
        List<(string Name, nuint Address, nuint Offset)> typeList = new();

        IClassNameProvider classNameProvider = process.Is32Bit ? new Rtti32ClassNameProvider(snapshot) : new Rtti64ClassNameProvider(snapshot);
        nuint step = process.Is32Bit ? 4u : 8u;
        for (nuint address = mainModuleInfo.BaseAddress; address < mainModuleInfo.BaseAddress + mainModuleInfo.Size; address += step)
        {
            if (classNameProvider.GetClassName(address, PointerDepth.VTable) is string className)
            {
                typeList.Add((className, address, address - mainModuleInfo.BaseAddress));
            }
        }

        ;
    }
}