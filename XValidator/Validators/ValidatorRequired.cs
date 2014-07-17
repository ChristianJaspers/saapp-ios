namespace XValidator
{
    public class ValidatorRequired : ValidatorBase
    {
        public override bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}

