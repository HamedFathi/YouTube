namespace SqliteMCP.Models;

public class SqliteIndex
{
    public int Seq { get; set; }
    public string Name { get; set; }
    public bool Unique { get; set; }
    public string Origin { get; set; }   
    public bool Partial { get; set; }
    public List<string> Columns { get; set; } = new();
}