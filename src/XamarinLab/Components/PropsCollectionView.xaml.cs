
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropsCollectionView : ContentView
    {
        public PropsCollectionView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty InstanceProperty = BindableProperty.Create(
    "Instance",        // the name of the bindable property
    typeof(object),     // the bindable property type
    typeof(PropsCollectionView),   // the parent object type
    propertyChanged: HandleInstanceChanged);

        public static readonly BindableProperty PropsProperty = BindableProperty.Create(
"Props",        // the name of the bindable property
typeof(PropsModel),     // the bindable property type
typeof(PropsCollectionView)   // the parent object type
);      

        private static void HandleInstanceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var dis = (PropsCollectionView)bindable;
            if (newValue != null && newValue is object o)
            {
                dis.Props = new PropsModel(o);
            }
        }

        public object Instance
        {
            get => GetValue(PropsCollectionView.InstanceProperty);
            set =>SetValue(PropsCollectionView.InstanceProperty, value);
        }



        public PropsModel Props
        {
            get => (PropsModel)GetValue(PropsCollectionView.PropsProperty);
            set => SetValue(PropsCollectionView.PropsProperty, value);
        }
    }
}