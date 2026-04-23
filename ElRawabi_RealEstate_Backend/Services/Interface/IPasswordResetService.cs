namespace ElRawabi_RealEstate_Backend.Services.Interface
{
    public interface IPasswordResetService
    {
        Task<bool> SendResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
