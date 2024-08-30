using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QLThuVien.Business.Models;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
}