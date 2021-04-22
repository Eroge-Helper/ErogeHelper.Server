using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErogeHelper.Server.Model
{
    /// <summary>
    /// Table
    /// </summary>
    public class User
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The permissions that this user has.
        /// </summary>
        public bool Permissions { get; set; }

        /// <summary>
        /// The user's login name.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The user's login password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Other information about the user.
        /// </summary>
        public string ExtraInfo { get; set; } = string.Empty;

        /// <summary>
        /// The language used by this user.
        /// </summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// The user's avatar on avatars.io (file token).
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// The user's homepage url.
        /// </summary>
        public string HomePage { get; set; } = string.Empty;

        /// <summary>
        /// The color used by the user's subtitles or comments. (Hexadecimal color code)
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// </summary>
        public DateTime AccessTime { get; set; }

        /// <summary>
        /// </summary>
        public DateTime ModifiedTime { get; set; }

        public bool TermLevel { get; set; }

        public bool SubtitleLevel { get; set; }
    }
}
