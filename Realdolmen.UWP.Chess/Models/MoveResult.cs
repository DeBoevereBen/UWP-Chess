using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    [Serializable]
    public enum MoveResult
    {
        CheckIfObstructed,
        CannotMove,
        CanMove,
        Promote,
        CheckIfCanCastle
    }
}
