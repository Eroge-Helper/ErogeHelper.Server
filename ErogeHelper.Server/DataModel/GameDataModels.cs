using System;
using System.ComponentModel.DataAnnotations;

namespace ErogeHelper.Server.DataModel
{
    public class GameQueryResult
    {
        public int Id { get; set; }
        public string TextSettingJson { get; set; } = string.Empty;
        public string RegExp { get; set; } = string.Empty;
    }

    public class GameSubmitParams
    {
        [Required] 
        [StringLength(64)] 
        public string Username { get; set; } = string.Empty;

        [Required] 
        [StringLength(64)] 
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(32)]
        public string Md5 { get; set; } = string.Empty;

        [Required] 
        public GameNameSubmitParams[] Names { get; set; } = Array.Empty<GameNameSubmitParams>();

        [Required] 
        public string TextSetting { get; set; } = string.Empty;

        public string RegExp { get; set; } = string.Empty;
    }

    public class GameNameSubmitParams
    {
        [Required]
        [StringLength(32)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [StringLength(1024)]
        public string Value { get; set; } = string.Empty;
    }

    public class GameSubmitResult
    {
        public int Id { get; set; }
    }
}
