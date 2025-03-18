using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBeans.Core.Common;
namespace TheBeans.Core.Entities
{
    public class BeanOfTheDay : BaseEntity
	{
		[Required]
		public Guid CoffeeBeanId { get; set; }

		[ForeignKey("CoffeeBeanId")]
		public virtual CoffeeBean CoffeeBean { get; set; }

		public DateTime SelectedDate { get; set; } = DateTime.UtcNow;

	}
}

