using System.ComponentModel.DataAnnotations.Schema;
using Projekt.Entities.Models;

namespace Projekt.Entities.Models;

public class Post : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    [NotMapped]
    public ICollection<Image>? Images { get; set; } = new List<Image>();
    public Guid SubCategoryId { get; set; }
    public Subcategory Subcategory { get; set; }
    public string? UserId { get; set; }
    public AppUser? AppUser { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}