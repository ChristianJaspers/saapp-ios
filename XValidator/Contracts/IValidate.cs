using System.Collections.Generic;

namespace XValidator
{
    public interface IValidate
    {
        /// <summary>
        /// Gets or sets the name of the field displayed as referenced in error messages
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
        
        /// <summary>
        /// Validate input
        /// </summary>
        bool Validate();
        
        /// <summary>
        /// List of errors
        /// </summary>
        /// <value>The errors.</value>
        List<string> Errors { get; set; }
        
        /// <summary>
        /// List of validator attached to input
        /// </summary>
        /// <value>The validators.</value>
        IValidator[] Validators { get; set; }
        
        /// <summary>
        /// Redraw view when error appears during validaton
        /// </summary>
        void ViewStateError();
        
        /// <summary>
        /// Redraw view when no errors during validation
        /// </summary>
        void ViewStateNormal();
    }
}

