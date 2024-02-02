using System.Text;
using KotorDotNET.FileSystemPathing;

namespace KotorDotNET.Common.Data;

/// <summary>
///     A special class designed to store a path that accounts for the case-sensitivity
///     on unix-like systems.
/// </summary>
public class KotorPath
{
    public KotorPath(string path) =>
        Value = path;

    /// <summary>
    ///     The stored path.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     Returns a new KotorPath instance with the specified path adjoined to
    ///     the end of the current instance.
    /// </summary>
    /// <param name="path">The extra path to add to the end.</param>
    /// <returns>A new KotorPath instance with the old and new paths joined.</returns>
    public KotorPath Join(string path) => new(Path.Join(Value, path));
    public bool Exists() => File.Exists(Value) || Directory.Exists(Value);

    private bool ShouldResolveCase() => !OperatingSystem.IsWindows() && Path.IsPathRooted(Value) && !Exists();

    private string NormalizePath()
    {
        if (!ShouldResolveCase())
            return Value;

        StringBuilder currentPath = new(Path.DirectorySeparatorChar.ToString());
        foreach (string segment in Value.Split(Path.DirectorySeparatorChar))
        {
            string? resolvedSegment = ResolveCase(currentPath.ToString(), segment);
            if (string.IsNullOrEmpty(resolvedSegment))
                return Value; // Path not resolved, return original value

            currentPath.Append(resolvedSegment);
            currentPath.Append(Path.DirectorySeparatorChar);
        }

        return currentPath.ToString().TrimEnd(Path.DirectorySeparatorChar);
    }

    private static string? ResolveCase(string currentPath, string segment)
    {
        try
        {
            DirectoryInfo dirInfo = new(currentPath);
            foreach (FileSystemInfo fsInfo in dirInfo.EnumerateFileSystemInfosSafely())
            {
                if (segment.Equals(fsInfo.Name, StringComparison.OrdinalIgnoreCase))
                    return fsInfo.Name;
            }
        }
        catch (Exception)
        {
            return null; // Cannot open directory
        }

        return null; // Segment not found
    }

    public static implicit operator string(KotorPath kPath) => kPath.NormalizePath();
    public static implicit operator KotorPath(string strPath) => new(strPath);

    public static KotorPath operator +(KotorPath a, KotorPath b) =>
        new(a.Value + b.Value);

    public static KotorPath operator /(KotorPath a, KotorPath b) =>
        new(Path.Combine(a.Value, b.Value));
}
