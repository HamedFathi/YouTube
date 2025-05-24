namespace SqliteMCP.Models;

public class SqliteTrigger
{
    public string Name { get; set; }
    public string TableName { get; set; }
    public string Sql { get; set; }
}