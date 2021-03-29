using System;
using System.ComponentModel.DataAnnotations;

namespace ErogeHelper.Server.DataModel
{
    public class SubtitleQueryParams
    {
        public int[] GameIds { get; set; } = Array.Empty<int>();

        [StringLength(32)] 
        public string GameMd5 { get; set; } = string.Empty;

        public long UpdateTime;

        public string Language = string.Empty;
    }

    public class QuerySubtitleResult
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Liked { get; set; }
        public int Disliked { get; set; }
        public int CreatorId { get; set; }
        public string CreationSubtitle { get; set; } = string.Empty;
        public long CreationTime { get; set; }
        public int? EditorId { get; set; }
        public string RevisionSubtitle { get; set; } = string.Empty;
        public long RevisionTime { get; set; }
    }

    public class SubtitleSubmitParams
    {
        [Required]
        [StringLength(64)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int GameId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string Hash { get; set; } = string.Empty;

        [Required]
        public int Size { get; set; }

        [Required]
        [StringLength(16)]
        public string Language { get; set; } = string.Empty;

        [Required]
        [StringLength(1024)]
        public string CreationSubtitle { get; set; } = string.Empty;

        [StringLength(300)]
        public string RevisionComment { get; set; } = string.Empty;
    }

    public class SubtitleSubmitResult
    {
        public int Id { get; set; }
    }

    public class SubtitleUpdateParams
    {
        [Required]
        [StringLength(64)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int Id { get; set; }

        public bool Deleted { get; set; }

        [StringLength(300)]
        public string CreationSubtitle { get; set; } = string.Empty;

        [StringLength(300)]
        public string RevisionComment { get; set; } = string.Empty;
    }

    public class SubtitleUpdateResult
    {
        public int Id { get; set; }
    }
}