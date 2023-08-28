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
        public string Trim(string input, int length)
        {
            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return input.Substring(0, length);
            }
        }
    }
}
