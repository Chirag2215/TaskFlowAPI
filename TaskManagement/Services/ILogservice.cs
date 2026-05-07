namespace Ecommerce.Services
{
    public interface ILogservice
    {
        Task AddLog(string action, int userId);
    }
}
