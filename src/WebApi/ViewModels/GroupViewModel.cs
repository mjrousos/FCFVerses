namespace WebApi.ViewModels
{
    public class GroupViewModel
    {
        public GroupViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }
}
