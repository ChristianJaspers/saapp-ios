namespace XValidator
{
    public class XValidatorLengthMaximum : XValidatorBase
    {
        public new string Message = "Field {0} must contain not more than {1} characters.";
        
        int max;        
        
        public XValidatorLengthMaximum(int maximalLenght)
        {
            max = maximalLenght;
        }
        
        public override string ErrorMessage(string fieldName)
        {
            return string.Format(Message, fieldName, max);
        }
        
        public override bool IsValid(string value)
        {
            return max == -1 || value.Length < max;
        }
    }
}

