using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropsCollectionView : ContentView
    {
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

        public static readonly BindableProperty StaticTypeProperty = BindableProperty.Create(
"StaticType",        // the name of the bindable property
typeof(Type),     // the bindable property type
typeof(PropsCollectionView),   // the parent object type
propertyChanged: HandleStaticTypeChanged);

        public PropsCollectionView()
        {
            InitializeComponent();
        }

        public object Instance
        {
            get => GetValue(PropsCollectionView.InstanceProperty);
            set => SetValue(PropsCollectionView.InstanceProperty, value);
        }

        public PropsModel Props
        {
            get => (PropsModel)GetValue(PropsCollectionView.PropsProperty);
            set => SetValue(PropsCollectionView.PropsProperty, value);
        }

        public Type StaticType
        {
            get => (Type)GetValue(PropsCollectionView.StaticTypeProperty);
            set => SetValue(PropsCollectionView.StaticTypeProperty, value);
        }

        private static void HandleInstanceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (PropsCollectionView)bindable;
            if (newValue != null && newValue is object o)
            {
                view.Props = new PropsModel(o);
            }
        }

        private static void HandleStaticTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (PropsCollectionView)bindable;
            if (newValue != null && newValue is Type t)
            {
                view.Props = new PropsModel(t);
            }
        }
    }
}