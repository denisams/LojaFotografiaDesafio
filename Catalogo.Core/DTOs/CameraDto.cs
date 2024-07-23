namespace Catalogo.Core.DTOs
{
    public class CameraDto
    {
        public required string Brand { get; set; }
        public string? Model { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
