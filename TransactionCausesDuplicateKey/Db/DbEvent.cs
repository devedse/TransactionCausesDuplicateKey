using System.ComponentModel.DataAnnotations;

namespace TransactionCausesDuplicateKey.Db
{
    public class DbEvent
    {
        [Key]
        public int Id { get; set; }

        public string UniqueEventId { get; set; }
    }
}
