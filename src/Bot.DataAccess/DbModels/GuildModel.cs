using System.ComponentModel.DataAnnotations;

namespace Bot.DataAccess.DbModels
{
    public class GuildModel
    {
        [Key]
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        [MaxLength(25)]
        public string Prefix { get; set; }
        public ulong GreetingsChannelId { get; set; }
    }
}