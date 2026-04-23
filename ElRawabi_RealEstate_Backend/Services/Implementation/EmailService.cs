using System.Net;
using System.Net.Mail;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetToken)
        {
            var frontendUrl = _config["Frontend:BaseUrl"] ?? "http://localhost:5501";
            var resetLink = $"{frontendUrl}/ResetPassword.html?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(toEmail)}";

            var smtpHost = _config["Smtp:Host"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_config["Smtp:Port"] ?? "587");
            var smtpUser = _config["Smtp:Username"] ?? "";
            var smtpPass = _config["Smtp:Password"] ?? "";
            var fromName = _config["Smtp:FromName"] ?? "الروابي للعقارات";

            var body = $@"
<!DOCTYPE html>
<html dir='rtl' lang='ar'>
<head><meta charset='UTF-8'/></head>
<body style='margin:0;padding:0;background:#f4f6fb;font-family:Tahoma,Arial,sans-serif;direction:rtl'>
  <table width='100%' cellpadding='0' cellspacing='0' style='background:#f4f6fb;padding:40px 0'>
    <tr><td align='center'>
      <table width='520' cellpadding='0' cellspacing='0' style='background:#fff;border-radius:18px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,.08)'>
        
        <!-- Header -->
        <tr>
          <td style='background:linear-gradient(135deg,#0D2142,#1a3a6e);padding:36px 40px;text-align:center'>
            <div style='font-size:22px;font-weight:800;color:#fff;letter-spacing:-0.5px'>الروابي للعقارات</div>
            <div style='font-size:13px;color:rgba(255,255,255,.6);margin-top:4px'>نظام إدارة العقارات</div>
          </td>
        </tr>
 
        <!-- Body -->
        <tr>
          <td style='padding:40px'>
            <div style='font-size:24px;font-weight:800;color:#0D2142;margin-bottom:12px'>إعادة تعيين كلمة المرور 🔑</div>
            <p style='color:#555;font-size:15px;line-height:1.8;margin:0 0 28px'>
              تلقّينا طلباً لإعادة تعيين كلمة المرور الخاصة بحسابك.<br/>
              اضغط على الزر أدناه لإتمام العملية. الرابط صالح لمدة <strong>15 دقيقة</strong> فقط.
            </p>
            <div style='text-align:center;margin-bottom:28px'>
              <a href='{resetLink}' style='display:inline-block;background:linear-gradient(135deg,#4e8df5,#3a7de4);color:#fff;text-decoration:none;padding:15px 40px;border-radius:12px;font-size:16px;font-weight:700;letter-spacing:0.3px'>
                إعادة تعيين كلمة المرور
              </a>
            </div>
            <p style='color:#999;font-size:13px;line-height:1.7;margin:0'>
              إذا لم تطلب إعادة تعيين كلمة المرور، يمكنك تجاهل هذا البريد بأمان.<br/>
              لن يتم إجراء أي تغييرات على حسابك.
            </p>
          </td>
        </tr>
 
        <!-- Footer -->
        <tr>
          <td style='background:#f8faff;padding:20px 40px;text-align:center;border-top:1px solid #eef0f5'>
            <div style='color:#aaa;font-size:12px'>© 2025 الروابي للعقارات — جميع الحقوق محفوظة</div>
          </td>
        </tr>
 
      </table>
    </td></tr>
  </table>
</body>
</html>";

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var mail = new MailMessage
            {
                From = new MailAddress(smtpUser, fromName),
                Subject = "إعادة تعيين كلمة المرور — الروابي للعقارات",
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            await client.SendMailAsync(mail);
        }
    }
}