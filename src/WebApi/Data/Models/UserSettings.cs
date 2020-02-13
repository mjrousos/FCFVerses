using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Data.Models
{
    public class UserSettings : IEntity
    {
        public Translations? PreferredTranslation { get; set; }

        public string UserId { get; set; }

        public ICollection<GroupRole> GroupRoles { get; set; } = new HashSet<GroupRole>();

        public UserSettings(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new System.ArgumentException("User ID must not be null or empty", nameof(userId));
            }

            UserId = userId;
        }
    }
}
