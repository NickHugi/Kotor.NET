
namespace KotorDotNET.FileSystemPathing
{
    public class InsensitivePath : FileSystemInfo
    {
        private FileSystemInfo _fileSystemInfo { get; set; }
        private bool _isFile { get; }
        public bool IsFile => _isFile;
        public List<FileSystemInfo> FindDuplicates() => PathHelper.FindCaseInsensitiveDuplicates( FullName, includeSubFolders: true, isFile: IsFile ).ToList();
        public override string Name => _fileSystemInfo.Name;
        public override string FullName => _fileSystemInfo.FullName;
        public override bool Exists
        {
            get
            {
                if ( IsFile && File.Exists( FullName ) )
                    return true;
                if ( !IsFile && Directory.Exists( FullName ) )
                    return true;

                Refresh();

                return _fileSystemInfo.Exists;
            }
        }
        public override void Delete()
        {
            _fileSystemInfo.Delete();
            FindDuplicates()?.ToList().ForEach(duplicate => duplicate?.Delete());
        }

        public override string ToString() => FullName;

        public InsensitivePath( FileSystemInfo fileSystemInfo ) => _fileSystemInfo = fileSystemInfo;
        public InsensitivePath( string inputPath, bool isFile )
        {
            string formattedPath = PathHelper.FixPathFormatting( inputPath );
            OriginalPath = formattedPath;
            _isFile = isFile;
            _fileSystemInfo = _isFile
                ? (FileSystemInfo)new FileInfo( formattedPath )
                : (FileSystemInfo)new DirectoryInfo( formattedPath );

            Refresh();
        }

        public new void Refresh()
        {
            if ( _fileSystemInfo is null )
                throw new NullReferenceException("_fileSystemInfo cannot be null");

            _fileSystemInfo.Refresh();

            if ( _fileSystemInfo.Exists )
                return;

            ( string fileSystemItemPath, bool? isFile ) = PathHelper.GetCaseSensitivePath( OriginalPath );

            switch ( isFile )
            {
                case true:
                    _fileSystemInfo = new FileInfo(fileSystemItemPath);
                    break;
                case false:
                    _fileSystemInfo = new DirectoryInfo(fileSystemItemPath);
                    break;
                default:
                    return;
            }

            _fileSystemInfo.Refresh();
        }

        //public static implicit operator string( InsensitivePath insensitivePath ) => insensitivePath._fileSystemInfo?.FullName;
        public static implicit operator FileInfo?( InsensitivePath insensitivePath ) => insensitivePath._fileSystemInfo as FileInfo;
        public static implicit operator DirectoryInfo?( InsensitivePath insensitivePath ) => insensitivePath._fileSystemInfo as DirectoryInfo;
    }
}
