using System.Collections.Generic;
using GameTrackerApp.Models;
using Npgsql;

namespace GameTrackerApp.Repositories;

// implementace repozitare pro platformy, jednoduchy select
public class PlatformRepository : IPlatformRepository
{
    private readonly string _connString;

    public PlatformRepository(string connString)
    {
        _connString = connString;
    }

    public List<Platform> GetAll()
    {
        var list = new List<Platform>();

        // tady to taha data z db tak snad to nespadne
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT Id, Name FROM Platform ORDER BY Id", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Platform
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return list;
    }
}
