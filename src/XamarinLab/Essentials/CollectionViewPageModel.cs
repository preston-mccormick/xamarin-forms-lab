using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Essentials;
using XamarinLab.Components;

namespace XamarinLab.Essentials
{
    public class CollectionViewPageModel
    {
        public CollectionViewPageModel()
        {
            Sections = new ObservableList<PropsModel>()
            {
                new PropsModel(DeviceDisplay.MainDisplayInfo) { DisplayName = "Display Info" },
                new PropsModel(typeof(AppInfo)) { DisplayName = "App Info" },
                new PropsModel(typeof(Xamarin.Essentials.FileSystem)) { DisplayName = "File System" },
                new PropsModel(typeof(System.Environment)) { DisplayName = "System.Environment" }
            };
        }

        public ObservableList<PropsModel> Sections { get; }
    }
}
