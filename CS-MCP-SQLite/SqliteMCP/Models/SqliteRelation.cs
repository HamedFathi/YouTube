namespace SqliteMCP.Models;

public class SqliteRelation
{
    public string FromTable { get; set; }
    public string FromColumn { get; set; }
    public string ToTable { get; set; }
    public string ToColumn { get; set; }
    public string OnUpdate { get; set; }
    public string OnDelete { get; set; }
}