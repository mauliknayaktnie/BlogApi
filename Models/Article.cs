using System;
using System.Collections.Generic;

namespace BlogBackend.Models;

public partial class Article
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int? AuthorId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? Author { get; set; }
}
