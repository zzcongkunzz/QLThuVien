using QLThuVien.Business.Models;
using QLThuVien.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.ViewModels;

public class BookVm
{
    public required Guid Id { get; set; }
    public required string AuthorName { get; set; }
    public required string Title { get; set; }
    public required string? Description { get; set; }
    public required string PublisherName { get; set; }
    public required DateOnly PublishDate { get; set; }
    public required string CategoryName { get; set; }
    public required int Count { get; set; }
    public required string ImageUrl { get; set; }
    public float? AverageRating { get; set; }
}
