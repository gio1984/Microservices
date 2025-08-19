using System.ComponentModel.DataAnnotations;

public class SmokeDetectorModel
{
    public DateTime EventIdUtc { get; set; }
    public int DetectorId { get; set; }
    public required string Status { get; set; }
}