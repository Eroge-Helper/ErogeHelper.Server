using ErogeHelper.Server.Data;
using ErogeHelper.Server.DataModel;
using ErogeHelper.Server.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ErogeHelper.Server.Controllers
{
    [Route("v1/[controller]")]
    [EnableCors]
    [ApiController]
    public class GameController : ControllerBase
    {
        public GameController(ILogger<GameController> logger, MainDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly ILogger<GameController> _logger;
        private readonly MainDbContext _dbContext;

        [HttpGet("Setting")]
        public async Task<ActionResult<GameQueryResult>> QuerySetting(string md5)
        {
            if (md5.Length != 32)
            {
                _logger.LogError("Query: md5 error length");
                return BadRequest();
            }

            var game = await _dbContext.Games
                .Where(g => g.Md5.Equals(md5))
                .SingleOrDefaultAsync() ?? null;

            if (game is null)
            {
                _logger.LogInformation($"Query: Game '{md5}' not found.");
                return NotFound();
            }
            
            return new GameQueryResult
            {
                Id = game.Id,
                TextSettingJson = game.TextSettingJson,
                RegExp = game.RegExp
            };
        }

        [HttpPost("Setting")]
        public async Task<ActionResult<GameSubmitResult>> SubmitSetting(GameSubmitParams @params)
        {
            var user = await _dbContext.Users
                .Where(it => it.Username == @params.Username && it.Password == @params.Password)
                .SingleOrDefaultAsync() ?? null;

            if (user is null)
                return Unauthorized();

            user.AccessTime = DateTime.UtcNow;

            var game = await _dbContext.Games
                .Where(g => g.Md5.Equals(@params.Md5))
                .Include(g => g.Names)
                .SingleOrDefaultAsync() ?? null;

            if (game is null)
            {
                game = new Game()
                {
                    Md5 = @params.Md5,
                    Names = @params.Names.Select(it => new GameName
                    {
                        Type = it.Type,
                        Value = it.Value
                    }).ToList(),
                    TextSettingJson = @params.TextSetting,
                    RegExp = @params.RegExp,
                    CreatorId = user.Id,
                    Creator = user,
                    CreationTime = DateTime.UtcNow,
                    ModifiedTime = DateTime.UtcNow
                };

                await _dbContext.Games.AddAsync(game);
            }
            else
            {
                foreach (var gameName in @params.Names)
                {
                    if (!game.Names.Any(it => it.Value.Equals(gameName.Value)))
                    {
                        var newGameName = new GameName()
                        {
                            Type = gameName.Type,
                            Value = gameName.Value,
                        };
                        game.Names.Add(newGameName);
                    }
                }

                game.TextSettingJson = @params.TextSetting;
                game.RegExp = @params.RegExp;
                game.ModifiedTime = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync();

            return new GameSubmitResult { Id = game.Id };
        }
    }
}
