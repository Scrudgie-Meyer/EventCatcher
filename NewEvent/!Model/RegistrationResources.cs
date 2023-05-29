
using System.Net;
using System.Net.Mail;

namespace NewEvent.Support
{
    public static class RegistrationResources
    {
        public static  bool IsEmailValid(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


    }
    
}
