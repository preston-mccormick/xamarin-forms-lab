using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinLab.Components;

namespace XamarinLab.Essentials
{
    [DisplayName("Xamarin.Essentials: CollectionView")]
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PropsCollectionPage : ContentPage
	{
		public PropsCollectionPage ()
		{
			InitializeComponent ();

            //BindingContext = new PropsModel(DeviceDisplay.MainDisplayInfo);
            BindingContext = new XamarinEssentialsModel();
            //BindingContext = new PropsModel(typeof(Xamarin.Essentials.DeviceDisplay));
            //propsView.BindingContext = new PropsModel(DeviceDisplay.MainDisplayInfo);
        }
	}
}