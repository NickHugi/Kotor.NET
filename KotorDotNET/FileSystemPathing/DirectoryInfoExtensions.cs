

namespace KotorDotNET.FileSystemPathing
{
    public static class DirectoryInfoExtensions
    {
        private static IEnumerable<T> SafeEnumerate<T>(
            IEnumerator<T> enumerator)
        {
            while (true)
            {
                T thisEntry;
                try
                {
                    if (!enumerator.MoveNext())
                        break;

                    thisEntry = enumerator.Current;
                }
                catch (UnauthorizedAccessException permEx)
                {
                    Console.WriteLine($"Permission denied while enumerating file/folder wildcards: {permEx.Message} Skipping...");
                    continue; // Skip files or directories with access issues
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine($"IO exception enumerating file/folder wildcards: {ioEx.Message} Skipping file/folder...");
                    continue; // Skip files or directories with IO issues
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unhandled exception enumerating file/folder wildcards: {ex.Message}. Attempting to skip file/folder item...");
                    continue;
                }

                if (thisEntry == null)
                    continue;

                yield return thisEntry;
            }
        }

        public static IEnumerable<FileInfo> EnumerateFilesSafely(
            this DirectoryInfo dirInfo,
            string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                return SafeEnumerate(dirInfo.EnumerateFiles(searchPattern, searchOption).GetEnumerator());
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                return Array.Empty<FileInfo>();
            }
        }

        public static IEnumerable<DirectoryInfo> EnumerateDirectoriesSafely(
            this DirectoryInfo dirInfo,
            string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                return SafeEnumerate(dirInfo.EnumerateDirectories(searchPattern, searchOption).GetEnumerator());
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                return Array.Empty<DirectoryInfo>();
            }
        }

        public static IEnumerable<FileSystemInfo> EnumerateFileSystemInfosSafely(
            this DirectoryInfo dirInfo,
            string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                return SafeEnumerate(dirInfo.EnumerateFileSystemInfos(searchPattern, searchOption).GetEnumerator());
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                return Array.Empty<FileSystemInfo>();
            }
        }

        public static DirectoryInfo[] GetDirectoriesSafely(
            this DirectoryInfo dirInfo,
            string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                return dirInfo.EnumerateDirectoriesSafely(searchPattern, searchOption).ToArray();
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                return Array.Empty<DirectoryInfo>();
            }
        }

        public static FileInfo[] GetFilesSafely(
            this DirectoryInfo dirInfo,
            string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                return dirInfo.EnumerateFilesSafely(searchPattern, searchOption).ToArray();
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                return Array.Empty<FileInfo>();
            }
        }

        public static FileSystemInfo[] GetFileInfosSafely(
            this DirectoryInfo dirInfo,
            string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                return dirInfo.EnumerateFileSystemInfosSafely(searchPattern, searchOption).ToArray();
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                return Array.Empty<FileSystemInfo>();
            }
        }
    }
}
