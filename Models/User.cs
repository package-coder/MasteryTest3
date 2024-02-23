namespace MasteryTest3.Models
{
    public class User
    {
        public int Id { get; set; }
        public UserRole role {get; set;}
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }


        public User() { }

        public User(int id)
        {
            Id = id;
        }
    }
}
