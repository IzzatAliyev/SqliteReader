using System.Data;
using System.Text.Json;
using Microsoft.Data.Sqlite;

namespace Server;

public class Program
{
    public static void Main(string[] args)
    {
        Task.Run(async () =>
        {
            var cards = await ReadAll();
            var serialized = Serialize(cards);
            await WriteToFile(serialized);
        }).GetAwaiter().GetResult();
    }

    public static async Task<List<Card>> ReadAll()
    {
        var cards = new List<Card>();
        await using (var connection = new SqliteConnection(@"Data Source=../share.db"))
        {
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT id, timestamp, front, back, level FROM share_data ORDER BY front ASC";
            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var card = new Card(
                        reader.GetString("id"),
                        reader.GetString("timestamp"),
                        reader.GetString("front"),
                        reader.GetString("back"),
                        reader.GetString("level"));
                    cards.Add(card);
                }
            }

            await connection.CloseAsync();
        }

        return cards;
    }

    public static string Serialize(List<Card> cards)
    {
        JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper,
            WriteIndented = true,
        };

        var result = JsonSerializer.SerializeToUtf8Bytes(cards, _options);

        return System.Text.Encoding.UTF8.GetString(result);
    }

    public static async Task WriteToFile(string value)
    {
        string path = Path.GetFullPath("../result.json");
        await File.WriteAllLinesAsync(path, [value]);
    }

    public record Card(
        string Id,
        string Timestamp,
        string? Front,
        string? Back,
        string Level);
}