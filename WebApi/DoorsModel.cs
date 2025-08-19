using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("DataDoor")]
public class DoorsModel
{
    [Key]
    public DateTime EventIdUtc { get; set; }
    public int DoorId { get; set; }
    public string? Status { get; set; }
}