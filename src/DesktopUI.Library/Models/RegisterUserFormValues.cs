using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DesktopUI.Library.Validators;

namespace DesktopUI.Library.Models
{
    public class RegisterUserFormValues : IDataErrorInfo
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        private static readonly Dictionary<string, bool> ValidatedProperties = new Dictionary<string, bool>
        {
            {"Username", false},
            {"DisplayName", false},
            {"Email", false},
            {"Password", false}
        };

        public bool IsValid => ValidatedProperties.All(x => x.Value);

        public string Error => this[string.Empty];

        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;

                ValidatedProperties[columnName] = columnName switch
                {
                    nameof(Username) => Username.IsValidUsername(ref error),
                    nameof(DisplayName) => DisplayName.IsValidDisplayName(ref error),
                    nameof(Email) => Email.IsValidEmail(ref error),
                    nameof(Password) => Password.IsValidPassword(ref error),
                    _ => true
                };

                return error;
            }
        }
    }
}