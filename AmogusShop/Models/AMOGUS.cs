using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmogusShop.Models
{
    public class AMOGUS
    {
        public AMOGUS()
        {

        }
        public AMOGUS(int id, string name, float quef, string image, string desc, float price, string characteristics, int amount)
        {
            Id = id;
            Name = name;
            Sus_Queficient = quef;
            Image = image;
            Description = desc;
            Price = price;
            Characteristics = characteristics;
            Amount = amount;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Sus_Queficient { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Characteristics { get; set; }
        public int Amount { get; set; }
    }
}
