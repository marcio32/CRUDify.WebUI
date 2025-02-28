using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(200, ErrorMessage = "El nombre no puede tener mas de 200 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.00, 1000000, ErrorMessage = "El precio debe estar entre 0 y 1000000")]
        public decimal Price { get; set; }
        [MaxLength(500, ErrorMessage = "La descripción no puede tener mas de 500 caracteres")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, 20000, ErrorMessage = "El stock debe estar entre 0 y 1000000")]
        public int Stock { get; set; }
        public bool Active { get; set; }
    }
}
