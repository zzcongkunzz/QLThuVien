using System.ComponentModel.DataAnnotations;

namespace QLThuVien.Business.Models;

public class Penalty
{
    public Guid Id { get; set; }
    [Required]
    public Borrow Borrow { get; set; }
    [Required]
    public string Note { get; set; }
    [Required]
    public float Fees { get; set; }
    [Required]
    public string Status { get; set; }
}