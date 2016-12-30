using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    private const string ConnectionString = "Server=172.16.0.5;" +
                                            "Database=AdventureWorksLT2008;" +
                                            "User ID=AdvWorksUser;" +
                                            "Password=Pa$$w0rd;";

    public List<string> GetTables()
    {
        var tables = new List<string>();

        try
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("select * from sys.tables;", sqlConnection))
                {
                    sqlConnection.Open();
                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        tables.Add(reader[0].ToString());
                    }
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return tables;
    }

    public List<string> GetTableData(string tableName)
    {
        var data = new List<string>();

        try
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("select * from " + tableName + ";", sqlConnection))
                {
                    sqlConnection.Open();
                    var reader = sqlCommand.ExecuteReader();
                    var firstRow = true;
                    while (reader.Read())
                    {
                        if (firstRow)
                        {
                            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                            var colNames = columns.Aggregate("", (current, colName) => current + (colName + " | "));
                            data.Add(colNames);
                        }
                        var row = "";
                        for (var i = 0; i < reader.FieldCount; ++i)
                        {
                            row += reader[i].ToString() + " | ";
                        }
                        data.Add(row);
                        firstRow = false;
                    }
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return data;
    }
}
