using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pickfc.Model.Entities
{
    public class CompTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CompID { get; set; }
        public int TeamID { get; set; }
        public DateTime Timestamped { get; set; }
    }
}
