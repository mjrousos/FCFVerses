namespace WebApi.Data.Models
{
    public enum Roles
    {
        /// <summary>
        /// Group owner with permissions to add/remove verses
        /// </summary>
        Admin,

        /// <summary>
        /// Group member with permission to view verses
        /// </summary>
        Member
    }
}
