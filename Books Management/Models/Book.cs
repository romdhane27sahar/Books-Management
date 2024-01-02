using System.ComponentModel.DataAnnotations;

namespace Books_Management.Models
{
    public class Book
    {
        [Key]
        public int IdB { get; set; }

        [Required(ErrorMessage = "Le titre du livre est un champ obligatoire !")] //champs obligatoire avec message erreur 
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Le titre du livre doit contenir entre 3 et 30 caractères !")]
        public string Title { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Le genre est un champ obligatoire !")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Le genre doit contenir entre 3 et 30 caractères !")]
        public string Genre { get; set; } = null!;

        public int AuthorId { get; set; }
        public virtual Author? Author { get; set; } = null;

    }
}
