using System.Runtime.InteropServices;

public readonly record struct HANDLE(IntPtr Handle)
{
    public bool IsNull => Handle == IntPtr.Zero;
}

public readonly record struct BOOL(int Value)
{
    public static implicit operator bool(BOOL value) => value.Value != 0;
    public static implicit operator BOOL(bool value) => value ? new(1) : new(0);
}