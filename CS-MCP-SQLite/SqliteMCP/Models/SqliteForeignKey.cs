namespace SqliteMCP.Models;

public class SqliteForeignKey
{
    public int Id { get; set; }
    public int Seq { get; set; }
    public string Table { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string On_Update { get; set; }
    public string On_Delete { get; set; }
    public string Match { get; set; }
}