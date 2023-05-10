using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class DataEmailTag
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool Architectural { get; set; }

    public virtual ICollection<DataEmailEmail> Emails { get; set; } = new List<DataEmailEmail>();
}
