using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace Danmarks_Radio.Models
{
    public class MusicRecord
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public float Duration { get; set; }
        public int Year { get; set; }
  
        public override string ToString()
        {
            return $"Title: {Title} Artist: {Artist} Duration: {Duration} Year: {Year}";
        }
    }
}


