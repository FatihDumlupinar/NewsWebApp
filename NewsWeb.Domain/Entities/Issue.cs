using NewsWeb.Domain.Common;

namespace NewsWeb.Domain.Entities
{
    public class Issue : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }
}
