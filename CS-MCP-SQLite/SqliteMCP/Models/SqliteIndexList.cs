namespace SqliteMCP.Models;

public class SqliteIndexList
{
    public int Seq { get; set; }
    public string Name { get; set; }
    public int Unique { get; set; }
    public string Origin { get; set; }
    public int Partial { get; set; }
}