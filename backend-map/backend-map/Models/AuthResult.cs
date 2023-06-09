namespace backend_map.Models
{
    public class AuthResult
    {
        public string? Token { get; set; }
        public bool? IsSucceeded { get; set; }
        public List<string>? Message { get; set; }
    }
}
