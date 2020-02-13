using System.Collections.Generic;

namespace WebApi.ViewModels
{
    public class PassageGroup
    {
        public int GroupId { get; }

        public string GroupName { get; }

        public bool IsAdmin { get; }

        public IEnumerable<Passage> Passages { get; }

        public PassageGroup(int groupId, string groupName, bool isAdmin, IEnumerable<Passage> passages)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new System.ArgumentException("Group name must not be null or empty", nameof(groupName));
            }

            GroupId = groupId;
            GroupName = groupName;
            IsAdmin = isAdmin;
            Passages = passages ?? throw new System.ArgumentNullException(nameof(passages));
        }
    }
}
