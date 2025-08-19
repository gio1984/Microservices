using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("DataSmokeDetector")]
public class SmokeDetectorModel
{
    [Key]
    public DateTime EventIdUtc { get; set; }
    public int DetectorId { get; set; }
    public required string Status { get; set; }
}