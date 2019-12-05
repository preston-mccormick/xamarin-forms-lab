using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.Essentials
{
    [DisplayName("Environment: StackLayout")]
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StackLayoutPage : ContentPage
	{
		public StackLayoutPage ()
		{
			InitializeComponent ();
            BindingContext = new TableViewPageModel();
        }
    }
}