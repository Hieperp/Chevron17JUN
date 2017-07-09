using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TotalModel.Helpers
{
    public class RegexValidationRule : ValidationRule
    {

        private string _regex;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RegexValidationRule(string propertyName, string description, string regex)
            : base(propertyName, description)
        {
            _regex = regex;
        }


        public override bool ValidateRule(NotifyValidationRule domainObject)
        {
            PropertyInfo pi = domainObject.GetType().GetProperty(this.PropertyName);
            Match m = Regex.Match(pi.GetValue(domainObject, null).ToString(), _regex);
            if (m.Success)
            {
                return true;
            }
            else return false;
        }
    }
}
