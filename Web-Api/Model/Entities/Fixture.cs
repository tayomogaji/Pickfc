using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pickfc.Model.Entities
{
    public class Fixture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int RoundID { get; set; }
        public int  HomeID { get; set; }
        public int AwayID { get; set; }
        public string HomeResult { get; set; } = string.Empty; 
        public string AwayResult { get; set; } = string.Empty;
        public DateTime TimeStamped { get; set; }
    }
}
