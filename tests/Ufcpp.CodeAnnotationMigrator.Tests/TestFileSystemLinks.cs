using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Ufcpp.CodeAnnotationMigrator.Tests;

internal static class TestFileSystemLinks
{
    public static void CreateHardLink(string linkPath, string existingPath)
    {
        var success = OperatingSystem.IsWindows()
            ? CreateHardLinkWindows(linkPath, existingPath, 0)
            : CreateHardLinkUnix(existingPath, linkPath) == 0;
        if (!success)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    [DllImport(
        "kernel32.dll",
        EntryPoint = "CreateHardLinkW",
        SetLastError = true,
        CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CreateHardLinkWindows(
        string fileName,
        string existingFileName,
        nint securityAttributes);

    [DllImport("libc", EntryPoint = "link", SetLastError = true)]
    private static extern int CreateHardLinkUnix(
        string existingFileName,
        string fileName);
}
