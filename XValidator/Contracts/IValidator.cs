namespace XValidator
{
	public interface IValidator
	{
        string ErrorMessage(string fieldName);
        bool IsValid(string value);
	}

}

