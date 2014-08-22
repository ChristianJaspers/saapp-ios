namespace XValidator
{
    public class XValidatorLengthMinimum : XValidatorBase
    {
        public new string Message = "Field {0} must contain at least {1} characters.";
        
        int min;
        
        public XValidatorLengthMinimum(int minimumLenght)
        {
            min = minimumLenght;
        }
        
        public override string ErrorMessage(string fieldName)
        {
            return string.Format(Message, fieldName, min);
        }
        
        public override bool IsValid(string value)
        {
            return value.Length >= min;
        }
    }
}

