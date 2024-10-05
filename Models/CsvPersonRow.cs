using System.ComponentModel.DataAnnotations;

namespace Trainee_Test.Models;

public class PersonCsvRow
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public bool Married { get; set; }

    [Required]
    [RegularExpression(@"^\+?\d{10,15}$")]
    public string Phone { get; set; } = null!;

    [Required]
    [Range(0, 1000000.00)]
    public decimal Salary { get; set; }
}
