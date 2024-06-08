using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pickfc.Model.Entities
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CompID { get; set; }
        public int GameID { get; set; }
        public int PickID { get; set; }
        public int HitByID { get; set; }
        public int HitsTotal { get; set; }
        public int HitsPlayed { get; set; }
        public int Streak { get; set; }
        public int BoostTotal { get; set; }
        public int BoostPlayed { get; set; }
        public int Life { get; set; }
        public int Pts { get; set; }
        public int Champs { get; set; }
        public double PickTime { get; set; }
        public int RoundPts { get; set; }
        public double RoundPickTime { get; set; }
        public bool Eliminated { get; set; }
        public bool Admin { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamped { get; set; }
    }
}
