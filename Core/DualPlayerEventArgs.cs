using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.Core
{
    public class DualPlayerEventArgs : EventArgs
    {
        public Player Player1 { get; }
        public Player Player2 { get; }

        public DualPlayerEventArgs(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
        }
    }
}
