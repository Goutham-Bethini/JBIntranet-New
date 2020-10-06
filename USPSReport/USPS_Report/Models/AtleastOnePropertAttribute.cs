using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace USPS_Report.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AtleastOnePropertAttribute : ValidationAttribute
    {
        // Have to override IsValid
        public override bool IsValid(object value)
        {
            //  Need to use reflection to get properties of "value"...
            var typeInfo = value.GetType();

            var propertyInfo = typeInfo.GetProperties();

            foreach (var property in propertyInfo)
            {
                if (null != property.GetValue(value, null))
                {
                    // We've found a property with a value
                    return true;
                }
            }

            // All properties were null.
            return false;
        }
    }
}