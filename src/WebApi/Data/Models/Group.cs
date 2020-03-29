using System.Collections.Generic;

namespace WebApi.Data.Models
{
    public class Group : IEntity
    {
        public string Name { get; set; }

        public bool Public { get; set; }

        public ICollection<GroupRole> GroupRoles { get; set; } = new HashSet<GroupRole>();

        public ICollection<PassageReference> PassageReferences { get; set; } = new HashSet<PassageReference>();

        public Group(string name)
        {
            Name = name;
        }
    }
}
