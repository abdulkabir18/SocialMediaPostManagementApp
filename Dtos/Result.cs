namespace SocialMediaPostManager.Dtos
{
    public class Result<T>
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}