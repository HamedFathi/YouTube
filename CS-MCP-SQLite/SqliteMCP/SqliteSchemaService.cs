using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using SqliteMCP.Models;

namespace SqliteMCP;

public class SqliteSchemaService(string connectionString)
{
    public List<SqliteTable> GetTables()
    {
        using var connection = new SqliteConnection(connectionString);

        var tables = connection.Query<SqliteTable>(
            """
            SELECT name AS Name, type AS Type 
            FROM sqlite_master 
            WHERE type = 'table' AND name NOT LIKE 'sqlite_%'
            ORDER BY name;
            """).ToList();

        foreach (var table in tables)
        {
            LoadTableDetails(connection, table);
        }

        return tables;
    }

    public SqliteTable? GetTable(string tableName)
    {
        using var connection = new SqliteConnection(connectionString);

        var table = connection.QuerySingleOrDefault<SqliteTable>(
            """
            SELECT name AS Name, type AS Type 
            FROM sqlite_master 
            WHERE type = 'table' AND name = @TableName;
            """, new { TableName = tableName });

        if (table != null)
            LoadTableDetails(connection, table);

        return table;
    }

    public List<SqliteView> GetViews()
    {
        using var connection = new SqliteConnection(connectionString);

        return connection.Query<SqliteView>(
            """
            SELECT name AS Name, sql AS Sql 
            FROM sqlite_master 
            WHERE type = 'view' AND name NOT LIKE 'sqlite_%'
            ORDER BY name;
            """).ToList();
    }

    public List<SqliteRelation> GetForeignKeyRelations()
    {
        using var connection = new SqliteConnection(connectionString);
        var relations = new List<SqliteRelation>();

        var tables = connection.Query<string>(
            """
            SELECT name FROM sqlite_master 
            WHERE type = 'table' AND name NOT LIKE 'sqlite_%';
            """);

        foreach (var table in tables)
        {
            var foreignKeys = connection.Query<SqliteForeignKey>(
                $"PRAGMA foreign_key_list({QuoteIdentifier(table)});").ToList();

            relations.AddRange(foreignKeys.Select(fk => new SqliteRelation
            {
                FromTable = table,
                FromColumn = fk.From,
                ToTable = fk.Table,
                ToColumn = fk.To,
                OnUpdate = fk.On_Update,
                OnDelete = fk.On_Delete
            }));
        }

        return relations;
    }

    public string CreateTable(string tableName, Dictionary<string, string> columns)
    {
        if (columns.Count == 0)
            return $"Cannot create table '{tableName}' with no columns.";

        try
        {
            var columnsDef = string.Join(", ", columns.Select(kv => $"{QuoteIdentifier(kv.Key)} {kv.Value}"));
            var sql = $"CREATE TABLE {QuoteIdentifier(tableName)} ({columnsDef});";

            using var connection = new SqliteConnection(connectionString);
            connection.Execute(sql);

            return $"Successfully created table '{tableName}'.";
        }
        catch (Exception ex)
        {
            return $"Error creating table '{tableName}': {ex.Message}";
        }
    }

    public string AddTableColumn(string tableName, string columnName, string columnType)
    {
        try
        {
            var sql = $"ALTER TABLE {QuoteIdentifier(tableName)} ADD COLUMN {QuoteIdentifier(columnName)} {columnType};";

            using var connection = new SqliteConnection(connectionString);
            connection.Execute(sql);

            return $"Added column '{columnName}' of type '{columnType}' to '{tableName}'.";
        }
        catch (Exception ex)
        {
            return $"Error adding column '{columnName}' to '{tableName}': {ex.Message}";
        }
    }

    private void LoadTableDetails(IDbConnection connection, SqliteTable table)
    {
        table.Columns = GetColumns(connection, table.Name);
        table.PrimaryKeys = GetPrimaryKeys(connection, table.Name);
        table.ForeignKeys = GetForeignKeys(connection, table.Name);
        table.Indexes = GetIndexes(connection, table.Name);
        table.Triggers = GetTriggers(connection, table.Name);
    }

    private List<SqliteColumn> GetColumns(IDbConnection connection, string tableName) =>
        connection.Query<SqliteColumn>($"PRAGMA table_info({QuoteIdentifier(tableName)});").ToList();

    private List<string> GetPrimaryKeys(IDbConnection connection, string tableName) =>
        GetColumns(connection, tableName)
            .Where(c => c.Pk > 0)
            .OrderBy(c => c.Pk)
            .Select(c => c.Name)
            .ToList();

    private List<SqliteForeignKey> GetForeignKeys(IDbConnection connection, string tableName) =>
        connection.Query<SqliteForeignKey>($"PRAGMA foreign_key_list({QuoteIdentifier(tableName)});").ToList();

    private List<SqliteIndex> GetIndexes(IDbConnection connection, string tableName)
    {
        var indexList = connection.Query<SqliteIndexList>($"PRAGMA index_list({QuoteIdentifier(tableName)});");

        return indexList.Select(idx =>
        {
            var columns = connection.Query<SqliteIndexInfo>(
                $"PRAGMA index_info({QuoteIdentifier(idx.Name)});")
                .OrderBy(i => i.Seqno)
                .Select(i => i.Name)
                .ToList();

            return new SqliteIndex
            {
                Seq = idx.Seq,
                Name = idx.Name,
                Unique = idx.Unique == 1,
                Origin = idx.Origin,
                Partial = idx.Partial == 1,
                Columns = columns
            };
        }).ToList();
    }

    private List<SqliteTrigger> GetTriggers(IDbConnection connection, string tableName) =>
        connection.Query<SqliteTrigger>(
            """
            SELECT name AS Name, tbl_name AS TableName, sql AS Sql
            FROM sqlite_master 
            WHERE type = 'trigger' AND tbl_name = @TableName;
            """, new { TableName = tableName }).ToList();

    private string QuoteIdentifier(string identifier) =>
        "\"" + identifier.Replace("\"", "\"\"") + "\"";
}
