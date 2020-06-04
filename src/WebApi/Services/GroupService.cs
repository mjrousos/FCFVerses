using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Data;
using WebApi.Data.Models;
using WebApi.Models;
using WebApi.ViewModels;

namespace WebApi.Services
{
    public class GroupService : IGroupService
    {
        private VersesDbContext DbContext { get; }

        private IVerseService VerseService { get; }

        private ILogger<GroupService> Logger { get; }

        public GroupService(VersesDbContext dbContext, IVerseService verseService, ILogger<GroupService> logger)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            VerseService = verseService ?? throw new ArgumentNullException(nameof(verseService));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> AddPassageAsync(int groupId, PassageReference passageReference)
        {
            if (passageReference is null)
            {
                throw new ArgumentNullException(nameof(passageReference));
            }

            Logger.LogInformation("Adding passage {PassageReference} to group {GroupId}", passageReference, groupId);

            var group = await DbContext.Groups.FindAsync(groupId);

            if (group is null)
            {
                Logger.LogWarning("Could not add passage reference {PassageReference} to group {GroupId} because the group does not exist", passageReference, groupId);
                return false;
            }

            await DbContext.Entry(group).Collection(g => g.PassageReferences).LoadAsync();

            if (group.PassageReferences.Any(p => passageReference == p))
            {
                Logger.LogInformation("Could not add passage reference {PassageReference} to group {GroupId} because the group already contains that reference", passageReference, groupId);
                return false;
            }

            // Create a new reference since a specific passage reference instance should only belong to a single group.
            // This guards against a passage from another group being passed in and added.
            group.PassageReferences.Add(new PassageReference(passageReference));
            DbContext.Groups.Update(group);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Added passage {PassageReference} to group {GroupId}", passageReference, groupId);
            return true;
        }

        public async Task<bool> RemovePassageAsync(int groupId, int passageId)
        {
            Logger.LogInformation("Removing passage {PassageReferenceId} from group {GroupId}", passageId, groupId);

            var group = await DbContext.Groups.FindAsync(groupId);

            if (group is null)
            {
                Logger.LogWarning("Could not remove passage {PassageReferenceId} from group {GroupId} because the group does not exist", passageId, groupId);
                return false;
            }

            await DbContext.Entry(group).Collection(g => g.PassageReferences).LoadAsync();

            var passage = group.PassageReferences.FirstOrDefault(p => p.Id == passageId);

            if (passage is null)
            {
                Logger.LogInformation("Could not remove passage {PassageReferenceId} from group {GroupId} because the group does not contain that reference", passageId, groupId);
                return false;
            }

            group.PassageReferences.Remove(passage);
            DbContext.Groups.Update(group);
            DbContext.PassageReferences.Remove(passage);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Removed passage {PassageReferenceId} from group {GroupId}", passageId, groupId);
            return true;
        }

        public async Task<PassageGroup?> GetPassagesAsync(int groupId, Translations translation, bool admin)
        {
            Logger.LogInformation("Getting passages for group {GroupId}", groupId);

            var group = await DbContext.Groups.FindAsync(groupId);
            await DbContext.Entry(group).Collection(g => g.PassageReferences).LoadAsync();

            if (group is null)
            {
                Logger.LogWarning("Could not retrieve passages for group {GroupId} because the group does not exist", groupId);
                return null;
            }

            var passageTasks = group.PassageReferences.OrderByDescending(r => r.CreatedDate).Select(r => GetPassageAsync(r, translation));
            var passages = await Task.WhenAll(passageTasks);

            Logger.LogInformation("Retrieved {PassageCount} passages for group {GroupId}", passages.Length, groupId);
            return new PassageGroup(groupId, group.Name, admin, passages);
        }

        private async Task<Passage> GetPassageAsync(PassageReference reference, Translations translation)
        {
            var referenceString = reference?.ToString();
            if (reference == null || string.IsNullOrEmpty(referenceString))
            {
                Logger.LogError("Could not resolve passage references. Invalid reference {@PassageReference}", reference);
                throw new ArgumentException("Invalid passage reference", nameof(reference));
            }

            var passageVerses = await VerseService.GetVersesAsync(reference.Verses, translation);
            Logger.LogInformation("Retrieved {VerseCount} verses for {PassageReference}", passageVerses.Count(), reference);

            var text = string.Join(' ', passageVerses.Select(v => v.Text));
            if (reference.StartOffset > 0 || reference.EndOffset > 0)
            {
                var tokens = text.Split();
                text = string.Join(' ', tokens, reference.StartOffset, tokens.Length - (reference.StartOffset + reference.EndOffset));
            }

            return new Passage(reference.Id, referenceString, text, translation.ToString());
        }

        public async Task<IEnumerable<GroupViewModel>> GetAllPublicGroupsAsync() =>
            (await DbContext.Groups.Where(g => g.Public).ToArrayAsync()).Select(g => new GroupViewModel(g.Id, g.Name));

        public async Task<IEnumerable<GroupViewModel>> GetGroupsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                Logger.LogError("Could not get groups for user; invalid user ID {UserId}", userId);
                throw new ArgumentException("User ID must not be null or empty", nameof(userId));
            }

            var groups = await DbContext.GroupRoles
                .Include(r => r.UserSettings)
                .Where(r => string.Equals(r.UserSettings.UserId, userId))
                .Select(r => r.Group)
                .ToListAsync();

            Logger.LogInformation("Retrieved {GroupCount} groups for user {UserId}", groups.Count, userId);

            return groups.Select(g => new GroupViewModel(g.Id, g.Name));
        }
    }
}
