using System.Runtime.InteropServices;

namespace NiceSon;

internal static partial class WindowsClipboard
{
    internal static void SetClipboardData(string value)
    {
        OpenClipboard(IntPtr.Zero);
        var ptr = Marshal.StringToHGlobalUni(value);

        // 13 = Unicode string
        SetClipboardData(13, ptr);

        CloseClipboard();
        Marshal.FreeHGlobal(ptr);
    }

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool OpenClipboard(IntPtr hWndNewOwner);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CloseClipboard();

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetClipboardData(uint uFormat, IntPtr data);
}
