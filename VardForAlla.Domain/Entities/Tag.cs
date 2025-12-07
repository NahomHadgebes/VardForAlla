namespace VardForAlla.Domain.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Routine> Routines { get; set; } = new List<Routine>();
}

