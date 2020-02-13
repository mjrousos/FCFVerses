namespace WebApi.Data.Models
{
    public class GroupRole
    {
        public Roles Role { get; set; } = default!;

        public Group Group { get; set; } = default!;

        public int GroupId { get; set; }

        public UserSettings UserSettings { get; set; } = default!;

        public int UserSettingsId { get; set; }
    }
}
