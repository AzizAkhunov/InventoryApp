namespace InventoryApp.Application.DTO
{
    public class InventoryDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string CategoryName { get; set; }

        public bool IsPublic { get; set; }
        public string? ImageUrl { get; set; }

        public int ItemsCount { get; set; }
        public int Version { get; set; }
        public string ApiToken { get; set; }

        // Custom string fields
        public bool CustomString1Enabled { get; set; }
        public string? CustomString1Name { get; set; }

        public bool CustomString2Enabled { get; set; }
        public string? CustomString2Name { get; set; }

        public bool CustomString3Enabled { get; set; }
        public string? CustomString3Name { get; set; }

        // Custom number fields
        public bool CustomNumber1Enabled { get; set; }
        public string? CustomNumber1Name { get; set; }

        public bool CustomNumber2Enabled { get; set; }
        public string? CustomNumber2Name { get; set; }

        public bool CustomNumber3Enabled { get; set; }
        public string? CustomNumber3Name { get; set; }

        // Custom bool fields
        public bool CustomBool1Enabled { get; set; }
        public string? CustomBool1Name { get; set; }

        public bool CustomBool2Enabled { get; set; }
        public string? CustomBool2Name { get; set; }

        public bool CustomBool3Enabled { get; set; }
        public string? CustomBool3Name { get; set; }

        //Tags
        public List<string>? Tags { get; set; }
    }
}
