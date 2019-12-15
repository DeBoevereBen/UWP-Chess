using GalaSoft.MvvmLight.Command;
using Realdolmen.UWP.Chess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Realdolmen.UWP.Chess.ViewModels
{
    public class BoardViewModel : BaseViewModel<Board>
    {
        public ICommand HandleClickCommand { get; set; }
        public int SelectedTileId { get; set; }
        public ObservableCollection<TileViewModel> Tiles = new ObservableCollection<TileViewModel>();

        public string PlayerTurn
        {
            get => $"{Model.PlayersTurn.ToString()}'s turn";
        }

        public string GameOver
        {
            get => $"Game over. {(Model.GameOver.GameOverType == GameOverType.Stalemate ? GameOverType.Stalemate.ToString() : GameOverType.Checkmate.ToString())} {(Model.GameOver.GameOverType == GameOverType.Stalemate ? "Draw" : Model.GameOver.Winner.ToString() + " won.")}";
        }

        public BoardViewModel() : base(new Board())
        {
            SelectedTileId = -1;
            Model.InitTiles();

            foreach (var t in Model.Tiles.OrderBy(x => x.Location.Y).ThenBy(x => x.Location.X))
            {
                var tileVM = new TileViewModel(t) { IsBlack = (t.Location.Y % 2 == 0 && t.TileId % 2 == 0) || (t.Location.Y % 2 != 0 && t.TileId % 2 != 0) ? false : true };
                Tiles.Add(tileVM);
            }

            HandleClickCommand = new RelayCommand<TileViewModel>(t => HandleClick(t));
        }

        private void HandleClick(TileViewModel t)
        {
            if (SelectedTileId < 0)
            {
                SetSelectedTile(t);
            }
            else
            {
                if (SelectedTileId != t.Id)
                {
                    var current = Model.Tiles.Single(x => x.TileId == SelectedTileId);
                    var target = Model.Tiles.Single(x => x.TileId == t.Id);

                    var res = Model.CanMove(current, target);
                    if (res)
                    {
                        var move = Model.MovePiece(current.Location, target.Location);
                        var affectedTileCurrent = Tiles.Single(x => x.Id == move.CurrentTile.TileId);
                        var affectedTileTarget = Tiles.Single(x => x.Id == move.TargetTile.TileId);
                        affectedTileCurrent.Piece = Model.Tiles.Single(x => x.TileId == affectedTileCurrent.Id).Piece;
                        affectedTileTarget.Piece = Model.Tiles.Single(x => x.TileId == affectedTileTarget.Id).Piece;
                        var orgCurrent = Tiles.Single(x => x.Id == current.TileId);
                        var orgTarget = Tiles.Single(x => x.Id == target.TileId);

                        orgCurrent.Piece = Model.Tiles.Single(x => x.TileId == orgCurrent.Id).Piece;
                        orgTarget.Piece = Model.Tiles.Single(x => x.TileId == orgTarget.Id).Piece;
                    }
                } else
                {
                    t.Piece = Model.Tiles.Single(mt => mt.TileId == t.Id)?.Piece;
                }
                foreach (var vmTile in Tiles)
                {
                    vmTile.IsAvailableMove = false;
                    vmTile.IsSelected = false;
                }
                SelectedTileId = -1;
            }
            RaisePropertyChanged(nameof(GameOver));
            RaisePropertyChanged(nameof(Model));
            RaisePropertyChanged(nameof(Model.GameOver));
            RaisePropertyChanged(nameof(PlayerTurn));
        }

        private void SetSelectedTile(TileViewModel tile)
        {
            if (tile.Piece?.Color == Model.PlayersTurn)
            {
                SelectedTileId = tile.Id;
                tile.IsSelected = true;
                var tileIds = Model.GetAvailableTileIds(tile.Id);
                foreach (var _tile in Tiles)
                {
                    if (tileIds.Contains(_tile.Id))
                    {
                        _tile.IsAvailableMove = true;
                    }
                }
            }
        }

    }
}
