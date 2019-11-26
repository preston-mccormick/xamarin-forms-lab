namespace XamarinLab.FileSystem
{
    public class SdcardInfo
    {
        public FolderInfo Folder { get; protected set; }
        public virtual bool Removable { get; protected set; }
        public virtual string StorageState { get; protected set; }
    }
}