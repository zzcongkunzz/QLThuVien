using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.ViewModels;

public class BookEditVm
{
    public required string AuthorName { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string PublisherName { get; set; }
    public required DateOnly PublishDate { get; set; }
    public required string CategoryName { get; set; }
    public required int Count { get; set; }
}
