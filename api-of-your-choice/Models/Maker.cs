namespace api_of_your_choice.Models
{
    public class Maker
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int YearFounded { get; set; }
        public string? Type { get; set; }
        public virtual List<Flyrod>? flyrods { get; set; }
    }
}
