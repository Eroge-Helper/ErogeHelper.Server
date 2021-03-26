using ErogeHelper.Server.Data;
using ErogeHelper.Server.DataModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        public async Task<ActionResult<GameResultData>> QuerySetting(string md5)
        {
            if (md5.Length != 32)
            {
                _logger.LogError("Query: Missing game md5 or error length");
                return NotFound();
            }

            var game = await _dbContext.Games
                .Where(it => it.Md5 == md5)
                .FirstOrDefaultAsync();

            if (game is null)
            {
                _logger.LogInformation("Query: Game '{1}' not found.", md5);
                return NotFound();
            }

            await _dbContext.Entry(game)
                .Collection(it => it.Names).LoadAsync();

            return new GameResultData
            {
                Id = game.Id,
                TextSettingJson = game.TextSettingJson,
            };
        }
    }
}
