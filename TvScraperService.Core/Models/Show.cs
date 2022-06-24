using System.Text.Json.Serialization;

namespace TvScraperService.Core.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public int TVMazeId { get; set; }
        public List<Actor> Cast { get; set; }
    }
}
