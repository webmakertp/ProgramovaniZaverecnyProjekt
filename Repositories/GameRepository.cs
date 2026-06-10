using System;
using System.Collections.Generic;
using System.Data;
using GameTrackerApp.Models;
using Npgsql;

namespace GameTrackerApp.Repositories;

// implementace crud operaci pro hry
public class GameRepository : IGameRepository
{
    private readonly string _connString;

    public GameRepository(string connString)
    {
        _connString = connString;
    }

    public List<Game> GetAll()
    {
        var list = new List<Game>();

        // tady delam join na platformu abych mel nazev misto jen cisla
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT g.Id, g.Title, g.Developer, g.ReleaseYear, g.PlatformId, p.Name " +
            "FROM Game g LEFT JOIN Platform p ON g.PlatformId = p.Id " +
            "ORDER BY g.Id", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Game
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Developer = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                ReleaseYear = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                PlatformId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                PlatformName = reader.IsDBNull(5) ? null : reader.GetString(5)
            });
        }

        return list;
    }

    public Game? GetById(int id)
    {
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT g.Id, g.Title, g.Developer, g.ReleaseYear, g.PlatformId, p.Name " +
            "FROM Game g LEFT JOIN Platform p ON g.PlatformId = p.Id " +
            "WHERE g.Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        if (!reader.Read()) return null;

        return new Game
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Developer = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            ReleaseYear = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
            PlatformId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
            PlatformName = reader.IsDBNull(5) ? null : reader.GetString(5)
        };
    }

    public int Insert(Game game)
    {
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "INSERT INTO Game (Title, Developer, ReleaseYear, PlatformId) " +
            "VALUES (@title, @dev, @year, @plat) RETURNING Id", conn);
        cmd.Parameters.AddWithValue("@title", game.Title);
        cmd.Parameters.AddWithValue("@dev", string.IsNullOrEmpty(game.Developer) ? (object)DBNull.Value : game.Developer);
        cmd.Parameters.AddWithValue("@year", game.ReleaseYear == 0 ? (object)DBNull.Value : game.ReleaseYear);
        cmd.Parameters.AddWithValue("@plat", game.PlatformId.HasValue ? (object)game.PlatformId.Value : DBNull.Value);

        var result = cmd.ExecuteScalar();
        return result != null ? (int)result : 0;
    }

    public void Update(Game game)
    {
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "UPDATE Game SET Title = @title, Developer = @dev, ReleaseYear = @year, PlatformId = @plat " +
            "WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@title", game.Title);
        cmd.Parameters.AddWithValue("@dev", string.IsNullOrEmpty(game.Developer) ? (object)DBNull.Value : game.Developer);
        cmd.Parameters.AddWithValue("@year", game.ReleaseYear == 0 ? (object)DBNull.Value : game.ReleaseYear);
        cmd.Parameters.AddWithValue("@plat", game.PlatformId.HasValue ? (object)game.PlatformId.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@id", game.Id);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        // bacha na to ze sessiony se smazou samy kvuli on delete cascade
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand("DELETE FROM Game WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
