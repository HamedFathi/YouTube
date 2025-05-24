namespace SqliteMCP.Models;

public class SqliteTable
{
    public string Name { get; set; } 
    public List<SqliteColumn> Columns { get; set; } = new();
    public List<string> PrimaryKeys { get; set; } = new();
    public List<SqliteForeignKey> ForeignKeys { get; set; } = new();
    public List<SqliteIndex> Indexes { get; set; } = new();
    public List<SqliteTrigger> Triggers { get; set; } = new();
}