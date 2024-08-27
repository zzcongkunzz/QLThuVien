using QLThuVien.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.ViewModels;

public class BookVm
{
    public string AuthorName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? PublishDate { get; set; }
    public string CategoryName { get; set; }
}
