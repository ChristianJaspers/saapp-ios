using System;
using System.Collections.Generic;

namespace XValidator
{
    public class XFieldValidate
    {
        /// <summary>
        /// Gets or sets the name of the field displayed as referenced in error messages
        /// </summary>
        /// <value>The name.</value>
        public string Name = "Field";

        /// <summary>
        /// List of validator attached to input
        /// </summary>
        /// <value>The validators.</value>
        public XValidatorBase[] Validators = {};
        
        /// <summary>
        /// List of errors
        /// </summary>
        /// <value>The errors.</value>
        public List<string> Errors = new List<string>();
        
        /// <summary>
        /// Validate input
        /// </summary>
        public bool Validate()
        {
            foreach (var validator in Validators)
            {
                if (!validator.IsValid(Value()))
                {
                    Errors.Add(validator.ErrorMessage(Name));
                }
            }
            
            return Errors.Count > 0;
        }
        
        /// <summary>
        /// The field view object.
        /// </summary>
        public object FieldView;
        
        /// <summary>
        /// Value of the field
        /// </summary>
        public string Value()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Redraw view when error appears during validaton
        /// </summary>
        public void ViewStateError()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Redraw view when no errors during validation
        /// </summary>
        public void ViewStateNormal()
        {
            throw new NotImplementedException();
        }
    }
}

