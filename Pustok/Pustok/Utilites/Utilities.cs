using Pustok.Database.Models;
using Pustok.ViewModels.Client;

namespace Pustok.Utilites
{
    public class Utilities
    {
        public string[] GetEmails(string email)
        {
            string[] emails = email
                .Trim(' ')
                .Split(',');
            return emails;
        }
    }
}
