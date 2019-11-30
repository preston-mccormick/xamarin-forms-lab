using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace XamarinLab.Mvvm
{
    /// <summary>
    /// Provides a base implementation of INotifyPropertyChanged. This interface
    /// is primarily used to notify data bound UI controls to update their
    /// display to reflect the current bound property state.
    ///
    /// Provides a base implementation of IDataErrorInfo. This interface
    /// provides error information to data bound UI controls. Helper methods
    /// SetError and RemoveError can be used to simplify implementation of error
    /// notification in the properties of derived types.
    ///
    /// Provides a base validation implementation. Basic validation methods for
    /// use by derived classes for common validation scenarios.
    ///
    /// Use Validate() to force revalidation of all properties. Use
    /// GetValidationErrors() to retrieve a list of all validation errors on the object.
    /// </summary>
    [Serializable]
    public abstract partial class BaseModel : INotifyPropertyChanged, IDataErrorInfo
    {
        /// <summary>
        /// Set to true while initializing to suspend INotifyPropertyChanged
        /// notification. Derived types may also want to suspend other events.
        /// Events are fairly slow and generally not needed until after the
        /// object is fully initialized.
        /// </summary>
        public bool LoadMode = false;

        protected string _errorMessage = "There is a problem with the data.";

        protected Hashtable _propertyErrors = new Hashtable();

        protected bool _showPropertyNameInError = true;

        [NonSerialized] // Don't clone or serialize event handlers!
        private PropertyChangedEventHandler _propertyChanged;

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return (string)_propertyErrors[propertyName];
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (_propertyErrors.Count == 0)
                {
                    // Null is treated as no error.
                    return null;
                }
                else
                {
                    // If the error collection is not empty, there must be errors.

                    StringBuilder sb = new StringBuilder();
                    AttributeGetter attributeGetter = new AttributeGetter(this.GetType());
                    IEnumerator e = _propertyErrors.GetEnumerator();
                    while (e.MoveNext())
                    {
                        if (_showPropertyNameInError)
                        {
                            DictionaryEntry dictionaryEntry = (DictionaryEntry)e.Current;
                            string displayName = attributeGetter.GetDisplayName(dictionaryEntry.Key as string);
                            string errorLine = String.Format("{0}: {1}", displayName, dictionaryEntry.Value);
                            sb.AppendLine(errorLine);
                        }
                        else
                        {
                            DictionaryEntry dictionaryEntry = (DictionaryEntry)e.Current;
                            string errorLine = String.Format("{0}", dictionaryEntry.Value);
                            sb.AppendLine(errorLine);
                        }
                    }
                    return sb.ToString().Trim();
                }
            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                _propertyChanged += value;
            }
            remove
            {
                _propertyChanged -= value;
            }
        }

        /// <summary>
        /// Deep Copy Clone. Use with care.
        /// </summary>
        public object Clone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Serialize this object to a memory stream.
                BinaryFormatter formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Remoting));
                formatter.Serialize(stream, this);

                // Reset the stream.
                stream.Position = 0;

                // Deserialize a copy of the object from the memory stream.
                object clone = formatter.Deserialize(stream);

                // Return the cloned object.
                return clone;
            }
        }

        /// <summary>
        /// Creates a new instance and copies all property values from this instance.
        /// </summary>
        public virtual BaseModel Copy()
        {
            Type t = this.GetType();
            BaseModel copy = (BaseModel)Activator.CreateInstance(t);
            copy.CopyPropertyValues(this, new List<string>());
            return copy;
        }

        /// <summary>
        /// Creates a new instance and copies all property values from this
        /// instance. Casts to the specified type. The type must be a type that
        /// this is or the cast will fail.
        /// </summary>
        public T Copy<T>() where T : BaseModel
        {
            return (T)Copy();
        }

        /// <summary>
        /// Copies all properties except those in the dontCopyPropertyNames
        /// list. Reference types will get copied by reference, so beware.
        /// </summary>
        public void CopyPropertyValues(object source, List<string> dontCopyPropertyNames)
        {
            PropertyInfo[] props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            /* The remaining properties are value types, so we can
             * copy them by value without worries. */
            foreach (PropertyInfo prop in props)
            {
                if (!dontCopyPropertyNames.Contains(prop.Name) && prop.CanRead && prop.CanWrite)
                {
                    object value = prop.GetValue(source, null);
                    prop.SetValue(this, value, null);
                }
            }
        }

        public Hashtable GetInvalidProperties()
        {
            return _propertyErrors;
        }

        public ArrayList GetInvalidPropertiesList()
        {
            ArrayList invalidPropertiesList = new ArrayList(_propertyErrors.Keys);
            return invalidPropertiesList;
        }

        public string GetValidationErrors()
        {
            IDataErrorInfo dataErrorInfo = (IDataErrorInfo)this;
            return dataErrorInfo.Error;
        }

        public void RemoveError([CallerMemberName]string propertyName = null)
        {
            if (_propertyErrors[propertyName] != null)
            {
                // Clear the error.
                _propertyErrors.Remove(propertyName);

                // Need to fire change event or the control may not hide the
                // error icon immediately.
                NotifyPropertyChanged(propertyName);
            }
        }

        public void SetError(string errorMessage, [CallerMemberName]string propertyName = null)
        {
            // Set the error.
            _propertyErrors[propertyName] = errorMessage;

            // Need to fire change event or the control may not show the updated
            // or new error immediately.
            NotifyPropertyChanged(propertyName);
        }

        public bool Validate()
        {
            // Get a list of properties on the object.
            PropertyInfo[] props = GetType().GetProperties();

            // Loop through the objects properties and set them equal to
            // themselves. This will trigger the validation logic in each property.
            for (int i = 0; i < props.Length; i++)
            {
                object value = props[i].GetValue(this, null);
                if (props[i].CanWrite) props[i].SetValue(this, value, null);
            }

            if (_propertyErrors.Count == 0) return true;
            else return false;
        }

        protected bool Changed<T>(T oldValue, T newValue)
        {
            return !EqualityComparer<T>.Default.Equals(oldValue, newValue);
        }

        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (LoadMode) return;
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Basic property setter implementation implementing INotifyPropertyChanged.
        /// </summary>
        /// <param name="backingField">
        /// Reference to the backing field. If the backing data store cannot be
        /// passed by ref, you need to set the value yourself and call NotifyPropertyChanged.
        /// </param>
        /// <param name="value">
        /// The value to set the backingField to.
        /// </param>
        /// <param name="propertyName">
        /// The property name to use with the NotifyPropertyChanged event.
        /// Retrieved automatically if called from the property setter.
        /// </param>
        protected void SetAndNotify<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (Changed<T>(backingField, value))
            {
                backingField = value;
                NotifyPropertyChanged(propertyName);
            }
        }

        protected bool ValidateDateRange(DateTime? value, DateTime lowerBound, DateTime upperBound, [CallerMemberName]string propertyName = null)
        {
            if (value == null) return true;
            else return ValidateDateRange(value.Value, lowerBound, upperBound, propertyName);
        }

        protected bool ValidateDateRange(DateTime value, DateTime lowerBound, DateTime upperBound, [CallerMemberName]string propertyName = null)
        {
            int upperBoundTest = value.CompareTo(upperBound);
            if (upperBoundTest > 0)
            {
                string errorText = String.Format("Date cannot be later than {0:d}.", upperBound);
                SetError(errorText, propertyName);
                return false;
            }
            int lowerBoundTest = value.CompareTo(lowerBound);
            if (lowerBoundTest < 0)
            {
                string errorText = String.Format("Date cannot be earlier than {0:d}.", lowerBound);
                SetError(errorText, propertyName);
                return false;
            }
            return true;
        }

        protected bool ValidateDependencyPopulated(object value, object dependentValue, string dependentDisplayName, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null)
            {
                // Valid
                return true;
            }
            else if (dependentValue == null)
            {
                // Invalid
                // Dependent value is null but value is populated.
                //string errorTemplate = "Must be null if {0} is null.";
                string errorTemplate = "A value must be entered in {0}.";
                string errorMessage = String.Format(errorTemplate, dependentDisplayName);
                SetError(errorMessage, propertyName);
                return false;
            }
            else
            {
                // Valid
                return true;
            }
        }

        protected bool ValidateGreaterThan(int value, int referenceNumber, [CallerMemberName]string propertyName = null)
        {
            if (value > referenceNumber)
            {
                // Valid value.
                return true;
            }
            else
            {
                // Invalid value.
                string errorMessage = String.Format("Must be greater than {0}.", referenceNumber);
                SetError(errorMessage, propertyName);
                return false;
            }
        }

        protected bool ValidateGreaterThan(short value, int referenceNumber, [CallerMemberName]string propertyName = null)
        {
            if (value > referenceNumber)
            {
                // Valid value.
                return true;
            }
            else
            {
                // Invalid value.
                string errorMessage = String.Format("Must be greater than {0}.", referenceNumber);
                SetError(errorMessage, propertyName);
                return false;
            }
        }

        protected bool ValidateGreaterThan(short? value, int referenceNumber, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidateGreaterThan(value.Value, referenceNumber, propertyName);
        }

        protected bool ValidateGreaterThan(double value, double referenceNumber, [CallerMemberName]string propertyName = null)
        {
            if (value > referenceNumber)
            {
                // Valid value.
                return true;
            }
            else
            {
                // Invalid value.
                string errorMessage = String.Format("Must be greater than {0}.", referenceNumber);
                SetError(errorMessage, propertyName);
                return false;
            }
        }

        protected bool ValidateGreaterThan(double? value, double referenceNumber, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidateGreaterThan((double)value, referenceNumber, propertyName);
        }

        protected bool ValidateGreaterThan(decimal value, decimal referenceNumber, [CallerMemberName]string propertyName = null)
        {
            if (value > referenceNumber)
            {
                // Valid value.
                return true;
            }
            else
            {
                // Invalid value.
                string errorMessage = $"Must be greater than {referenceNumber}.";
                SetError(errorMessage, propertyName);
                return false;
            }
        }

        protected bool ValidateGreaterThan(decimal? value, decimal referenceNumber, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidateGreaterThan((decimal)value, referenceNumber, propertyName);
        }

        protected bool ValidateGreaterThanOrEqualTo(int value, int referenceNumber, [CallerMemberName]string propertyName = null)
        {
            if (value >= referenceNumber)
            {
                // Valid value.
                return true;
            }
            else
            {
                // Invalid value.
                string errorMessage = $"Must be greater than or equal to {referenceNumber}.";
                SetError(errorMessage, propertyName);
                return false;
            }
        }

        protected bool ValidateGreaterThanOrEqualTo(short value, int referenceNumber, [CallerMemberName]string propertyName = null)
        {
            if (value >= referenceNumber)
            {
                // Valid value.
                return true;
            }
            else
            {
                // Invalid value.
                string errorMessage = $"Must be greater than or equal to {referenceNumber}.";
                SetError(errorMessage, propertyName);
                return false;
            }
        }

        protected bool ValidateGreaterThanOrEqualTo(short? value, int referenceNumber, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidateGreaterThanOrEqualTo(value.Value, referenceNumber, propertyName);
        }

        protected bool ValidateGreaterThanOrEqualTo(double value, double referenceNumber, [CallerMemberName]string propertyName = null)
        {
            if (value >= referenceNumber)
            {
                // Valid value.
                return true;
            }
            else
            {
                // Invalid value.
                string errorMessage = $"Must be greater than or equal to {referenceNumber}.";
                SetError(errorMessage, propertyName);
                return false;
            }
        }

        protected bool ValidateGreaterThanOrEqualTo(double? value, double referenceNumber, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidateGreaterThanOrEqualTo(value.Value, referenceNumber, propertyName);
        }

        protected bool ValidateGreaterThanOrEqualTo(decimal value, decimal referenceNumber, [CallerMemberName]string propertyName = null)
        {
            if (value >= referenceNumber)
            {
                // Valid value.
                return true;
            }
            else
            {
                // Invalid value.
                string errorMessage = $"Must be greater than or equal to {referenceNumber}.";
                SetError(errorMessage, propertyName);
                return false;
            }
        }

        protected bool ValidateGreaterThanOrEqualTo(decimal? value, decimal referenceNumber, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidateGreaterThanOrEqualTo(value.Value, referenceNumber, propertyName);
        }

        protected bool ValidateGreaterThanOrEqualTo(string value, double referenceNumber, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;

            if (double.TryParse(value, out double doubleValue))
            {
                return ValidateGreaterThanOrEqualTo(doubleValue, referenceNumber, propertyName);
            }
            else
            {
                string errorText = String.Format("Must be greater than or equal to {0}.", referenceNumber);
                SetError(errorText, propertyName);
                return false;
            }
        }

        protected bool ValidateGreaterThanOrEqualTo(string value, decimal referenceNumber, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;

            if (decimal.TryParse(value, out decimal decimalValue))
            {
                return ValidateGreaterThanOrEqualTo(decimalValue, referenceNumber, propertyName);
            }
            else
            {
                string errorText = $"Must be greater than or equal to {referenceNumber}.";
                SetError(errorText, propertyName);
                return false;
            }
        }

        protected bool ValidateLength(string value, int maxLength, [CallerMemberName]string propertyName = null)
        {
            return ValidateLength(value, 0, maxLength, propertyName);
        }

        protected bool ValidateLength(string value, int minLength, int maxLength, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;

            if (minLength == maxLength && value.Length != minLength)
            {
                // Invalid value.
                SetError($"Must be exactly {minLength} characters.", propertyName);
                return false;
            }
            if (value.Length > maxLength)
            {
                // Invalid value.
                SetError($"Cannot exceed {maxLength} characters.", propertyName);
                return false;
            }
            else
            {
                // Valid value.
                return true;
            }
        }

        protected bool ValidateNotNull(object value, [CallerMemberName]string propertyName = null)
        {
            // Return false if null.
            if (value == null)
            {
                // Invalid
                SetError("This item is required.", propertyName);
                return false;
            }
            else
            {
                return true;
            }
        }

        protected bool ValidateNotNullOrEmpty(string value, [CallerMemberName]string propertyName = null)
        {
            // Return false if null.
            if (String.IsNullOrEmpty(value))
            {
                // Invalid
                SetError("This item is required.", propertyName);
                return false;
            }
            else
            {
                return true;
            }
        }

        protected bool ValidateNotNullOrWhiteSpace(string value, [CallerMemberName]string propertyName = null)
        {
            // Return false if null.
            if (value == null || String.IsNullOrEmpty(value.Trim()))
            {
                // Invalid
                SetError("This item is required.", propertyName);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// I found that there are several ways to interpret precision and
        /// scale, so I asked Cindy Dalby and she checked how it is actually
        /// implemented in Oracle. In Oracle precision has nothing to do with
        /// significant digits.
        ///
        /// For precision = 8 and scale = 4 ----------------------------------
        /// 1234 ok 12345 no 12.1234 ok 1.12345 no 12345678 no 1234.1234 ok
        /// 9999.9999 ok --&gt; biggest number(8,4)
        /// -9999.9999 ok --&gt; smallest number(8,4) ----------------------------------
        /// </summary>
        protected bool ValidatePrecisionAndScale(double value, int precision, int scale, [CallerMemberName]string propertyName = null)
        {
            int digitsAllowedLeft = precision - scale;
            int digitsAllowedRigth = scale;

            // Convert to string.
            string valueString = value.ToString();

            // Get digits to the left and right of decimal.
            string leftDigits;
            string rightDigits;
            // Strip negative sign and split on the decimal.
            string[] chunks = valueString.TrimStart(new char[] { '-' }).Split('.');
            leftDigits = chunks[0];
            if (chunks.Length > 1)
                rightDigits = chunks[1]; // Will be null if no decimal;
            else
                rightDigits = String.Empty;

            // Validate value.
            if (leftDigits.Length > digitsAllowedLeft)
            {
                // Invalid value.
                string errorMessage = $"Cannot exceed {digitsAllowedLeft} digits to the left of the decimal.";
                SetError(errorMessage, propertyName);
                return false;
            }
            else if (rightDigits.Length > digitsAllowedRigth)
            {
                // Invalid value.
                string errorMessage = $"Cannot exceed {digitsAllowedRigth} digits to the right of the decimal.";
                SetError(errorMessage, propertyName);
                return false;
            }
            else
            {
                // Valid value.
                return true;
            }
        }

        protected bool ValidatePrecisionAndScale(double? value, int precision, int scale, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidatePrecisionAndScale((double)value, precision, scale, propertyName);
        }

        protected bool ValidatePrecisionAndScale(decimal value, int precision, int scale, [CallerMemberName]string propertyName = null)
        {
            int digitsAllowedLeft = precision - scale;
            int digitsAllowedRigth = scale;

            // Convert to string.
            string valueString = value.ToString();

            // Get digits to the left and right of decimal.
            string leftDigits;
            string rightDigits;
            // Strip negative sign and split on the decimal.
            string[] chunks = valueString.TrimStart(new char[] { '-' }).Split('.');
            leftDigits = chunks[0];
            if (chunks.Length > 1)
                rightDigits = chunks[1]; // Will be null if no decimal;
            else
                rightDigits = String.Empty;

            // Validate value.
            if (leftDigits.Length > digitsAllowedLeft)
            {
                // Invalid value.
                string errorMessage = $"Cannot exceed {digitsAllowedLeft} digits to the left of the decimal.";
                SetError(errorMessage, propertyName);
                return false;
            }
            else if (rightDigits.Length > digitsAllowedRigth)
            {
                // Invalid value.
                string errorMessage = $"Cannot exceed {digitsAllowedRigth} digits to the right of the decimal.";
                SetError(errorMessage, propertyName);
                return false;
            }
            else
            {
                // Valid value.
                return true;
            }
        }

        protected bool ValidatePrecisionAndScale(decimal? value, int precision, int scale, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (!value.HasValue) return true;
            else return ValidatePrecisionAndScale(value.Value, precision, scale, propertyName);
        }

        protected bool ValidateRange(int value, int lowerBound, int upperBound, [CallerMemberName]string propertyName = null)
        {
            if (value < lowerBound || value > upperBound)
            {
                // Invalid value.
                SetError($"Must be from {lowerBound} to {upperBound}.", propertyName);
                return false;
            }
            else
            {
                // Valid value.
                return true;
            }
        }

        protected bool ValidateRange(int? value, int lowerBound, int upperBound, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidateRange(propertyName, (int)value, lowerBound, upperBound);
        }

        protected bool ValidateRange(double value, double lowerBound, double upperBound, [CallerMemberName]string propertyName = null)
        {
            if (value < lowerBound || value > upperBound)
            {
                // Invalid value.
                string errorText = $"Must be from {lowerBound} to {upperBound}.";
                SetError(errorText, propertyName);
                return false;
            }
            else
            {
                // Valid value.
                return true;
            }
        }

        protected bool ValidateRange(double? value, double lowerBound, double upperBound, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidateRange((double)value, lowerBound, upperBound, propertyName);
        }

        protected bool ValidateRange(string propertyName, decimal value, decimal lowerBound, decimal upperBound)
        {
            if (value < lowerBound || value > upperBound)
            {
                // Invalid value.
                SetError($"Must be from {lowerBound} to {upperBound}.", propertyName);
                return false;
            }
            else
            {
                // Valid value.
                return true;
            }
        }

        protected bool ValidateRange(decimal? value, decimal lowerBound, decimal upperBound, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;
            else return ValidateRange(propertyName, (decimal)value, lowerBound, upperBound);
        }

        protected bool ValidateRange(string value, int lowerBound, int upperBound, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;

            if (int.TryParse(value, out int intValue))
            {
                return ValidateRange(propertyName, intValue, lowerBound, upperBound);
            }
            else
            {
                SetError($"Must be from {lowerBound} to {upperBound}.", propertyName);
                return false;
            }
        }

        protected bool ValidateRange(string value, double lowerBound, double upperBound, [CallerMemberName]string propertyName = null)
        {
            // Return true if null.
            if (value == null) return true;

            if (double.TryParse(value, out double doubleValue))
            {
                return ValidateRange(doubleValue, lowerBound, upperBound, propertyName);
            }
            else
            {
                SetError($"Must be from {lowerBound} to {upperBound}.", propertyName);
                return false;
            }
        }

        protected bool ValidateRegularExpressionMatch(string value, string regexToMatch, string errorMessage, [CallerMemberName]string propertyName = null)
        {
            Regex regex = new Regex(regexToMatch);
            bool ok = regex.IsMatch(value);
            if (!ok) SetError(errorMessage, propertyName);
            return ok;
        }

        protected class AttributeGetter
        {
            public AttributeGetter(Type type)
            {
                type = type ?? throw new ArgumentNullException("type");
                foreach (PropertyInfo propInfo in type.GetProperties())
                {
                    PropertyInfos.Add(propInfo.Name, propInfo);
                }
            }

            public Dictionary<string, PropertyInfo> PropertyInfos { get; } = new Dictionary<string, PropertyInfo>();

            public static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
            {
                object[] attributeObjects = memberInfo.GetCustomAttributes(typeof(T), true);
                if (attributeObjects.Length > 0)
                {
                    T attribute = (T)attributeObjects[0];
                    return attribute;
                }
                else
                {
                    return null;
                }
            }

            public static List<T> GetAttributes<T>(MemberInfo memberInfo) where T : Attribute
            {
                List<T> attributes = new List<T>();

                object[] attributeObjects = memberInfo.GetCustomAttributes(typeof(T), true);
                foreach (object attributeObject in attributeObjects)
                {
                    attributes.Add((T)attributeObject);
                }
                return attributes;
            }

            /// <summary>
            /// If the System.ComponentModel.DescriptionAttribute attribute is
            /// present, then it returns the Description value. Otherwise, it
            /// returns the null.
            /// </summary>
            public static string GetDescription(MemberInfo memberInfo)
            {
                if (memberInfo == null)
                {
                    return null;
                }
                DescriptionAttribute descriptionAttribute = GetAttribute<DescriptionAttribute>(memberInfo);
                if (descriptionAttribute == null)
                {
                    return null;
                }
                else
                {
                    return descriptionAttribute.Description;
                }
            }

            /// <summary>
            /// Uses the DisplayName attribute if present on the member,
            /// otherwise it returns the member name.
            /// </summary>
            public static string GetDisplayName(MemberInfo memberInfo)
            {
                if (memberInfo == null)
                {
                    return null;
                }
                DisplayNameAttribute displayNameAttribute = GetAttribute<DisplayNameAttribute>(memberInfo);
                if (displayNameAttribute == null)
                {
                    return memberInfo.Name;
                }
                else
                {
                    return displayNameAttribute.DisplayName;
                }
            }

            /// <summary>
            /// If the enum has Description attributes on the enum values, then
            /// that text is used rather than the usual ToString conversion.
            /// </summary>
            public static string GetEnumText(Enum e)
            {
                FieldInfo enumInfo = e.GetType().GetField(e.ToString());
                DescriptionAttribute[] enumAttributes =
                    (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (enumAttributes.Length > 0)
                {
                    return enumAttributes[0].Description;
                }
                else
                {
                    return e.ToString();
                }
            }

            /// <summary>
            /// If the property is flagged with the
            /// System.ComponentModel.ReadOnlyAttribute and the IsReadOnly
            /// attribute is true, returns true. If the property does not have
            /// the ReadOnlyAttribute or IsReadOnly is false, then this method
            /// returns false.
            /// </summary>
            /// <remarks>
            /// Be aware that the System.ComponentModel.ReadOnlyAttribute has
            /// effects on Forms Designer behavior and databinding. For
            /// databinding, properties with the ReadOnlyAttribute (set to true)
            /// will not be set by data-bound controls.
            /// </remarks>
            public static bool GetReadOnly(PropertyInfo propInfo)
            {
                if (propInfo == null)
                {
                    return false;
                }
                ReadOnlyAttribute readOnlyAttribute = GetAttribute<ReadOnlyAttribute>(propInfo);
                if (readOnlyAttribute == null)
                {
                    return false;
                }
                else
                {
                    return readOnlyAttribute.IsReadOnly;
                }
            }

            public T GetAttribute<T>(string propertyName) where T : Attribute
            {
                if (PropertyInfos.ContainsKey(propertyName))
                {
                    PropertyInfo propInfo = PropertyInfos[propertyName];
                    T attribute = GetAttribute<T>(propInfo);
                    return attribute;
                }
                else
                {
                    // Did not find the property.
                    return null;
                }
            }

            /// <summary>
            /// Checks for a System.ComponentModel.DescriptionAttribute on the
            /// specified property. If present, the Description text is
            /// returned. Otherwise, null is returned.
            /// </summary>
            public string GetDescription(string propertyName)
            {
                if (propertyName == null) return null;
                DescriptionAttribute descriptionAttribute = GetAttribute<DescriptionAttribute>(propertyName);
                if (descriptionAttribute == null) return null;
                else return descriptionAttribute.Description;
            }

            /// <summary>
            /// Checks for a System.ComponentModel.DisplayNameAttribute on the
            /// specified property. If present, the DisplayName text is
            /// returned, otherwise the property name is returned.
            /// </summary>
            public string GetDisplayName(string propertyName)
            {
                if (propertyName == null) return null;
                DisplayNameAttribute displayNameAttribute = GetAttribute<DisplayNameAttribute>(propertyName);
                if (displayNameAttribute == null) return propertyName;
                else return displayNameAttribute.DisplayName;
            }

            /// <summary>
            /// Returns the PropertyInfo for the given property name. If the
            /// property does not exist on the type, returns null. Use
            /// PropertyInfos[propertyName] if you would rather get an exception
            /// when the property does not exist.
            /// </summary>
            public PropertyInfo TryGetPropertyInfo(string propertyName)
            {
                if (PropertyInfos.ContainsKey(propertyName))
                {
                    return PropertyInfos[propertyName];
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Returns the type of the property, or null if the property does
            /// not exist.
            /// </summary>
            public Type TryGetPropertyType(string propertyName)
            {
                PropertyInfo propInfo = TryGetPropertyInfo(propertyName);
                if (propInfo == null) return null;
                else return propInfo.PropertyType;
            }
        }
    }
}