using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace PageMenu
{
    [DisplayName("Page List"), Description("Remake of the Page List using a View-Model bound to a CollectionView control.")]
    public partial class PageMenuPage : ContentPage
    {

        public PageMenuPage()
        {
            InitializeComponent();
            this.BindingContext = new PageMenuModel(Navigation, exclude: this.GetType());
        }

    }
}