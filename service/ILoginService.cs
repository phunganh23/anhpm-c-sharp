namespace danh_gia_csharp.service
{
    public interface ILoginService
    {
        Task<string> LoginAsync(string username, string password);
    }
}
