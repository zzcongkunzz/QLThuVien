using System.ComponentModel.DataAnnotations;

namespace QLThuVien.Business.ViewModels;

public class CategoryEditVm
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
}
