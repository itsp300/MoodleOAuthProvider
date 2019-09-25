using BCrypt;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodleOAuthProvider
{
    [Table("mdl_user", Schema = "moodle")]
    public class MoodleUser
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; }

        public string FullName {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public bool VerifyPassword(string input)
        {
            return BCrypt.Net.BCrypt.Verify(input, Password);
        }

        public override string ToString()
        {
            return $"MoodleUser(Id={Id},Username={Username}, FirstName={FirstName}, LastName={LastName}, Email={Email}, Hashed Pass = {Password})";
        }
    }
}