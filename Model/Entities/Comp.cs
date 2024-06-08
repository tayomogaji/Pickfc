using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pickfc.Model.Entities
{
    public class Comp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int AdminID { get; set; }
        public int RoundID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;
        public int TeamsTotal { get; set; }
        public bool Legacy { get; set; }
        public bool Active { get; set; }
        public bool Default { get; set; }
        public bool OpenNotified { get; set; }
        public DateTime Open { get; set; }
        public DateTime Timestamped { get; set; }
    }
}
