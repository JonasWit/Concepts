using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Post.Query.Domain.Entities;

[Table("Post")]
public class PostEntity
{
    [Key]
    public Guid PostId { get; set; }

    public string Author { get; set; }
    public DateTime DatePosted { get; set; }
    public string Message { get; set; }
    public int Likes { get; set; }
    public virtual ICollection<CommentEntity> Comments { get; set; }
}

[Table("Comment")]
public class CommentEntity
{
    [Key]
    public Guid CommentId { get; set; }
    public string Username { get; set; }
    public DateTime CommentDate { get; set; }
    public string Comment { get; set; }
    public bool Edited { get; set; }
    public Guid PostId { get; set; }
    
    [JsonIgnore]
    public virtual PostEntity Post { get; set; }
}