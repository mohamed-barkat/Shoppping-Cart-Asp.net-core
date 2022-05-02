using System.ComponentModel.DataAnnotations;

namespace Shop.UI.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string emailDomain;

        public ValidEmailDomainAttribute(string EmailDomain)
        {
            emailDomain = EmailDomain;
        }
        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split("@");
            return strings[1].ToUpper() == emailDomain.ToUpper();

        }
    }
}
