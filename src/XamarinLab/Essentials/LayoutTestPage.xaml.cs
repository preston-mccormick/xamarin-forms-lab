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
    [DisplayName("ScrollView Testing")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LayoutTestPage : ContentPage
	{
		public LayoutTestPage ()
		{
			InitializeComponent ();
            BindingContext = new List<int> { 1, 2, 3, 4, 5, 6 };
		}
	}
}