using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pickfc.Model.Entities
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int CompsCount { get; set; }
        public bool Club { get; set; }
        public DateTime Timestamped { get; set; }
    }
} 
