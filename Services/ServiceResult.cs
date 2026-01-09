namespace csharp_groep31.Services
{
    public class ServiceResult
    {
        public bool Success { get; set; } = true;
        public List<string> Messages { get; set; } = new();

        public static ServiceResult Ok(params string[] messages)
            => new ServiceResult { Success = true, Messages = messages.ToList() };

        public static ServiceResult Fail(params string[] messages)
            => new ServiceResult { Success = false, Messages = messages.ToList() };
    }
}
