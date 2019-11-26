using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinLab.PageList
{
    [DisplayName("Page List"), Description("Remake of the Page List using a View-Model bound to a CollectionView control.")]
    public partial class PageListCollectionView : ContentPage
    {

        public PageListCollectionView()
        {
            InitializeComponent();
            this.BindingContext = new PageListViewModel(Navigation, exclude: this.GetType());
        }

    }
}