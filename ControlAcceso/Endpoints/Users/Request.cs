namespace ControlAcceso.Endpoints.Users
{
    public class Request
    {
        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? SecondName { get; set; }

        public string? FirstSurname { get; set; }

        public string? SecondSurname { get; set; }

        public string? Password { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? RoleId { get; set; }

    }
}