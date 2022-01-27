using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErogeHelper.Server.Data;
using ErogeHelper.Server.DataModel;
using ErogeHelper.Server.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErogeHelper.Server.Controllers;

[Route("v1/[controller]")]
[EnableCors]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly MainDbContext _dbContext;

    public CommentController(ILogger<CommentController> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost("Submit")]
    public async Task<ActionResult> SendComment(CommentSubmitParams @params)
    {
        var user = await _dbContext.Users
            .Where(it => it.Username == @params.Username && it.Password == @params.Password)
            .SingleOrDefaultAsync() ?? null;

        if (user is null)
            return Unauthorized();

        user.AccessTime = DateTime.UtcNow;

        Game? game = null;

        if (@params.GameId != null)
        {
            game = await _dbContext.Games
                .Where(it => it.Id == @params.GameId)
                .FirstOrDefaultAsync();
        }
        else if (@params.GameMd5 != null && @params.GameMd5.Length == 32)
        {
            game = await _dbContext.Games
                .Where(it => it.Md5 == @params.GameMd5)
                .FirstOrDefaultAsync();
        }
        else
        {
            return NotFound();
        }

        var currentTime = DateTime.UtcNow;
        Comment comment = new()
        {
            GameId = game.Id,
            Context = new Context
            {
                Hash = @params.Context.Hash,
                Size = @params.Context.Size,
                Content = @params.Context.Content
            },
            Language = @params.Language,
            Text = @params.Text,
            UserComment = @params.CreationComment,
            CreatorId = user.Id,
            CreationTime = currentTime,
            UpdateTime = currentTime
        };

        _dbContext.Comments.Add(comment);

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("Sync")]
    public async Task<ActionResult<IEnumerable<CommentSyncResultData>>> SyncComment(CommentSyncParams @params)
    {
        Game? game = null;
        if (@params.GameId != null)
        {
            game = await _dbContext.Games
                .Where(it => it.Id == @params.GameId)
                .FirstOrDefaultAsync();
        }
        else if (@params.GameMd5 != null && @params.GameMd5.Length == 32)
        {
            game = await _dbContext.Games
                .Where(it => it.Md5 == @params.GameMd5)
                .FirstOrDefaultAsync();
        }
        else
        {
            return BadRequest();
        }

        return await _dbContext.Comments
            .Where(it => it.GameId == @params.GameId
                && it.UpdateTime > @params.LastSyncTime
                && it.Deleted == false)
            .Select(it => new CommentSyncResultData
            {
                Id = it.Id,
                GameId = it.GameId,
                Text = it.Text,
                UserComment = it.UserComment,
                CreationTime = it.CreationTime,
            }).ToArrayAsync();
    }

}

