using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Essentials;
using XamarinLab.Components;

namespace XamarinLab.Essentials
{
    public class TableViewPageModel
    {
        public TableViewPageModel()
        {
            MainDisplay = new PropsModel(DeviceDisplay.MainDisplayInfo);
            AppInfo = new PropsModel(typeof(AppInfo));
            FileSystem = new PropsModel(typeof(Xamarin.Essentials.FileSystem));
            Environment = new PropsModel(typeof(System.Environment));

        }

        public PropsModel Environment { get; }

        [DisplayName("Main Display Info")]
        public PropsModel MainDisplay { get; }

        [DisplayName("App Info")]
        public PropsModel AppInfo { get; }


        [DisplayName("File System")]
        public PropsModel FileSystem { get; }

    }
}
