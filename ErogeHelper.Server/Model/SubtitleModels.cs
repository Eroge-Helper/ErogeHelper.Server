using System;
using Microsoft.EntityFrameworkCore;

namespace ErogeHelper.Server.Model
{
    public class Subtitle
    {
        public int Id { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Navigation Property
        /// </summary>
        public Game Game { get; set; } = new();

        #region Original Text Info
        public long Hash { get; set; }

        /// <summary>
        /// Suggest size by vnr
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 元のText
        /// </summary>
        public string Content { get; set; } = string.Empty;
        # endregion Original Text Info

        /// <summary>
        /// Foreign key
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Navigation Property
        /// </summary>
        public User Creator { get; set; } = new();

        public string Language { get; set; } = string.Empty;

        public string CreationSubtitle { get; set; } = string.Empty;

        public int UpVote { get; set; }

        public int DownVote { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public int? EditorId { get; set; }

        /// <summary>
        /// Navigation Property
        /// </summary>
        public User Editor { get; set; } = new();

        public string RevisionSubtitle { get; set; } = string.Empty;

        public DateTime RevisionTime { get; set; }

        public bool Deleted { get; set; }
    }
}