namespace TyperV1API.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public virtual ImageDTO? Image { get; set; }
    }
}
