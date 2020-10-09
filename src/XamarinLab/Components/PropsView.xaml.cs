using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropsView : ContentView
    {
        public static readonly BindableProperty InstanceProperty = BindableProperty.Create(
    "Instance",        // the name of the bindable property
    typeof(object),     // the bindable property type
    typeof(PropsView),   // the parent object type
    propertyChanged: HandleInstanceChanged);

        public static readonly BindableProperty PropsProperty = BindableProperty.Create(
"Props",
typeof(PropsModel),
typeof(PropsView)
);

        public static readonly BindableProperty StaticTypeProperty = BindableProperty.Create(
"StaticType",        // the name of the bindable property
typeof(Type),     // the bindable property type
typeof(PropsView),   // the parent object type
propertyChanged: HandleStaticTypeChanged);

        public PropsView()
        {
            InitializeComponent();
        }

        public object Instance
        {
            get => GetValue(PropsView.InstanceProperty);
            set => SetValue(PropsView.InstanceProperty, value);
        }

        public PropsModel Props
        {
            get => (PropsModel)GetValue(PropsView.PropsProperty);
            set => SetValue(PropsView.PropsProperty, value);
        }

        public Type StaticType
        {
            get => (Type)GetValue(PropsView.StaticTypeProperty);
            set => SetValue(PropsView.StaticTypeProperty, value);
        }

        private static void HandleInstanceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (PropsView)bindable;
            if (newValue != null && newValue is object o)
            {
                view.Props = new PropsModel(o);
            }
        }

        private static void HandleStaticTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (PropsView)bindable;
            if (newValue != null && newValue is Type t)
            {
                view.Props = new PropsModel(t);
            }
        }
    }
}