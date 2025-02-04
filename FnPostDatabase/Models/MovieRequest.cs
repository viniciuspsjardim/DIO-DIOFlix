using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FnPostDatabase.Models;

public class MovieRequest
{
    public string id { get; } = Guid.NewGuid().ToString();
    public required int year { get; set; }
    public required string title { get; set; }
    public required string videoPath { get; set; }
    public required string thumbPath { get; set; }
}