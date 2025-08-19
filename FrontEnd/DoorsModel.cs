using System.ComponentModel.DataAnnotations;

public class DoorsModel
{
    public DateTime EventIdUtc { get; set; }
    public int DoorId { get; set; }
    public required string Status { get; set; }
}