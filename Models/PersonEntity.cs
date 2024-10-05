using System.ComponentModel.DataAnnotations.Schema;

namespace Trainee_Test.Models;

[Table(name: "Person")]
public class PersonEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public bool Married { get; set; }

    public string Phone { get; set; } = null!;

    public decimal Salary { get; set; }
}