using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace XamarinLab.Components
{
    public class PropsModel
    {
        public PropsModel(object instance, BindingFlags? flags = null) : this(instance.GetType(), instance, flags)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
        }

        public PropsModel(Type type, BindingFlags? flags = null) : this(type, null, flags)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
        }

        protected PropsModel(Type type, object instance, BindingFlags? flags)
        {
            Type = type;

            DisplayName = type.GetCustomAttribute<DisplayNameAttribute>()?
                .DisplayName ?? type.Name;

            if (flags == null)
            {
                flags = instance == null ? BindingFlags.Public | BindingFlags.Static
                : BindingFlags.Public | BindingFlags.Instance;
            }


            PropertyInfo[] props = type.GetProperties(flags.Value);
            //var models = props.Select((prop) => new PropertyModel(prop));
            var models = from prop in props select new PropModel(prop, instance);
            Properties = new ReadOnlyCollection<PropModel>(models.ToList());
        }

        public string DisplayName { get; set; }

        public Type Type { get; }

        public IReadOnlyCollection<PropModel> Properties { get; }
    }

    public class PropModel
    {
        public PropModel(PropertyInfo prop, object instance = null)
        {
            Info = prop;

            DisplayName = prop.GetCustomAttribute<DisplayNameAttribute>()?
                .DisplayName ?? prop.Name;

            Description = prop.GetCustomAttribute<DescriptionAttribute>()?
                .Description;
            try
            {
                Value = prop.GetValue(instance);
                ValueText = Value?.ToString();
                Color = Color.Black;
            }
            catch (Exception error)
            {
                if (error.InnerException != null)
                {
                    ValueText = error.InnerException.Message;
                }
                else
                {
                    ValueText = error.Message;
                }
                Color = Color.Red;
            }

            ReadOnly = !prop.CanWrite;
        }

        public PropertyInfo Info { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public object Value { get; }

        public string ValueText { get; }

        public bool ReadOnly { get; }

        public Color Color { get; }
    }
}
