using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAuthRole.Models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        [ForeignKey("Username")]
        public virtual string AuthorName { get; set; } = "";
    }
}
