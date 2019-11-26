using Android.OS;
using XamarinLab.FileSystem;

namespace XamarinLab.Droid
{
    public partial class MainActivity
    {
        public class AndroidFileSystemInfo : IFileSystemInfo
        {
            public AndroidFileSystemInfo()
            {
                var folders = new FolderInfoCollection() {
                new AndroidFolderInfo("Data", Environment.DataDirectory),
                new AndroidFolderInfo("Alarms", Environment.DirectoryAlarms),
                new AndroidFolderInfo("DCIM (Digital Camera Images)", Environment.DirectoryDcim),
                new AndroidFolderInfo("Documents", Environment.DirectoryDocuments),
                new AndroidFolderInfo("Downloads", Environment.DirectoryDownloads),
                new AndroidFolderInfo("Movies", Environment.DirectoryMovies),
                new AndroidFolderInfo("Music", Environment.DirectoryMusic),
                new AndroidFolderInfo("Notifications", Environment.DirectoryNotifications),
                new AndroidFolderInfo("Pictures", Environment.DirectoryPictures),
                new AndroidFolderInfo("Podcasts", Environment.DirectoryPodcasts),
                new AndroidFolderInfo("Ringtones", Environment.DirectoryRingtones),
                new AndroidFolderInfo("Download Cache", Environment.DownloadCacheDirectory),
                new AndroidFolderInfo("External Storage", Environment.ExternalStorageDirectory),
                new AndroidFolderInfo("Root", Environment.RootDirectory)
            };
                InterestingFolders = folders;

                SdcardInfo = new AndroidSdcardInfo();
            }

            public FolderInfoCollection InterestingFolders { get; } = new FolderInfoCollection();

            public SdcardInfo SdcardInfo { get; }
        }
    }
}