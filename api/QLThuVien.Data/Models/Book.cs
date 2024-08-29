using QLThuVien.Data.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLThuVien.Business.Models;

public class Book
{
    public Guid Id { get; set; }
    [Required]
    public required string AuthorName { get; set; }
    [Required]
    public required string Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public required string PublisherName { get; set; }
    [Required]
    public required DateOnly PublishDate { get; set; }
    [DefaultValue(0)]
    public required int Count { get; set; }
    public Category Category { get; set; }
    public IEnumerable<User> BorrowingUsers { get; set; }
    public IEnumerable<Rating> Ratings { get; set; }
}