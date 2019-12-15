using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml.Media;

namespace Realdolmen.UWP.Chess.Models
{
    [Serializable]
    public class Tile
    {
        public int TileId { get; set; }
        public virtual Coordinate Location { get; set; }
        public virtual ChessPiece Piece { get; set; }
        public bool IsFree { get => Piece == null; }
        public bool IsBlack { get; set; }
        public bool IsSelected { get; set; }
        public bool IsAvailableMove { get; set; }
    }
}