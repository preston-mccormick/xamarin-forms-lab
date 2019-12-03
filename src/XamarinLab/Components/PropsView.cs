using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace XamarinLab.Components
{
	public class PropsView : ContentView
	{
		public PropsView (PropsViewModel props)
		{
            Content = GetCollectionView(props);
		}

    }
}