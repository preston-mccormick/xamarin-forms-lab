using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.Essentials
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AppInfoView : ContentView
	{
		public AppInfoView ()
		{
			InitializeComponent ();
		}
	}

    public class AppInfoModel
    {
        public AppInfoModel()
        {
            Name = AppInfo.Name;
            Package = AppInfo.PackageName;
            Version = AppInfo.VersionString;
            Build = AppInfo.BuildString;
            ShowSettings = new Command(AppInfo.ShowSettingsUI);
        }


        public Command ShowSettings { get; }

        public string Name { get; }

        public string Version { get; }

        public string Build { get; }

        public string Package { get; }
    }
}