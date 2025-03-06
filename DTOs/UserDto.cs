namespace TaskManagementApi.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
