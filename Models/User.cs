using System;
using System.Collections.Generic;

namespace BlogBackend.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; } = new List<Article>();
}
