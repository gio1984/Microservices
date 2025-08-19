using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("DataElevator")]
public class ElevatorModel
{
    [Key]
    public DateTime EventIdUtc { get; set; }
    public int ElevatorId { get; set; }
    public int Floor { get; set; }
    public required string Status { get; set; }
}