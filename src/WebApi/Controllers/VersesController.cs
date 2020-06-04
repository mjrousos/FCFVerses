using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Data.Models;
using WebApi.Models;
using WebApi.Services;
using WebApi.Utilities;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class VersesController : ControllerBase
    {
        private ILogger<VersesController> Logger { get; }

        private IVerseService VerseService { get; }

        private IUserService UserService { get; }

        private IGroupService GroupService { get; }

        public VersesController(IVerseService verseService, IUserService userService, IGroupService groupService, ILogger<VersesController> logger)
        {
            VerseService = verseService ?? throw new System.ArgumentNullException(nameof(verseService));
            UserService = userService ?? throw new System.ArgumentNullException(nameof(userService));
            GroupService = groupService ?? throw new System.ArgumentNullException(nameof(groupService));
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<PassageGroupList>> GetAllPassagesAsync()
        {
            var currentUserId = HttpContext.GetCurrentUserId();
            if (currentUserId is null)
            {
                return Unauthorized();
            }

            var groups = await UserService.GetGroupRolesAsync(currentUserId, Roles.Member);
            var translation = await UserService.GetPreferredTranslationAsync(currentUserId);
            var admin = await UserService.IsGlobalAdminAsync(currentUserId);
            var passageRetrievalTasks = groups.Select(g => GroupService.GetPassagesAsync(g.GroupId, translation, admin || g.Role == Roles.Admin));
            var passageGroups = await Task.WhenAll(passageRetrievalTasks);
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types. Null items are removed, but Roslyn doesn't recognize that.
            return Ok(new PassageGroupList(translation.GetCopyrightNotice(), passageGroups.Where(g => g != null)));
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        }

        [HttpGet("{groupId}")]
        public async Task<ActionResult<PassageGroup>> GetPassagesAsync(int groupId)
        {
            var currentUserId = HttpContext.GetCurrentUserId();
            if (currentUserId is null)
            {
                return Unauthorized();
            }

            var role = await UserService.GetUserRoleAsync(currentUserId, groupId);
            if (role is null)
            {
                return NotFound();
            }

            var translation = await UserService.GetPreferredTranslationAsync(currentUserId);
            var admin = await UserService.IsGlobalAdminAsync(currentUserId);

            return Ok(await GroupService.GetPassagesAsync(groupId, translation, admin || role == Roles.Admin));
        }

        [HttpPut("{groupId}")]
        public async Task<ActionResult> AddPassageAsync(int groupId, [FromBody] PassageReference passage)
        {
            if (passage.Chapter < 1 || passage.Verse < 1 || passage.Length < 1 || passage.StartOffset < 0 || passage.EndOffset < 0)
            {
                return BadRequest();
            }

            var currentUserId = HttpContext.GetCurrentUserId();
            if (currentUserId is null)
            {
                return Unauthorized();
            }

            var role = await UserService.GetUserRoleAsync(currentUserId, groupId);
            if (role is null)
            {
                return NotFound();
            }
            else if (role > Roles.Admin)
            {
                return Forbid();
            }

            return await GroupService.AddPassageAsync(groupId, passage)
                ? (ActionResult)Ok()
                : Conflict();
        }

        [HttpDelete("{groupId}/{passageId}")]
        public async Task<ActionResult> DeletePassageAsync(int groupId, int passageId)
        {
            var currentUserId = HttpContext.GetCurrentUserId();
            if (currentUserId is null)
            {
                return Unauthorized();
            }

            var role = await UserService.GetUserRoleAsync(currentUserId, groupId);
            if (role is null)
            {
                return NotFound();
            }
            else if (role > Roles.Admin)
            {
                return Forbid();
            }

            return await GroupService.RemovePassageAsync(groupId, passageId)
                ? (ActionResult)Ok()
                : NotFound();
        }

        [HttpGet("passage/{book}/{chapter}/{verse}")]
        public async Task<ActionResult<Passage>> GetPassageAsync(Books book, byte chapter, byte verse, [FromQuery] byte length = 1, [FromQuery] Translations translation = Translations.KJV)
        {
            if (chapter == 0 || verse == 0 || length == 0)
            {
                return BadRequest();
            }

            var passageRef = new PassageReference(book, chapter, verse, length, 0, 0);
            var verses = await VerseService.GetVersesAsync(passageRef.Verses, translation);

            return Ok(verses);
        }
    }
}
