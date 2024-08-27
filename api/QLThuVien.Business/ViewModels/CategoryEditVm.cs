using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.ViewModels;

public class CategoryEditVm
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
}
