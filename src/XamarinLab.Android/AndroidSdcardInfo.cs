using Android.OS;
using XamarinLab.FileSystem;

namespace XamarinLab.Droid
{
    public partial class MainActivity
    {
        public class AndroidSdcardInfo : SdcardInfo
        {
            public AndroidSdcardInfo()
            {
                Folder = new AndroidFolderInfo("SD Card", Environment.ExternalStorageDirectory);
                StorageState = Environment.GetExternalStorageState(Environment.ExternalStorageDirectory);
                Removable = Environment.IsExternalStorageRemovable;
            }

        }
    }
}