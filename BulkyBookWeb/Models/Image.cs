using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.Models
{

    public class Image
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int ImageID { get; set; }

        public string Url { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
