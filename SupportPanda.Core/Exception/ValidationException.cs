using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class ValidationException : ApplicationException
    {
        List<ValidationResult> result = new List<ValidationResult>();

        public ValidationException() : base() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception excption) : base(message, excption) { }
        public ValidationException(string message, IList<ValidationResult> valiationErrors) : base(message)
        {

            this.result = valiationErrors.ToList();
        }

        public List<ValidationResult> ValidationResults
        {
            get
            {
                return result;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var valResult in result)
                builder.Append(valResult.MemberNames.First() + ":" + valResult.ErrorMessage + "\n");

            return builder.ToString();
        }
    }
}
