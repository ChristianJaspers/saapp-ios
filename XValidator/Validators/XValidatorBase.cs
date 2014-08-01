using System;

namespace XValidator
{
    public abstract class XValidatorBase
    {
        public string Message = "Field {0} is required.";

        public virtual string ErrorMessage(string fieldName)
        {
            return string.Format(Message, fieldName);
        }

        public virtual bool IsValid(string value)
        {
            throw new NotImplementedException();
        }
    }
}

