using ApplicationTracker.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApplicationTracker.Dtos
{
    public class ApplicationDto
    {

        public required string Title { get; set; }
        public required string Company { get; set; }
        public required string Notes { get; set; }
        public required string Status { get; set; }

        public string? Link { get; set; } = string.Empty;

    }
}
