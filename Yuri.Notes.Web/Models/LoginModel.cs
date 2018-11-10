using System.ComponentModel.DataAnnotations;

namespace Yuri.Notes.Web.Model
{
    public class LoginModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}