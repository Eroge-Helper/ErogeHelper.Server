using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErogeHelper.Server.Data;
using ErogeHelper.Server.DataModel;
using ErogeHelper.Server.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErogeHelper.Server.Controllers
{
    [Route("v1/[controller]")]
    [EnableCors]
    [ApiController]
    public class SubtitleController : ControllerBase
    {
        public SubtitleController(ILogger<SubtitleController> logger, MainDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly ILogger<SubtitleController> _logger;
        private readonly MainDbContext _dbContext;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuerySubtitleResult>>> Query(SubtitleQueryParams @params)
        {
            if (@params.GameIds.Length == 0)
                return BadRequest();

            List<QuerySubtitleResult> resultData = new();

            if (@params.UpdateTime == 0)
            {
                foreach (var gameId in @params.GameIds)
                {
                    resultData.AddRange(
                        await _dbContext.Subtitles
                            .Where(it =>
                                it.GameId == gameId &&
                                it.Deleted == false &&
                                it.Language.Equals(@params.Language))
                            .Select(it => new QuerySubtitleResult
                            {
                                Id = it.Id,
                                GameId = it.GameId,
                                Hash = it.HashString,
                                Size = it.Size.ToString(),
                                Content = it.Content,
                                Language = it.Language,
                                Liked = it.UpVote,
                                Disliked = it.DownVote,
                                CreatorId = it.CreatorId,
                                CreationSubtitle = it.CreationSubtitle,
                                CreationTime = it.CreationTimeUnix,
                                EditorId = it.EditorId,
                                RevisionSubtitle = it.RevisionSubtitle,
                                RevisionTime = it.RevisionTimeUnix,
                            }).ToArrayAsync());
                }
            }
            else
            {
                foreach (var gameId in @params.GameIds)
                {
                    resultData.AddRange(
                        await _dbContext.Subtitles
                            .Where(it =>
                                it.GameId == gameId &&
                                it.Deleted == false &&
                                it.Language.Equals(@params.Language) &&
                                // QUESTION: Correct?
                                it.RevisionTimeUnix > @params.UpdateTime)
                            .Select(it => new QuerySubtitleResult
                            {
                                Id = it.Id,
                                GameId = it.GameId,
                                Hash = it.HashString,
                                Size = it.Size.ToString(),
                                Content = it.Content,
                                Language = it.Language,
                                Liked = it.UpVote,
                                Disliked = it.DownVote,
                                CreatorId = it.CreatorId,
                                CreationSubtitle = it.CreationSubtitle,
                                CreationTime = it.CreationTimeUnix,
                                EditorId = it.EditorId,
                                RevisionSubtitle = it.RevisionSubtitle,
                                RevisionTime = it.RevisionTimeUnix,
                            }).ToArrayAsync());
                }
            }

            return resultData;
        }

        [HttpPost]
        public async Task<ActionResult<SubtitleSubmitResult>> Submit(SubtitleSubmitParams @params)
        {
            var user = await _dbContext.Users
                .Where(it => it.Username.Equals(@params.Username) && it.Password.Equals(@params.Password))
                .SingleOrDefaultAsync() ?? null;

            if (user is null)
                return Unauthorized();

            user.AccessTime = DateTime.UtcNow;

            var game = await _dbContext.Games
                .Where(it => it.Id == @params.GameId)
                .SingleOrDefaultAsync() ?? null;

            if (game is null)
                return NotFound();

            Subtitle subtitle = new()
            {
                GameId = @params.GameId,
                Hash = Convert.ToInt64(@params.Hash),
                HashString = @params.Hash,
                Size = @params.Size,
                Content = @params.Content,
                CreationSubtitle = @params.CreationSubtitle,
                Language = @params.Language,
                CreatorId = user.Id,
                CreationTime = DateTime.UtcNow,
                CreationTimeUnix = DateTime.UtcNow.ToUnixTimeSeconds(),
                EditorId = user.Id,
                RevisionTime = DateTime.UtcNow,
                RevisionTimeUnix = DateTime.UtcNow.ToUnixTimeSeconds(),
            };

            await _dbContext.Subtitles.AddAsync(subtitle);

            await _dbContext.SaveChangesAsync();

            // 返回Id作为更新字幕的唯一凭证
            return new SubtitleSubmitResult { Id = subtitle.Id };
        }

        [HttpPut]
        public async Task<ActionResult<SubtitleUpdateResult>> Update(SubtitleUpdateParams @params)
        {
            var user = await _dbContext.Users
                .Where(it => it.Username == @params.Username && it.Password == @params.Password)
                .SingleOrDefaultAsync() ?? null;

            if (user is null)
                return Unauthorized();

            user.AccessTime = DateTime.UtcNow;

            var subtitle = await _dbContext.Subtitles
                .Where(it => it.Id == @params.Id)
                .SingleOrDefaultAsync() ?? null;

            if (subtitle is null)
                return NotFound();

            // 只有创建者可以改创建者的字幕
            if (user.Id != subtitle.CreatorId)
                return Unauthorized();

            subtitle.Deleted = (bool)@params.Deleted;
            subtitle.CreationSubtitle = @params.CreationSubtitle;
            subtitle.RevisionSubtitle = @params.RevisionComment;

            subtitle.EditorId = user.Id;
            subtitle.RevisionTime = DateTime.UtcNow;
            subtitle.RevisionTimeUnix = DateTime.UtcNow.ToUnixTimeSeconds();

            await _dbContext.SaveChangesAsync();

            return new SubtitleUpdateResult { Id = subtitle.Id };
        }
    }
}
