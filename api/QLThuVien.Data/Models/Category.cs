using System.ComponentModel.DataAnnotations;

namespace QLThuVien.Business.Models;

public class Category
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
}