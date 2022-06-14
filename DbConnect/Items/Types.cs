﻿using Npgsql;
using Type = DbConnect.Models.Type;

namespace DbConnect.Items;

public static class Types
{
    private static void Check(NpgsqlConnection npgsqlConnection, string name)
    {
        var selCmd = new NpgsqlCommand("SELECT * FROM types " +
                                       "WHERE name = @name",
            npgsqlConnection);
        selCmd.Parameters.AddWithValue("name", name);

        var dataReader = selCmd.ExecuteReader();

        try
        {
            if (dataReader.HasRows)
                throw new Exception("Данная запись уже существует в базе данных");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if(dataReader is {IsClosed:false})
                dataReader.Close();
        }
    }

    public static int Add(string name)
    {
        DbConnection.Start();

        var npgsqlConnection = DbConnection.NpgsqlConnection;
        var state = DbConnection.IsConnected;
        if (!state)
        {
            throw new NpgsqlException("Не удалось подключиться к базе данных");
        }

        Check(npgsqlConnection,name);
        
        var insCmd =
            new NpgsqlCommand("INSERT INTO types (name) VALUES (@name)",
                npgsqlConnection);
        insCmd.Parameters.AddWithValue("name", name);
        
        var result = insCmd.ExecuteNonQuery();

        DbConnection.Stop();
        return result;
    }

    private static NpgsqlDataReader GetDataReader()
    {
        DbConnection.Start();

        var npgsqlConnection = DbConnection.NpgsqlConnection;
        var state = DbConnection.IsConnected;
        if (!state)
        {
            throw new NpgsqlException("Не удалось подключиться к базе данных");
        }

        const string query = "SELECT t.id as id" +
                             ", t.name as name " +
                             "FROM types t " +
                             "ORDER BY t.id";
        var npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection);
        return npgsqlCommand.ExecuteReader();
    }

    public static IEnumerable<Type> GetItems()
    {
        var dataReader = GetDataReader();
        
        while (dataReader.Read())
        {
            yield return new Type
            {
                Id = Convert.ToInt32(dataReader["id"].ToString() ?? string.Empty),
                Name = dataReader["name"].ToString() ?? string.Empty
            };
        }

        if (dataReader is {IsClosed: false})
            dataReader.Close();
        
        DbConnection.Stop();
    }
}