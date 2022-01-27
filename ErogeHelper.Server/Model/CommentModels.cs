using System;
using Microsoft.EntityFrameworkCore;

namespace ErogeHelper.Server.Model;

public class Comment
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

    public string Text { get; set; } = string.Empty;

    public string UserComment { get; set; } = string.Empty;

    public Context Context { get; set; } = new();

    /// <summary>
    /// Foreign key
    /// </summary>
    public int CreatorId { get; set; }

    /// <summary>
    /// Navigation Property
    /// </summary>
    public User Creator { get; set; } = new();

    public string Language { get; set; } = string.Empty;

    public int UpVote { get; set; }

    public int DownVote { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime UpdateTime { get; set; }

    public bool Deleted { get; set; }
}

/// <summary>
/// Text context of the comment.
/// </summary>
[Owned]
public class Context
{
    public string Hash { get; set; } = string.Empty;

    /// <summary>
    /// Suggest size by vnr
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// 元のText Composition
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
