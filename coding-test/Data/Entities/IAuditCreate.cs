namespace coding_test.Data.Entities
{
    public interface IAuditCreate
    {
        public string CreatedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
