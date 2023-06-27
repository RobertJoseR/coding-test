namespace coding_test.Data.Entities
{
    public interface IAuditEdit
    {
        public string? ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
    }
}
