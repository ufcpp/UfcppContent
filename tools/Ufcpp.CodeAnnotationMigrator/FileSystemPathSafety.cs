using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Ufcpp.CodeAnnotationMigrator;

internal static class FileSystemPathSafety
{
    public static bool IsDirectoryWithin(
        string candidateDirectory,
        string ancestorDirectory)
    {
        if (!OperatingSystem.IsWindows())
        {
            return IsLexicallyWithin(candidateDirectory, ancestorDirectory);
        }

        var ancestor = GetWindowsDirectoryIdentity(ancestorDirectory);
        for (var current = Path.GetFullPath(candidateDirectory);
             !string.IsNullOrEmpty(current);
             current = Directory.GetParent(current)?.FullName)
        {
            if (GetWindowsDirectoryIdentity(current) == ancestor)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsLexicallyWithin(string path, string directory)
    {
        var comparison = OperatingSystem.IsMacOS()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        var normalizedDirectory = directory.TrimEnd(
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar);
        return path.Equals(normalizedDirectory, comparison)
            || path.StartsWith(
                normalizedDirectory + Path.DirectorySeparatorChar,
                comparison)
            || Path.AltDirectorySeparatorChar != Path.DirectorySeparatorChar
            && path.StartsWith(
                normalizedDirectory + Path.AltDirectorySeparatorChar,
                comparison);
    }

    private static WindowsDirectoryIdentity GetWindowsDirectoryIdentity(string path)
    {
        using var handle = CreateFile(
            path,
            0,
            FileShare.ReadWrite | FileShare.Delete,
            0,
            FileMode.Open,
            FileFlagBackupSemantics,
            0);
        if (handle.IsInvalid)
        {
            throw new MigrationInputException(
                $"Unable to validate report directory '{path}': "
                + new Win32Exception(Marshal.GetLastWin32Error()).Message);
        }

        if (!GetFileInformationByHandle(handle, out var information))
        {
            throw new MigrationInputException(
                $"Unable to inspect report directory '{path}': "
                + new Win32Exception(Marshal.GetLastWin32Error()).Message);
        }

        return new WindowsDirectoryIdentity(
            information.VolumeSerialNumber,
            ((ulong)information.FileIndexHigh << 32) | information.FileIndexLow);
    }

    private const uint FileFlagBackupSemantics = 0x02000000;

    [DllImport(
        "kernel32.dll",
        EntryPoint = "CreateFileW",
        SetLastError = true,
        CharSet = CharSet.Unicode)]
    private static extern SafeFileHandle CreateFile(
        string fileName,
        uint desiredAccess,
        FileShare shareMode,
        nint securityAttributes,
        FileMode creationDisposition,
        uint flagsAndAttributes,
        nint templateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetFileInformationByHandle(
        SafeFileHandle file,
        out ByHandleFileInformation information);

    [StructLayout(LayoutKind.Sequential)]
    private struct ByHandleFileInformation
    {
        public uint FileAttributes;
        public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
        public uint VolumeSerialNumber;
        public uint FileSizeHigh;
        public uint FileSizeLow;
        public uint NumberOfLinks;
        public uint FileIndexHigh;
        public uint FileIndexLow;
    }

    private readonly record struct WindowsDirectoryIdentity(
        uint VolumeSerialNumber,
        ulong FileIndex);
}
