namespace Catalogo.Core.DTOs
{
    public class CameraDto
    {
        public required string Marca { get; set; }
        public string? Modelo { get; set; }
        public decimal Preco { get; set; }
        public string? Descricao { get; set; }
    }
}
