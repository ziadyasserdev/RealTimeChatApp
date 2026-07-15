using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Contracts.ExternalServices
{
    public interface IEmailTemplateBuilder
    {
        string BuildPasswordResetTemplate(
            string displayName,
            string email,
            string token);

        string BuildWelcomeTemplate(
            string displayName);

        string BuildPasswordChangedTemplate(
            string displayName);

        string BuildEmailConfirmationTemplate(
            string displayName,
            string confirmationLink);
    }
}
