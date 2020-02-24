using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.Data;
using WebApi.Data.Models;
using WebApi.Models;

namespace WebApi.Services
{
    /// <summary>
    /// Services for interacting with users and user settings.
    /// </summary>
    public class UserService
    {
        private const string DefaultTranslationKey = "DefaultTranslation";

        private IConfiguration Configuration { get; }

        private VersesDbContext DbContext { get; }

        private ILogger<UserService> Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="dbContext">A DbContext for interacting with the app database.</param>
        /// <param name="configuration">App configuration, including default Bible translation to use.</param>
        public UserService(VersesDbContext dbContext, IConfiguration configuration, ILogger<UserService> logger)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieve the preferred Bible translation for a user.
        /// </summary>
        /// <param name="userId">The user to find a preferred translation for.</param>
        /// <returns>The translation the user has selected.</returns>
        public async Task<Translations> GetPreferredTranslationAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("userId must not be null or empty", nameof(userId));
            }

            Logger.LogInformation("Getting preferred translation for user {UserId}", userId);

            var user = await GetOrCreateUserAsync(userId);

            var ret = user.PreferredTranslation ?? Enum.Parse<Translations>(Configuration[DefaultTranslationKey]);
            Logger.LogInformation("Found preferred translation {Translation} for user {UserId}", ret, userId);

            return ret;
        }

        /// <summary>
        /// Retrieve group roles for the specified user.
        /// </summary>
        /// <param name="userId">The user to retrieve roles for.</param>
        /// <returns>Group roles assigned to the specified user.</returns>
        public async Task<IEnumerable<GroupRole>> GetGroupRolesAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("userId must not be null or empty", nameof(userId));
            }

            Logger.LogInformation("Getting group roles for user {UserId}", userId);

            var user = await GetOrCreateUserAsync(userId);
            await DbContext.Entry(user).Collection(u => u.GroupRoles).LoadAsync();

            Logger.LogInformation("Retrieved {RoleCount} roles for user {UserId}", user.GroupRoles.Count, userId);

            return user.GroupRoles;
        }

        /// <summary>
        /// Determine whether the specified user is a global admin.
        /// </summary>
        /// <param name="userId">The user to check global admin status for.</param>
        /// <returns>Whether the specified user is a global admin. False if the user does not exist.</returns>
        public async Task<bool> IsGlobalAdmin(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("userId must not be null or empty", nameof(userId));
            }

            Logger.LogInformation("Checking global adming status for {UserId}", userId);

            var user = await DbContext.UserSettings.Where(s => s.UserId.Equals(userId)).FirstOrDefaultAsync();
            var globalAdmin = user?.IsGlobalAdmin ?? false;

            Logger.LogInformation("User {UserId} global admin status: {GlobalAdmin}", userId, globalAdmin);

            return globalAdmin;

        }

        private async Task<UserSettings> GetOrCreateUserAsync(string userId)
        {
            var user = await DbContext.UserSettings.Where(s => s.UserId.Equals(userId)).FirstOrDefaultAsync();
            if (user == null)
            {
                user = await CreateUserAsync(userId);
            }

            return user;
        }

        private async Task<UserSettings> CreateUserAsync(string userId)
        {
            Logger.LogInformation("Creating new user settings for user {UserId}", userId);
            var settings = new UserSettings(userId)
            {
                PreferredTranslation = Enum.Parse<Translations>(Configuration[DefaultTranslationKey])
            };

            var personalGroup = new Group("Personal");
            settings.GroupRoles.Add(new GroupRole { Group = personalGroup, UserSettings = settings, Role = Roles.Admin });

            await DbContext.UserSettings.AddAsync(settings);
            await DbContext.SaveChangesAsync();

            var ret = await DbContext.UserSettings.Where(s => s.UserId.Equals(userId)).FirstAsync();
            Logger.LogInformation("User settings for user {UserId} created with ID {Id}", userId, ret.Id);

            return ret;
        }
    }
}
