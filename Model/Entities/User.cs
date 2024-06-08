using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pickfc.Model.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ArtID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;    
        //public string PicExternal { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool Notify { get; set; }
        public bool Active { get; set; }
        public bool Admin { get; set; }
        public bool FullAdmin { get; set; }
        public DateTime? VerifyTime { get; set; }
        public DateTime? CodeExpires { get; set; }
        public DateTime Timestamped { get; set; }
    }
}
