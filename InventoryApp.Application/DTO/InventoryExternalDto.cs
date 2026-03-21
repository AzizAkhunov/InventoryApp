namespace InventoryApp.Application.DTO
{
    public class InventoryExternalDto
    {
        public Guid InventoryId { get; set; }
        public string Title { get; set; } = null!;
        public int ItemsCount { get; set; }

        public List<ExternalFieldDto> Fields { get; set; } = new();
        public List<ExternalNumberStatDto> NumberStats { get; set; } = new();
        public List<ExternalTextStatDto> TextStats { get; set; } = new();
        public List<ExternalBoolStatDto> BoolStats { get; set; } = new();
    }

    public class ExternalFieldDto
    {
        public string Key { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public class ExternalNumberStatDto
    {
        public string FieldKey { get; set; } = null!;
        public string FieldTitle { get; set; } = null!;
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
        public decimal? Avg { get; set; }
    }

    public class ExternalTextStatDto
    {
        public string FieldKey { get; set; } = null!;
        public string FieldTitle { get; set; } = null!;
        public List<PopularTextValueDto> TopValues { get; set; } = new();
    }

    public class PopularTextValueDto
    {
        public string Value { get; set; } = null!;
        public int Count { get; set; }
    }

    public class ExternalBoolStatDto
    {
        public string FieldKey { get; set; } = null!;
        public string FieldTitle { get; set; } = null!;
        public int TrueCount { get; set; }
        public int FalseCount { get; set; }
    }
}
