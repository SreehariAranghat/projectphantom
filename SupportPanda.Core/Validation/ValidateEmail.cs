using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class ValidateEmail : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                return true;
            }
            else
                return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}
