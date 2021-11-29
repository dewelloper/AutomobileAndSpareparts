using Mvc.Mailer;
using Mvc4WebRole.Mailers.Models;

namespace Mvc4WebRole.Mailers
{
    public interface IPasswordResetMailer
    {
        MvcMailMessage PasswordReset(MailerModel model);
    }
}
