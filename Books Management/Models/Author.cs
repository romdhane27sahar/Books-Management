using System.ComponentModel.DataAnnotations;

namespace Books_Management.Models
{
    public class Author
    {
        [Key]
        public int IdA { get; set; }

        [Required(ErrorMessage = "Le prénom est un champ obligatoire !")] //champs obligatoire avec message erreur 
        [StringLength(20, MinimumLength = 3, ErrorMessage = "votre prénom doit contenir entre 3 et 20 caractères !")]
        public string FirstName { get; set; } = null!;


        [Required(ErrorMessage = "Le nom est un champ obligatoire !")] //champs obligatoire avec message erreur 
        [StringLength(20, MinimumLength = 3, ErrorMessage = "votre nom doit contenir entre 3 et 20 caractères !")]
        public string LastName { get; set; } = null!;

        [StringLength(20, MinimumLength = 3, ErrorMessage = "La nationnalité doit contenir entre 3 et 20 caractères !")]
        public string Nationality { get; set; } = null!;


        [Required(ErrorMessage = "L'email est un champ obligatoire !")]
        [RegularExpression(@"^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,6}$", ErrorMessage = "Veuillez entrer un format d'email valide !")]
        public string Email { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";


        public virtual ICollection<Book>? Books{ get; set;} = new List <Book>();

    }
}
