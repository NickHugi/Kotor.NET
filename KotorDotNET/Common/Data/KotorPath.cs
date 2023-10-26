using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Common.Data
{
    /// <summary>
    /// A special class designed to store a path that accounts for the case-sensitivity
    /// on unix-like systems.
    /// </summary>
    public class KotorPath
    {
        /// <summary>
        /// The stored path.
        /// </summary>
        public string Value { get; }

        public KotorPath(string path)
        {
            Value = path;
        }

        /// <summary>
        /// Returns a new KotorPath instance with the specified path adjoined to
        /// the end of the current instance.
        /// </summary>
        /// <param name="path">The extra path to add to the end.</param>
        /// <returns>A new KotorPath instance with the old and new paths joined.</returns>
        public KotorPath Join(string path) => new(Path.Join(Value, path));

        [DllImport("libc", SetLastError = true)]
        private static extern IntPtr opendir(string name);

        [DllImport("libc", SetLastError = true)]
        private static extern IntPtr readdir(IntPtr dirp);

        [DllImport("libc", SetLastError = true)]
        private static extern int closedir(IntPtr dirp);

        [StructLayout(LayoutKind.Sequential)]
        private struct Dirent
        {
            public IntPtr d_ino;
            public IntPtr d_off;
            public ushort d_reclen;
            public byte d_type;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string d_name;
        }

        private bool ShouldResolveCase() => !OperatingSystem.IsWindows() && Path.IsPathRooted(Value);

        private string NormalizePath()
        {
            if (!ShouldResolveCase())
                return Value;

            StringBuilder currentPath = new("/");
            foreach (string segment in Value.Split('/'))
            {
                string? resolvedSegment = ResolveCase(currentPath.ToString(), segment);
                if (string.IsNullOrEmpty(resolvedSegment))
                    return Value; // Path not resolved, return original value

                currentPath.Append(resolvedSegment);
                currentPath.Append('/');
            }

            return currentPath.ToString().TrimEnd('/');
        }

        private string? ResolveCase(string currentPath, string segment)
        {
            IntPtr dirPtr = opendir(currentPath);
            if (dirPtr == IntPtr.Zero)
                return null; // Cannot open directory

            IntPtr direntPtr;
            while ((direntPtr = readdir(dirPtr)) != IntPtr.Zero)
            {
                Dirent dirent = Marshal.PtrToStructure<Dirent>(direntPtr);
                if (segment.Equals(dirent.d_name, StringComparison.OrdinalIgnoreCase))
                {
                    closedir(dirPtr);
                    return dirent.d_name;
                }
            }

            closedir(dirPtr);
            return null; // Segment not found
        }

        public static implicit operator KotorPath(string strPath) => new(strPath);
        public static implicit operator string(KotorPath kPath) => kPath.NormalizePath();
    }

}
