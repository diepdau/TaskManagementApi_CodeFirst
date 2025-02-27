namespace TaskManagement.Interfaces
{
    public class IdentityUser
    {
        public IdentityUser(string userName) { }
        public virtual string Email { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Id { get; set; }
    }
}
