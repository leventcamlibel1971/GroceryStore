using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.Entity.Domain
{
    public class Customer
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }
    }
}