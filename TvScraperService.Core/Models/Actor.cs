using System.Text.Json.Serialization;

namespace TvScraperService.Core.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        [JsonIgnore]
        public int TVMazeId { get; set; }
        [JsonIgnore]
        public List<Show> Shows { get; set; }
    }
}
