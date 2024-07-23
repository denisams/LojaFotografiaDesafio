namespace LojaFotografiaApp.DTOs
{
    public class CameraDto
    {
        public int Id { get; set; } = 0;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
