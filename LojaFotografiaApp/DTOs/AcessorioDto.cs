namespace LojaFotografiaApp.DTOs
{
    public class AcessorioDto
    {
        public int Id { get; set; } 
        public string Nome { get; set; }
        public string Marca { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }

        public AcessorioDto()
        {
            Id = 0; // Inicialize com um valor padrão
        }
    }
}
