using RealTimeChatApp.Application.Contracts.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.ExternalServices
{
    public sealed class EmailTemplateBuilder : IEmailTemplateBuilder
    {
        public string BuildPasswordResetTemplate(
            string displayName,
            string email,
            string token)
        {
            return $$"""
<!DOCTYPE html>
<html>

<head>
    <meta charset="UTF-8">
</head>

<body style="font-family:Arial,sans-serif;background:#f8f9fa;padding:30px;">

    <div style="
        max-width:600px;
        margin:auto;
        background:white;
        border-radius:10px;
        padding:30px;
        border:1px solid #e5e5e5;">

        <h2 style="color:#2563eb;">
            Reset Your Password
        </h2>

        <p>
            Hello <strong>{{displayName}}</strong>,
        </p>

        <p>
            We received a request to reset your password.
        </p>

        <p>
            <strong>Email:</strong>
            {{email}}
        </p>

        <p>
            Copy the following token and use it inside the
            <strong>Reset Password</strong>
            endpoint in Swagger.
        </p>

        <div style="
            background:#f3f4f6;
            padding:15px;
            border-radius:8px;
            font-family:Consolas;
            word-break:break-all;">

{{token}}

        </div>

        <br/>

        <p style="color:#666;">
            If you didn't request a password reset,
            you can safely ignore this email.
        </p>

        <hr/>

        <small style="color:#888;">
            RealTimeChatApp Team
        </small>

    </div>

</body>

</html>
""";
        }

        public string BuildWelcomeTemplate(
            string displayName)
        {
            return $$"""
<h2>Welcome {{displayName}} 🎉</h2>

<p>
Thank you for joining RealTimeChatApp.
</p>
""";
        }

        public string BuildPasswordChangedTemplate(
            string displayName)
        {
            return $$"""
<h2>Password Changed</h2>

<p>
Hello <strong>{{displayName}}</strong>,
</p>

<p>
Your password has been changed successfully.
If this wasn't you, please contact support immediately.
</p>
""";
        }

        public string BuildEmailConfirmationTemplate(
            string displayName,
            string confirmationLink)
        {
            return $$"""
<h2>Email Confirmation</h2>

<p>
Hello <strong>{{displayName}}</strong>,
</p>

<p>
Please confirm your email by clicking the link below.
</p>

<p>

<a href="{{confirmationLink}}">
Confirm Email
</a>

</p>
""";
        }
    }
}
