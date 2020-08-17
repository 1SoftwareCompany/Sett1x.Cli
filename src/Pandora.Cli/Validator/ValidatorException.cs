using System;

namespace Pandora.Cli.Core.Validator
{
    public class ValidatorException : Exception
    {
        public ValidatorException(string message) : base(message)
        {

        }

        public ValidatorException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
