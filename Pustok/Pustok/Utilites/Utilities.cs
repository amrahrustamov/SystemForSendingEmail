using Pustok.Database.Models;
using Pustok.ViewModels.Client;

namespace Pustok.Utilites
{
    public class Utilities
    {
        public Email GetDefaultValue()
        {
            Email emailInstance = null; // Replace this with your actual logic to create an Email instance

            var defaultEmail = new Email("Default Title", "Default Content", "default@example.com", DateTime.Now);

            var email = emailInstance ?? defaultEmail;
            return email;
        }
    }
}
