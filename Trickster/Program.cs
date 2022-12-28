using System.Diagnostics;

public unsafe class Program
{
    public static void Main(string[] args)
    {
        var process = Process.GetProcesses().Single(x => x.Name.Contains("Frontiers"));

        GetAllTypes(process); 
    }

    public static void GetAllRegions(Process process)
    {
        var regions = process.GetMemoryRegions();

        Debugger.Break();
    }

    public static void GetAllTypes(Process process)
    {
        using var handle = process.OpenHandle();

        var memory = new ProcessMemory(handle);
        var mainModuleInfo = process.MainModule;
        var snapshot = new MemorySnapshot(memory, mainModuleInfo.BaseAddress, mainModuleInfo.Size);
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

        Debugger.Break();
    }
}
