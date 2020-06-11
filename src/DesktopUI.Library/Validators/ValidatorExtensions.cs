using System.Text.RegularExpressions;

namespace DesktopUI.Library.Validators
{
    public static class ValidatorExtensions
    {
        public static bool IsValidPassword(this string password, ref string errorMessage)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasMinChars = new Regex(@".{6,}");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "The field is required";
                return false;
            }
            else if (!hasLowerChar.IsMatch(password))
            {
                errorMessage = "Password should contain at least one lower case letter.";
                return false;
            }
            else if (!hasUpperChar.IsMatch(password))
            {
                errorMessage = "Password should contain at least one upper case letter.";
                return false;
            }
            else if (!hasMinChars.IsMatch(password))
            {
                errorMessage = "Password should not be lesser than 6 characters.";
                return false;
            }
            else if (!hasNumber.IsMatch(password))
            {
                errorMessage = "Password should contain at least one numeric value.";
                return false;
            }
            else if (!hasSymbols.IsMatch(password))
            {
                errorMessage = "Password should contain at least one special case character.";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsValidEmail(this string email, ref string errorMessage)
        {
            var isEmail = new Regex(
                @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");

            if (string.IsNullOrWhiteSpace(email))
            {
                errorMessage = "The field is required";
                return false;
            }
            else if (!isEmail.IsMatch(email))
            {
                errorMessage = "Please enter a valid email address.";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsValidUsername(this string username, ref string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                errorMessage = "The field is required";
                return false;
            }
            else if (username.Length > 256)
            {
                errorMessage = "The maximum length may be 256 characters";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsValidDisplayName(this string displayName, ref string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                errorMessage = "The field is required";
                return false;
            }
            else if (displayName.Length > 50)
            {
                errorMessage = "The maximum length may be 50 characters";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}