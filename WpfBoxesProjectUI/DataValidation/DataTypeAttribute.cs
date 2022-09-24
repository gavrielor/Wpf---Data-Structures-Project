using System;

namespace WpfBoxesProjectUI.DataValidation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataTypeAttribute : Attribute
    {
        public DataType DataType { get; set; }

        public DataTypeAttribute(DataType dataType) => DataType = dataType;
    }
}
