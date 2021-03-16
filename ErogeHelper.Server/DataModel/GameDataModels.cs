using System.ComponentModel.DataAnnotations;

namespace ErogeHelper.Server.DataModel
{
    public class GameResultData
    {
        public int Id { get; set; }
        public string TextSettingJson { get; set; } = string.Empty;
    }
}
