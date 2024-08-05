using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Papara_Bootcamp.Models.Input
{
    public class UpdateBookInputModel
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        [DisplayName("Kitap Adı")]
        public string Name { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 5)]
        [DisplayName("Kitap Yazar Bilgisi")]
        public string Author { get; set; }

        [Range(minimum: 50, maximum: 400)]
        [DisplayName("Kitap Sayfa Sayısı")]
        public int PageCount { get; set; }

        [Range(minimum: 1850, maximum: 2024)]
        [DisplayName("Kitap Yılı")]
        public int Year { get; set; }
    }
}
