using MonoTouch.UIKit;
using System.Text.RegularExpressions;

namespace BetterSalesman.iOS
{
    public static class UITextFieldValidatorExtensions
    {
        public static bool IsNotEmpty(this UITextField input)
        {
            if (string.IsNullOrEmpty(input.Text))
            {   
                return false;
            }
            
            return true;
        }

		public static bool IsValidEmail(this UITextField input)
		{
			string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" 
				+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" 
				+ @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

			return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(input.Text);
		}
    }
}

