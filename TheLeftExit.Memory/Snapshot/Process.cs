using System.Collections.Immutable;

public sealed class ProcessSnapshot : IReadOnlyMemorySource, IDisposable
{
    private readonly ImmutableDictionary<nuint, MemorySnapshot> _snapshotsByBaseAddress;
    private readonly ImmutableArray<nuint> _sortedBaseAddresses;
    private readonly ImmutableArray<MemorySnapshot> _sortedSnapshots;
    private readonly nuint _minAddress;
    private readonly nuint _maxAddress;

    public ImmutableDictionary<nuint, MemorySnapshot> SnapshotsByBaseAddress => _snapshotsByBaseAddress;
    public ImmutableArray<nuint> BaseAddresses => _sortedBaseAddresses;
    public ImmutableArray<MemorySnapshot> Snapshots => _sortedSnapshots;
    public nuint MinAddress => _minAddress;
    public nuint MaxAddress => _maxAddress;

    public ProcessSnapshot(IEnumerable<MemorySnapshot> memorySnapshots)
    {
        _snapshotsByBaseAddress = memorySnapshots.ToImmutableDictionary(x => x.BaseAddress, x => x);
        _sortedSnapshots = memorySnapshots.OrderBy(x => x.BaseAddress).ToImmutableArray();

        _sortedBaseAddresses = _sortedSnapshots.Select(x => x.BaseAddress).ToImmutableArray();
        _minAddress = _sortedSnapshots[0].BaseAddress;
        _maxAddress = _sortedSnapshots[^1].BaseAddress + _sortedSnapshots[^1].Size;
    }

    public unsafe bool TryRead(nuint address, nuint byteCount, void* buffer)
    {
        if(!SnapshotHelper.CheckBoundsMinMax(address, byteCount, _minAddress, _maxAddress))
        {
            return false;
        }
        var possibleBaseAddress = SnapshotHelper.FindFirstGreaterOrEqual(_sortedBaseAddresses, address);
        var possibleRegion = _snapshotsByBaseAddress[possibleBaseAddress];
        return possibleRegion.TryRead(address, byteCount, buffer);
    }

    public void Dispose()
    {
        foreach(var snapshot in _sortedSnapshots)
        {
            snapshot.Dispose();
        }
    }
}