using System;

namespace XValidator
{
    public class ValidatorBase : IValidator
    {
        #region IValidator implementation

        public string Message = "Field {0} is required.";

        public override string ErrorMessage(string fieldName)
        {
            return string.Format(Message, fieldName);
        }

        public override bool IsValid(string value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

