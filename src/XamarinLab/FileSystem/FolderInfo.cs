namespace XamarinLab.FileSystem
{
    public class FolderInfo
    {
        public FolderInfo()
        { }

        public bool CanRead { get; set; }

        public bool CanWrite { get; set; }

        public string DisplayName { get; set; }

        /// <summary>
        /// Should be the canonical path.
        /// </summary>
        public string Path { get; set; }
    }
}