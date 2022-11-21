using System;

namespace databatdongsan.helper
{
    public static class ObjectHelper
    {
        public static void CopyPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent, null), null);
                        break;
                    }
                }
            }
        }

        public static void SetPropertyValue(this object source, string property, object value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            var sourceType = source.GetType();
            var sourceProperties = sourceType.GetProperty(property);
            if (sourceProperties != null)
            {
                sourceProperties.SetValue(source, Convert.ChangeType(value, sourceProperties.PropertyType), null);
            }
        }
    }
}
