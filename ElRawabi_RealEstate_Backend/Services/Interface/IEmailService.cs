namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string resetToken);
    }
}
