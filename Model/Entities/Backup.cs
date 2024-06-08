using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Pickfc.Model.Entities
{
    public class Backup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int GameID { get; set; }
        public int PlayerID { get; set; }
        public int Streak { get; set; }
        public int Life { get; set; }
        public int Pos { get; set; }
        public int Pts { get; set; }
        public int BoostTotal { get; set; }
        public int HitsTotal { get; set; }
        public int HitByID { get; set; }
        public double PickTime { get; set; }
        public bool Eliminated { get; set; }
        public bool BackedUp { get; set; }
        public DateTime Timestamped { get; set; }
    }
}
