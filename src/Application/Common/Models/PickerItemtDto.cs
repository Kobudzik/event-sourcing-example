namespace EventSourcingExample.Application.Common.Models
{
    public class PickerItemtDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public PickerItemtDto()
        {
        }

        public PickerItemtDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
