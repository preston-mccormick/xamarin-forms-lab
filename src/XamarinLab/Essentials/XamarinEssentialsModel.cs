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
        }

        [DisplayName("Main Display Info")]
        public DisplayInfo MainDisplayInfo { get; }
    }
}
