using System.Collections.Generic;
using XamarinLab.FileSystem;
using Android.OS;

namespace XamarinLab.Droid
{
    public class AndroidSpecialFolders : IPlatformSpecialFolders
    {
        public AndroidSpecialFolders()
        {
            SpecialFolders.Add("Data", Environment.DataDirectory.CanonicalPath);
            SpecialFolders.Add("Alarms", Environment.DirectoryAlarms);
            SpecialFolders.Add("Dcim", Environment.DirectoryDcim);
            SpecialFolders.Add("Documents", Environment.DirectoryDocuments);
            SpecialFolders.Add("Downloads", Environment.DirectoryDownloads);
            SpecialFolders.Add("Movies", Environment.DirectoryMovies);
            SpecialFolders.Add("Music", Environment.DirectoryMusic);
            SpecialFolders.Add("Notifications", Environment.DirectoryNotifications);
            SpecialFolders.Add("Pictures", Environment.DirectoryPictures);
            SpecialFolders.Add("Podcasts", Environment.DirectoryPodcasts);
            SpecialFolders.Add("Ringtones", Environment.DirectoryRingtones);
            SpecialFolders.Add("DownloadCache", Environment.DownloadCacheDirectory.CanonicalPath);
            SpecialFolders.Add("ExternalStorage", Environment.ExternalStorageDirectory.CanonicalPath);
            SpecialFolders.Add("Root", Environment.RootDirectory.CanonicalPath);
        }

        public Dictionary<string, string> SpecialFolders { get; } = new Dictionary<string, string>();
    }
}