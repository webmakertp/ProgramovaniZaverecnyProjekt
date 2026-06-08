using System;
using System.Collections.Generic;
using GameTrackerApp.Models;
using Npgsql;

namespace GameTrackerApp.Repositories;

// crud pro herni relace, sessiony
public class GameSessionRepository : IGameSessionRepository
{
    private readonly string _connString;

    public GameSessionRepository(string connString)
    {
        _connString = connString;
    }

    public List<GameSession> GetByGameId(int gameId)
    {
        var list = new List<GameSession>();

        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT Id, GameId, DatePlayed, HoursPlayed FROM GameSession WHERE GameId = @gid ORDER BY DatePlayed DESC", conn);
        cmd.Parameters.AddWithValue("@gid", gameId);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new GameSession
            {
                Id = reader.GetInt32(0),
                GameId = reader.GetInt32(1),
                DatePlayed = reader.GetDateTime(2),
                HoursPlayed = reader.GetDecimal(3)
            });
        }

        return list;
    }

    public GameSession? GetById(int id)
    {
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT Id, GameId, DatePlayed, HoursPlayed FROM GameSession WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        if (!reader.Read()) return null;

        return new GameSession
        {
            Id = reader.GetInt32(0),
            GameId = reader.GetInt32(1),
            DatePlayed = reader.GetDateTime(2),
            HoursPlayed = reader.GetDecimal(3)
        };
    }

    public void Insert(GameSession session)
    {
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "INSERT INTO GameSession (GameId, DatePlayed, HoursPlayed) VALUES (@gid, @date, @hours)", conn);
        cmd.Parameters.AddWithValue("@gid", session.GameId);
        cmd.Parameters.AddWithValue("@date", session.DatePlayed);
        cmd.Parameters.AddWithValue("@hours", session.HoursPlayed);
        cmd.ExecuteNonQuery();
    }

    public void Update(GameSession session)
    {
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "UPDATE GameSession SET GameId = @gid, DatePlayed = @date, HoursPlayed = @hours WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@gid", session.GameId);
        cmd.Parameters.AddWithValue("@date", session.DatePlayed);
        cmd.Parameters.AddWithValue("@hours", session.HoursPlayed);
        cmd.Parameters.AddWithValue("@id", session.Id);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = new NpgsqlConnection(_connString);
        conn.Open();
        using var cmd = new NpgsqlCommand("DELETE FROM GameSession WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
