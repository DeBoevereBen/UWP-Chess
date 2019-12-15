using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    public class Move
    {
        [Key]
        public int Id { get; set; }
        public Tile CurrentTile { get; set; }
        public Tile TargetTile { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Color CurrentPlayersTurn { get; set; }
        [JsonConverter(typeof(StringEnumConverter))] 
        public Color NextPlayersTurn { get; set; }

        public bool IsCastlingMove { get; set; }
    }
}
