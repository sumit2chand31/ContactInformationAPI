using System.ComponentModel.DataAnnotations;

namespace ContactInformationAPI.Modal
{
    public class ConatctModal
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "PLease Enter First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "PLease Enter Last Name")]
        public string? LastName { get; set; }

        [Required (ErrorMessage = "PLease Enter Email")]
        [EmailAddress(ErrorMessage ="Enter vaild Email")]
        public string? Email { get; set; }
    }

    public class ConatctModalVM
    {
        [Required(ErrorMessage = "PLease Enter First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "PLease Enter Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "PLease Enter Email")]
        [EmailAddress(ErrorMessage = "Enter vaild Email")]
        public string? Email { get; set; }
    }
}
