using System.Collections.Generic;
using System.Linq;

namespace XamarinLab.FileSystem
{
    public class FolderInfoCollection : ObservableList<FolderInfo>
    {
        public FolderInfoCollection(IEnumerable<FolderInfo> list)
        {
            if (list != null) AddRange(list);
            ReadableFolders = new ObservableList<FolderInfo>(GetReadableFolders());
            WriteableFolders = new ObservableList<FolderInfo>(GetWriteableFolders());
            CollectionChanged += OnCollectionChanged;
        }

        public FolderInfoCollection() : this(null)
        { }

        public ObservableList<FolderInfo> ReadableFolders { get; }

        public ObservableList<FolderInfo> WriteableFolders { get; }

        private List<FolderInfo> GetReadableFolders()
        {
            var readable = from folder in this where folder.CanRead select folder;
            return readable.ToList();
        }

        private List<FolderInfo> GetWriteableFolders()
        {
            var writeable = from folder in this where folder.CanWrite select folder;
            return writeable.ToList();
        }

        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ReadableFolders.Clear();
            ReadableFolders.AddRange(GetReadableFolders());

            WriteableFolders.Clear();
            WriteableFolders.AddRange(GetWriteableFolders());
        }
    }
}