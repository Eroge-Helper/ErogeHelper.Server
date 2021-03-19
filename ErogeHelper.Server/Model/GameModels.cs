using System;
using System.Collections.Generic;

namespace ErogeHelper.Server.Model
{
    public class Game
    {
        public int Id { get; set; }
        public string Md5 { get; set; } = string.Empty;
        public List<GameName> Names { get; set; } = new();
        public int TextSettingId { get; set; }// TODO: Delete this
        public string TextSettingJson { get; set; } = string.Empty;
        public int CreatorId { get; set; }
        public User Creator { get; set; } = new();
        public DateTime CreationTime { get; set; }
        public DateTime ModifiedTime { get; set; }
    }

    public class GameName
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}