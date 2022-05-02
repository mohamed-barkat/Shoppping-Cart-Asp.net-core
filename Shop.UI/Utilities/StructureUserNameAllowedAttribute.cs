using System.ComponentModel.DataAnnotations;

namespace Shop.UI.Utilities
{
    public class StructureUserNameAllowedAttribute: ValidationAttribute
    {

        private readonly string[] _usernaem;

        public StructureUserNameAllowedAttribute(string[] UserName)
        {
            _usernaem = UserName;
        }


        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            var valuem = value.ToString();

            string res = valuem.Substring(0, 1);

       

           for(int i=0; i<_usernaem.Length; i++)
            {
                if (res == _usernaem[i])
                    return false;
                break;

            }
            return true;
        }
        

    }
}
