using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace XamarinLab.Components
{
	public class PropView : StackLayout
	{
        public PropView(PropModel prop)
        {
            Children.Add(new Label
            {
                Text = prop.DisplayName,
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                Padding = new Thickness(1)
            });
            if (prop.ReadOnly)
            {
                Children.Add(new Label
                {
                    Text = prop.Value,
                    FontSize = 18,
                    Padding = new Thickness(15, 1, 1, 1)
                });
            }
            else
            {
                Children.Add(new Entry
                {
                    Text = prop.Value,
                    FontSize = 18,
                    HorizontalTextAlignment = TextAlignment.Center,
                    IsReadOnly = prop.ReadOnly
                });
            }
        }
    }
}