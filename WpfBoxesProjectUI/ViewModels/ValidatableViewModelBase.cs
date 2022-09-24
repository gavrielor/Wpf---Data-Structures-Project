using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WpfBoxesProjectUI.DataValidation;

namespace WpfBoxesProjectUI.ViewModels
{
    public class ValidatableViewModelBase : ViewModelBase, IDataErrorInfo
    {
        public string Error => string.Empty;
        public string this[string propName]
        {
            get
            {
                var propAttribute = Attribute.GetCustomAttribute(GetType().GetProperty(propName), typeof(DataTypeAttribute)) as DataTypeAttribute;
                if (propAttribute == null) throw new ArgumentException("Property without DataTypeAttribute has called for validate, implement DataTypeAttribute in order to inherit from ValidatableViewModelBase class");

                string propValue = GetType().GetProperty(propName).GetValue(this) as string;
                string result = string.Empty;
                switch (propAttribute.DataType)
                {
                    case DataType.PositiveNumberOrZero:
                        result = PositiveNumberOrZeroValidate(propValue);
                        break;
                    case DataType.PositiveNumber:
                        result = PositiveNumberValidate(propValue);
                        break;
                    case DataType.PositiveDecimalNumber:
                        result = PositiveDecimalNumberValidate(propValue);
                        break;
                    default:
                        break;
                }

                ErrorsCollection[propName] = result;
                RaisePropertyChanged(nameof(ErrorsCollection));

                GetType().GetProperties().Where(p => p.PropertyType == typeof(RelayCommand)).ToList().ForEach((p) =>
                {
                    var rc = p.GetValue(this) as RelayCommand;
                    rc.RaiseCanExecuteChanged();
                });

                return result;
            }
        }

        public Dictionary<string, string> ErrorsCollection { get; set; } = new Dictionary<string, string>();
        public bool HasError => ErrorsCollection.Any(kv => kv.Value != string.Empty);

        private string PositiveNumberOrZeroValidate(string propValue)
        {
            if (int.TryParse(propValue, out int result) && result >= 0) return string.Empty;
            return "Enter a non-negative number";
        }

        private static string PositiveNumberValidate(string propValue)
        {
            if (int.TryParse(propValue, out int result) && result > 0) return string.Empty;
            return "Enter a positive number";
        }

        private static string PositiveDecimalNumberValidate(string propValue)
        {
            if (double.TryParse(propValue, out double result) && result > 0) return string.Empty;
            return "Enter a positive decimal number";
        }
    }
}
