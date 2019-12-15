using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public Color Color { get; set; }
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }
}
