using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
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

        return IsLexicallyWithin(
            GetWindowsFinalPath(candidateDirectory),
            GetWindowsFinalPath(ancestorDirectory));
    }

    private static bool IsLexicallyWithin(string path, string directory)
    {
        var comparison = OperatingSystem.IsWindows() || OperatingSystem.IsMacOS()
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

    private static string GetWindowsFinalPath(string path)
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

        var buffer = new StringBuilder(512);
        var length = GetFinalPathNameByHandle(handle, buffer, buffer.Capacity, 0);
        if (length == 0)
        {
            throw new MigrationInputException(
                $"Unable to inspect report directory '{path}': "
                + new Win32Exception(Marshal.GetLastWin32Error()).Message);
        }

        if (length >= buffer.Capacity)
        {
            buffer.EnsureCapacity(checked((int)length + 1));
            length = GetFinalPathNameByHandle(handle, buffer, buffer.Capacity, 0);
            if (length == 0 || length >= buffer.Capacity)
            {
                throw new MigrationInputException(
                    $"Unable to resolve report directory '{path}': "
                    + new Win32Exception(Marshal.GetLastWin32Error()).Message);
            }
        }

        const string UncPrefix = @"\\?\UNC\";
        const string DevicePrefix = @"\\?\";
        var resolved = buffer.ToString();
        if (resolved.StartsWith(UncPrefix, StringComparison.OrdinalIgnoreCase))
        {
            resolved = @"\\" + resolved[UncPrefix.Length..];
        }
        else if (resolved.StartsWith(DevicePrefix, StringComparison.OrdinalIgnoreCase))
        {
            resolved = resolved[DevicePrefix.Length..];
        }

        return ResolveSubstitutedDrive(Path.GetFullPath(resolved));
    }

    private static string ResolveSubstitutedDrive(string path)
    {
        for (var attempt = 0; attempt < 8; attempt++)
        {
            var root = Path.GetPathRoot(path);
            if (root is null || root.Length < 2 || root[1] != ':')
            {
                return path;
            }

            var buffer = new StringBuilder(1024);
            if (QueryDosDevice(root[..2], buffer, buffer.Capacity) == 0)
            {
                throw new MigrationInputException(
                    $"Unable to resolve report drive '{root[..2]}': "
                    + new Win32Exception(Marshal.GetLastWin32Error()).Message);
            }

            const string SubstitutionPrefix = @"\??\";
            var target = buffer.ToString();
            if (!target.StartsWith(
                    SubstitutionPrefix,
                    StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }

            path = Path.GetFullPath(Path.Combine(
                target[SubstitutionPrefix.Length..],
                path[root.Length..]));
        }

        throw new MigrationInputException(
            "Report drive substitution depth exceeds the safety limit.");
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

    [DllImport(
        "kernel32.dll",
        EntryPoint = "GetFinalPathNameByHandleW",
        SetLastError = true,
        CharSet = CharSet.Unicode)]
    private static extern uint GetFinalPathNameByHandle(
        SafeFileHandle file,
        StringBuilder filePath,
        int filePathLength,
        uint flags);

    [DllImport(
        "kernel32.dll",
        EntryPoint = "QueryDosDeviceW",
        SetLastError = true,
        CharSet = CharSet.Unicode)]
    private static extern uint QueryDosDevice(
        string deviceName,
        StringBuilder targetPath,
        int maximumLength);
}
