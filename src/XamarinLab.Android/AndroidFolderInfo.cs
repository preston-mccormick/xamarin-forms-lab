using Java.IO;
using XamarinLab.FileSystem;

namespace XamarinLab.Droid
{
    public class AndroidFolderInfo : FolderInfo
    {
        public AndroidFolderInfo(string displayName, string path)
        {
            var javaFile = new File(path);
            DisplayName = displayName;
            Path = javaFile.CanonicalPath;
            CanRead = javaFile.CanRead();
            CanWrite = javaFile.CanWrite();
        }

        public AndroidFolderInfo(string displayName, File javaFile)
        {
            DisplayName = displayName;
            Path = javaFile.CanonicalPath;
            CanRead = javaFile.CanRead();
            CanWrite = javaFile.CanWrite();
        }
    }
}