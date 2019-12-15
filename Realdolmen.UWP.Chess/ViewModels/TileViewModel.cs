using Realdolmen.UWP.Chess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Realdolmen.UWP.Chess.ViewModels
{
    public class TileViewModel : BaseViewModel<Tile>
    {
        public ChessPiece Piece
        {
            get => Model.Piece;
            set
            {
                SetProperty(Model.Piece, value, () => Model.Piece = value);
                RaisePropertyChanged(nameof(PieceName));
                RaisePropertyChanged(nameof(ImageSrc));
            }
        }

        public string PieceName => Piece == null ? string.Empty : Piece.Name.ToString();

        public Coordinate Location
        {
            get => Model.Location;
            set => SetProperty(Model.Location, value, () => Model.Location = value);
        }

        public int Id
        {
            get => Model.TileId;
            set => SetProperty(Model.TileId, value, () => Model.TileId = value);
        }

        public bool IsBlack
        {
            get => Model.IsBlack;
            set => SetProperty(Model.IsBlack, value, () => Model.IsBlack = value);
        }

        public bool IsSelected
        {
            get => Model.IsSelected;
            set
            {
                SetProperty(Model.IsSelected, value, () => Model.IsSelected = value);
                RaisePropertyChanged(nameof(Bools));
            }
        }

        public string ImageSrc
        {
            get => Piece != null ? $"/Assets/Pieces/{Piece.Name.ToString().ToLower()}_{Piece.Color.ToString().ToLower()}.png" : $"/Assets/Pieces/empty.png";
        }


        public bool IsAvailableMove
        {
            get => Model.IsAvailableMove;
            set
            {
                SetProperty(Model.IsAvailableMove, value, () => Model.IsAvailableMove = value);
                RaisePropertyChanged(nameof(Bools));
            }
        }

        public Bools Bools
        {
            get => new Bools { IsAvailableMove = this.IsAvailableMove, IsSelected = this.IsSelected };
        }

        public TileViewModel(Tile model) : base(model)
        {
        }
    }


    public struct Bools
    {
        public bool IsSelected { get; set; }
        public bool IsAvailableMove { get; set; }
    }
}
