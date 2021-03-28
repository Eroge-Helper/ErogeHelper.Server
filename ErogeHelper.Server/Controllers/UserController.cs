using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ErogeHelper.Server.Data;
using ErogeHelper.Server.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using ErogeHelper.Server.DataModel;
using Microsoft.EntityFrameworkCore;

namespace ErogeHelper.Server.Controllers
{
    [Route("v1/[controller]")]
    [EnableCors]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(ILogger<UserController> logger, MainDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly ILogger<UserController> _logger;
        private readonly MainDbContext _dbContext;

        // POST: v1/User/
        [HttpPost]
        public async Task<ActionResult> Register(UserRegisterParams @params)
        {
            var user = await _dbContext.Users
                .Where(it => it.Username.Equals(@params.Username))
                .SingleOrDefaultAsync() ?? null;

            if (user is not null)
            {
                // 用户名已存在
                return BadRequest();
            }

            await _dbContext.Users.AddAsync(new User
            {
                Username = @params.Username,
                Password = @params.Password,
                Email = @params.Email,
                CreationTime = DateTime.UtcNow,
                AccessTime = DateTime.UtcNow,
                ModifiedTime = DateTime.UtcNow
            });

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Session")] // 555 ?
        public async Task<ActionResult<UserInfoResult>> Login(UserLoginParams @params)
        {
            // UNDONE: 也考虑可以使用邮箱登录
            var user = await _dbContext.Users
                .Where(it => it.Username.Equals(@params.Username))
                .SingleOrDefaultAsync() ?? null;

            if (user is null)
                return Unauthorized();

            if (user.Password == string.Empty)
            {
                user.Password = @params.Password;
            }
            else if (!user.Password.Equals(@params.Password))
            {
                return Unauthorized();
            }

            _logger.LogInformation($"User '{user.Username}' logged in.");

            user.AccessTime = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return new UserInfoResult
            {
                Id = user.Id,
                Avatar = user.Avatar,
                Color = user.Color,
                Username = user.Username,
                Language = user.Language,
                Email = user.Email,
                HomePage = user.HomePage,
                ExtraInfo = user.ExtraInfo,
            };
        }

        /// <summary>
        /// 通常资料的更新，不涉及pw和邮箱
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(UserUpdateParams @params)
        {
            var user = await _dbContext.Users
                .Where(it => it.Username.Equals(@params.Username) && it.Password.Equals(@params.Password))
                .SingleOrDefaultAsync() ?? null;

            if (user is null)
                return Unauthorized();

            user.ModifiedTime = DateTime.UtcNow;

            user.Language = @params.Language;
            user.ExtraInfo = @params.ExtraInfo;
            user.Avatar = @params.Avatar;
            user.HomePage = @params.HomePage;
            user.Color = @params.Color;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Email")]
        public async Task<ActionResult> UpdateEmail(UserUpdateEmailParams @params)
        {
            // QUESTION: 参数中带Required标签的字符串值为string.Empty 也是可以的吧
            var user = await _dbContext.Users
                .Where(it => it.Username.Equals(@params.Username) && it.Password.Equals(@params.Password))
                .SingleOrDefaultAsync() ?? null;
            
            if (user is null)
                return Unauthorized();

            user.ModifiedTime = DateTime.UtcNow;
            user.Email = @params.Email;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Update password by sending email
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPut("Secret")]
        public async Task<ActionResult> UpdateSecret(UserUpdateSecretParams @params)
        {
            var user = await _dbContext.Users
                .Where(it => it.Username.Equals(@params.Username) && it.Password.Equals(@params.OldPassword))
                .SingleOrDefaultAsync() ?? null;

            if (user is null)
                return Unauthorized();

            // UNDONE: do update with token
            if (!@params.Token.Equals("sth"))
            {
                return new UnauthorizedResult();
            }
            return new UnauthorizedResult();

            //user.ModifiedTime = DateTime.UtcNow;
            //await _dbContext.SaveChangesAsync();

            //return Ok();
        }
    }
}
