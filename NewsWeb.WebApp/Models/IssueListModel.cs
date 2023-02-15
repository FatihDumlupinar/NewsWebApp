namespace NewsWeb.WebApp.Models
{
    public class IssueListModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }= DateTime.Now;

    }
}
