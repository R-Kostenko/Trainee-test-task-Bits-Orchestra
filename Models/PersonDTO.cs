using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Trainee_Test.Resources;

namespace Trainee_Test.Models;

public class PersonDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Date of birth is required.")]
    [DisplayName("Date of birth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Marital status is required.")]
    [DisplayName("Marital status")]
    public MaritalStatus MaritalStatus { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid phone number.")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "Salary is required.")]
    [Range(0, 1000000.00, ErrorMessage = "Salary must be between 0 and 1 000 000.")]
    public decimal Salary { get; set; }
}
