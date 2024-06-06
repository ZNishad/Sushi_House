namespace Sushi_House.DTOModels
{
    public class SushiDTO
    {
        public int SushiId { get; set; }
        public int? SushiTypeId { get; set; }
        public string? SushiName { get; set; }
        public string? SushiPicName { get; set; }
        public string? SushiInqr { get; set; }
        public decimal? SushiPrice { get; set; }
    }
}
