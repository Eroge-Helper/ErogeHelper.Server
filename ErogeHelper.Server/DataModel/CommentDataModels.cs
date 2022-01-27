using System;
using System.ComponentModel.DataAnnotations;

namespace ErogeHelper.Server.DataModel;

public class ContextSubmitParams
{
    [Required]
    [StringLength(64)]
    public string Hash { get; set; } = string.Empty;

    [Required]
    [Range(1, 5)]
    public int Size { get; set; }

    [Required]
    [StringLength(1024)]
    public string Content { get; set; } = string.Empty;
}

public class CommentSubmitParams
{
    [Required]
    [StringLength(64)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(64)]
    public string Password { get; set; } = string.Empty;

    public int? GameId { get; set; }

    [StringLength(32)]
    public string GameMd5 { get; set; } = string.Empty;

    [Required]
    public ContextSubmitParams Context { get; set; } = null!;

    [Required]
    [StringLength(16)]
    public string Language { get; set; } = string.Empty;

    [Required]
    [StringLength(1024)]
    public string Text { get; set; } = string.Empty;

    [StringLength(300)]
    public string CreationComment { get; set; } = string.Empty;
}

public class CommentSyncParams
{
    public int? GameId { get; set; }

    [StringLength(32)]
    public string GameMd5 { get; set; } = string.Empty;

    public DateTime LastSyncTime { get; set; }
}

public class CommentSyncResultData
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string UserComment { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }
}
