namespace BankingApp.Core.Application.CustomEntities
{
    public class Response<T>
    {
        public T? Data { get; set; }

        public bool Success { get; set; }

        public string? Error { get; set; }
    }
}
