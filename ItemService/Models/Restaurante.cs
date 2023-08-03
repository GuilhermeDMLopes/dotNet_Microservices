using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItemService.Models
{
    public class Restaurante
    {
        [Key]
        [Required]
        //Id do restaurante no banco local
        public int Id { get; set; }

        //Id do restaurante recebido de RestauranteService
        [Required]
        public int IdExterno { get; set; }

        [Required]
        public string Nome { get; set; }

        public ICollection<Item> Itens { get; set; } = new List<Item>();
    }
}