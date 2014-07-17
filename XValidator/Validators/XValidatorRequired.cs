namespace XValidator
{
    public class XValidatorRequired : XValidatorBase
    {
        public override bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}

