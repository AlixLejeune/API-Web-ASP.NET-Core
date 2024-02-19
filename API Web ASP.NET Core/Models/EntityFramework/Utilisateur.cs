using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace API_Web_ASP.NET_Core.Models.EntityFramework
{
    [Table("t_e_utilisateur_utl")]
    [Index(nameof(Mail), Name = "uq_utl_mail", IsUnique = true)]
    public partial class Utilisateur
    {
        public Utilisateur()
        {
            NotesUtilisateur = new HashSet<Notation>();
        }

        [Key]
        [Column("utl_id")]
        public int UtilisateurId { get; set; }
        [Column("utl_nom")]
        [StringLength(50)]
        public string? Nom { get; set; }
        [Column("utl_prenom")]
        [StringLength(50)]
        public string? Prenom { get; set; }
        [Column("utl_mobile")]
        [StringLength(10)]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Le mobile doit contenir 10 chiffres")]
        public string? Mobile { get; set; }
        [Required]
        [Column("utl_mail")]
        [EmailAddress]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La longueur d’un email doit être comprise entre 6 et 100 caractères.")]
        public string Mail { get; set; } = null!;
        [Required]
        [Column("utl_pwd")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[-+!*$@%_])([-+!*$@%_\w]{12,20})$", ErrorMessage = "Le mot de passe doit contenir entre 12 et 20 caractères avec au moins 1 lettre majuscule, 1 chiffre et 1 caractère spécial")]
        [StringLength(64)]
        public string Pwd { get; set; } = null!;
        [Column("utl_rue")]
        [StringLength(200)]
        public string? Rue { get; set; }
        [Column("utl_cp")]
        [StringLength(5)]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Le code postal doit contenir 5 chiffres")]
        public string? CodePostal { get; set; }
        [Column("utl_ville")]
        [StringLength(50)]
        public string? Ville { get; set; }
        [Column("utl_pays")]
        [StringLength(50)]
        public string? Pays { get; set; }
        [Column("utl_latitude")]
        public float? Latitude { get; set; }
        [Column("utl_longitude")]
        public float? Longitude { get; set; }
        [Required]
        [Column("utl_datecreation", TypeName = "date")]
        public DateTime DateCreation { get; set; }

        [InverseProperty(nameof(Notation.UtilisateurNotant))]
        public virtual ICollection<Notation> NotesUtilisateur { get; set; }


    }
}
