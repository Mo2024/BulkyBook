namespace BulkyBookWeb.Models
{
	public class CreateProductViewModel
	{
		public IEnumerable<Category> categories { get; set; }
		public Product Product { get; set; }
	}
}
