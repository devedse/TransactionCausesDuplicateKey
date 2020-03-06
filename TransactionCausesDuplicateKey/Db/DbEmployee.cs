using System.ComponentModel.DataAnnotations;

namespace TransactionCausesDuplicateKey.Db
{
    public class DbEmployee
    {
        [Key]
        public int Id { get; set; }

        public string PersonName { get; set; }

        public string PersonNumber { get; set; }
        public string PersonId { get; set; }
    }
}
