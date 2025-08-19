public class ElevatorModel
{
    public DateTime EventIdUtc { get; set; }
    public int ElevatorId { get; set; }
    public int Floor { get; set; }
    public required string Status { get; set; }
}