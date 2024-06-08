using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Pickfc.Model.Entities
{
    public class Pick
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int RoundID { get; set; }
        public int GameID { get; set; }
        public int PlayerID { get; set; }
        public int TeamID { get; set; }
        public int RoundNumber { get; set; }
        public string Result { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public DateTime Timestamped { get; set; }
    }
}
