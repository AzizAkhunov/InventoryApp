namespace InventoryApp.Application.DTO
{
    public class InventoryStatisticsDto
    {
        public int ItemsCount { get; set; }

        public List<NumberFieldStatsDto> NumberStats { get; set; } = new();

        public List<StringFieldStatsDto> TopStrings { get; set; } = new();
    }
}
