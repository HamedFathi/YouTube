namespace SqliteMCP.Models;

public class SqliteColumn
{
    public int Cid { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int NotNull { get; set; }
    public string Dflt_Value { get; set; }
    public int Pk { get; set; }
}