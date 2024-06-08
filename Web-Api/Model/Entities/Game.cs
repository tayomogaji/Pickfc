using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pickfc.Model.Entities
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CreatorID { get; set; }
        public int CompID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool Public { get; set; }
        public bool Legacy { get; set; }
        public bool Deadline { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public DateTime ResetDate { get; set; }
        public DateTime Timestamped { get; set; }
    }
}
