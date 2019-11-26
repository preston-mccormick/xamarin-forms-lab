namespace XamarinLab.FileSystem
{
    public interface IFileSystemInfo
    {
        FolderInfoCollection InterestingFolders { get; }

        SdcardInfo SdcardInfo { get; }
    }
}