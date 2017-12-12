

namespace SupportPanda.Core
{
    public class RegistrationRequest
    {
        [ValidateName]
        public string FirstName     { get; set; }

        [ValidateName]
        public string LastName      { get; set; }

        [ValidateEmail]
        public string Email    { get; set; }

        [ValidateClientName]
        public string Client   { get; set; }

        [ValiatePassword]
        public string Password { get; set; }
    }
}