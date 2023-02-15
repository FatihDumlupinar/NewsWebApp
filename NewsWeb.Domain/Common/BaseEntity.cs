using System.ComponentModel.DataAnnotations;

namespace NewsWeb.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public Guid CreateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateUserId { get; set; }

    }
}
