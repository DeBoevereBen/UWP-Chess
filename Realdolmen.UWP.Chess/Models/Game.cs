using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        public virtual Board Board { get; set; }
        public int? BoardId { get; set; }
        public virtual Player PlayerWhite { get; set; }
        public int? PlayerWhiteId { get; set; }
        public virtual Player PlayerBlack { get; set; }
        public int? PlayerBlackId { get; set; }

        public Game()
        {
        }
    }
}
