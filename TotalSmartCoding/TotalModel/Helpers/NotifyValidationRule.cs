using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TotalModel.Helpers
{
    public abstract class NotifyValidationRule
    {
        //http://stackoverflow.com/questions/88541/business-objects-validation-and-exceptions
        //http://www.codeproject.com/Articles/14178/Delegates-and-Business-Objects


        private List<ValidationRule> validationRule;

        /// <summary>
        /// Gets a value indicating whether or not this domain object is valid. 
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                return this.Error == null;
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this domain object. The default is an empty string ("").
        /// </summary>
        public virtual string Error
        {
            get
            {
                string result = this[string.Empty];
                if (result != null && result.Trim().Length == 0)
                {
                    result = null;
                }
                return result;
            }
        }


        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public virtual string this[string propertyName]
        {
            get
            {
                string result = string.Empty;

                propertyName = CleanString(propertyName);

                foreach (ValidationRule rule in GetBrokenRules(propertyName))
                {
                    if (propertyName == string.Empty || rule.PropertyName == propertyName)
                    {
                        result += rule.Description;
                        result += Environment.NewLine;
                    }
                }
                result = result.Trim();
                if (result.Length == 0)
                {
                    result = null;
                }

                return result;
            }
        }

        public virtual void PickupErrorList(string propertyName, ExceptionTable exceptionTable)
        {
            propertyName = CleanString(propertyName);

            foreach (ValidationRule rule in GetBrokenRules(propertyName))
            {
                if (propertyName == string.Empty || rule.PropertyName == propertyName)
                    exceptionTable.AddException(new string[] { "Input not correct. Please make sure all required fields are filled out correctly", rule.Description });
            }
        }

        /// <summary>
        /// Validates all rules on this domain object, returning a list of the broken rules.
        /// </summary>
        /// <returns>A read-only collection of rules that have been broken.</returns>
        public virtual ReadOnlyCollection<ValidationRule> GetBrokenRules()
        {
            return GetBrokenRules(string.Empty);
        }

        /// <summary>
        /// Validates all rules on this domain object for a given property, returning a list of the broken rules.
        /// </summary>
        /// <param name="property">The name of the property to check for. If null or empty, all rules will be checked.</param>
        /// <returns>A read-only collection of rules that have been broken.</returns>
        public virtual ReadOnlyCollection<ValidationRule> GetBrokenRules(string property)
        {
            property = CleanString(property);

            // If we haven't yet created the rules, create them now.
            if (validationRule == null)
            {
                validationRule = new List<ValidationRule>();
                validationRule.AddRange(this.CreateRules());
            }
            List<ValidationRule> broken = new List<ValidationRule>();


            foreach (ValidationRule rule in this.validationRule)
            {
                // Ensure we only validate a rule 
                if (rule.PropertyName == property || property == string.Empty)
                {
                    bool isRuleBroken = !rule.ValidateRule(this);
                    Debug.WriteLine(DateTime.Now.ToLongTimeString() + ": Validating the rule: '" + rule.ToString() + "' on object '" + this.ToString() + "'. Result = " + ((isRuleBroken == false) ? "Valid" : "Broken"));
                    if (isRuleBroken)
                    {
                        broken.Add(rule);
                    }
                }
            }

            return broken.AsReadOnly();
        }


        /// <summary>
        /// Override this method to create your own rules to validate this business object. These rules must all be met before 
        /// the business object is considered valid enough to save to the data store.
        /// </summary>
        /// <returns>A collection of rules to add for this business object.</returns>
        protected virtual List<ValidationRule> CreateRules()
        {
            return new List<ValidationRule>();
        }

        /// <summary>
        /// Cleans a string by ensuring it isn't null and trimming it.
        /// </summary>
        /// <param name="s">The string to clean.</param>
        protected string CleanString(string s)
        {
            return (s ?? string.Empty).Trim();
        }
    }
}
