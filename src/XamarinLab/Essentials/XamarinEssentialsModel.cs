using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;

namespace XamarinLab.Essentials
{
    public class XamarinEssentialsModel
    {
        public XamarinEssentialsModel()
        {
            MainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            AppInfoStaticType = typeof(AppInfo);
            FileSystemStaticType = typeof(Xamarin.Essentials.FileSystem);
        }

        [DisplayName("Main Display Info")]
        public DisplayInfo MainDisplayInfo { get; }

        [DisplayName("App Info (static)")]
        public Type AppInfoStaticType { get; }


        [DisplayName("File System (static)")]
        public Type FileSystemStaticType { get; }
    }
}
