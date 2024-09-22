namespace Server.Models;

public record Card(
    string Id,
    string Timestamp,
    string? Front,
    string? Back,
    string Level);
