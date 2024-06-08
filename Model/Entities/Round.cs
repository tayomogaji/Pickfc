using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pickfc.Model.Entities
{
    public class Round
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CompID { get; set; }
        public int Number { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Show { get; set; }
        public bool StartNotified { get; set; }
        public bool DeadlineNotified { get; set; }
        public DateTime Start { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime Timestamped { get; set; }
    }
}
