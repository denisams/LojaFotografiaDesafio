namespace LojaFotografiaApp.DTOs
{
    public class AccessoryDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public AccessoryDto()
        {
            Id = 0; // Inicialize com um valor padrão
        }
    }
}
