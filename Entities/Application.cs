using System.ComponentModel.DataAnnotations;

namespace ApplicationTracker.Entities
{
    public class Application
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Title { get; set; }
        public required string Description { get; set; }
        public string Status { get; set; } = string.Empty;

        [Url]
        public string? Link { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public required string UserId { get; set; }
        public required CustomUser CustomUser { get; set; }
    }
}
