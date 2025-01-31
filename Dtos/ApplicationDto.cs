using ApplicationTracker.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApplicationTracker.Dtos
{
    public class ApplicationDto
    {

        public required string Title { get; set; }
        public required string Description { get; set; }
        public string Status { get; set; } = string.Empty;

        public UrlAttribute? Link { get; set; }

    }
}
