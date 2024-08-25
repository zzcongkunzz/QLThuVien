using System.ComponentModel.DataAnnotations;

namespace QLThuVien.Business.Models;

public class Book
{
    public Guid Id { get; set; }
    [Required]
    public string AuthorName { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? PublishDate { get; set; }
    [Required]
    public Category Category { get; set; }
}