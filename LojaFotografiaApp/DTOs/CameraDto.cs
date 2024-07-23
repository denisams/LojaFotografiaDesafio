namespace LojaFotografiaApp.DTOs
{
    public class CameraDto
    {
        public int Id { get; set; } = 0;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
