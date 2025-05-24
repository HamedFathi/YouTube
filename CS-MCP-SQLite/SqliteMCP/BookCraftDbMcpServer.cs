using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using ModelContextProtocol.Server;
using SqliteMCP.Extensions;

namespace SqliteMCP;

[McpServerToolType, Description("""
                                    BookCraftDbMcpServer is a specialized MCP Server Tool interface for exploring and modifying the 'BookCraftDB' SQLite database.
                                
                                    It provides a structured and semantic way to:
                                    - Retrieve full database schema information
                                    - Inspect individual tables and views
                                    - Explore table relationships (foreign keys)
                                    - Add new columns to existing tables
                                    - Create new tables
                                
                                    All operations are tailored exclusively for SQLite syntax and features, as BookCraftDB is implemented using SQLite. 
                                    Any request outside SQLite capabilities will be gracefully handled or responded with a humorous SQLite-themed dad joke.
                                
                                    Use this server when you want to:
                                    - Write SQL queries for BookCraftDB
                                    - Understand how the database is structured
                                    - Add or change schema elements like columns and tables
                                """)]
public static class BookCraftDbMcpServer
{
    private static string? _dbPath;

    public static void Configure(IConfiguration configuration)
    {
        _dbPath = configuration["Database:Path"];
    }

    private static string GetConnectionString()
    {
        if (string.IsNullOrWhiteSpace(_dbPath))
            throw new InvalidOperationException("Database path not configured.");
        return $"Data Source={_dbPath}";
    }

    private static SqliteSchemaService CreateSqliteSchemaService() => new(GetConnectionString());

    [McpServerTool, Description("""
        Returns a complete general overview of the BookCraftDB system, including its design purpose, usage context, and capabilities.
        Useful when a user asks: "What is BookCraftDB?" or "Tell me about the database."
    """)]
    public static string GeneralInfo()
    {
        return """
               BookCraftDB is a lightweight relational database system designed by Hamed Fathi on May 24, 2025, specifically for SQLite.

               It powers a fully operational Library Management System, covering user registration, cataloging, loans, reservations,
               fines, and automation via triggers. Views summarize data for quick reporting.

               Built on SQLite, it offers reliable performance without needing a separate database server — ideal for desktop apps
               or smaller institutions that prefer file-based storage with rich functionality.
               """;
    }

    [McpServerTool, Description("""
        Returns the schema and metadata of all user-defined tables in the BookCraftDB database.
        This includes column info, primary keys, foreign keys, indexes, and triggers per table.

        Always use this tool to explore table structure before writing SQL queries.
        If the user says: "List tables", "Show schema", "What are the tables?", call this.
    """)]
    public static string GetTablesInfo()
    {
        var explorer = CreateSqliteSchemaService();
        return explorer.GetTables().ToYaml();
    }

    [McpServerTool, Description("""
        Returns full details of a specific table including columns, primary keys, foreign keys, triggers, and indexes.

        Use this tool when the user requests:
        - A specific table's structure
        - Wants to add a column (to validate table first)
        - Asks "What is in the [TableName] table?"

        If table is not found, inform the user gracefully.
    """)]
    public static string GetTableInfo(string tableName)
    {
        var explorer = CreateSqliteSchemaService();
        var table = explorer.GetTable(tableName);
        return table?.ToYaml() ?? $"Table '{tableName}' not found. Please check the table name.";
    }

    [McpServerTool, Description("""
        Returns all foreign key relationships between tables in BookCraftDB.

        This tool shows how tables are connected, which is essential when writing JOIN queries.
        Use this if user asks:
        - "Show table relations"
        - "How are tables connected?"
        - "What are foreign keys in BookCraftDB?"
    """)]
    public static string GetTableRelationsInfo()
    {
        var explorer = CreateSqliteSchemaService();
        return explorer.GetForeignKeyRelations().ToYaml();
    }

    [McpServerTool, Description("""
        Returns metadata of all views in BookCraftDB including their SQL definitions.

        Use this tool if the user asks:
        - "Show database views"
        - "What summaries or reports exist?"
        - "What data is pre-aggregated?"

        Output is YAML containing view names and SQL bodies.
    """)]
    public static string GetViewsInfo()
    {
        var explorer = CreateSqliteSchemaService();
        return explorer.GetViews().ToYaml();
    }

    [McpServerTool, Description("""
        Adds a new column to an existing table.

        Requires:
        - Valid `tableName`
        - New `columnName`
        - SQLite-compatible `columnType` (e.g., TEXT, INTEGER)

        If an invalid type is provided, convert it to the closest valid SQLite type.
        If there's an error (e.g., duplicate column or bad name), return error message.

        Example use:
        AddNewColumn("Users", "Bio", "TEXT")
    """)]
    public static string AddNewColumn(string tableName, string columnName, string columnType)
    {
        var explorer = CreateSqliteSchemaService();
        return explorer.AddTableColumn(tableName, columnName, columnType);
    }

    [McpServerTool, Description("""
        Creates a new table in the BookCraftDB with the provided column definitions.

        Input: Table name and a Dictionary<string, string> with column names and SQLite-compatible types.

        Example:
        {
            "Id": "INTEGER PRIMARY KEY AUTOINCREMENT",
            "Username": "TEXT NOT NULL",
            "Email": "TEXT",
            "CreatedAt": "TEXT NOT NULL"
        }

        Ensure:
        - Column types are valid SQLite types
        - Inform the user on success/failure
    """)]
    public static string CreateNewTable(string tableName, Dictionary<string, string> columns)
    {
        var explorer = CreateSqliteSchemaService();
        return explorer.CreateTable(tableName, columns);
    }
}



