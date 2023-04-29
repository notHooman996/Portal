using PortalGame.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.CommandPattern
{
    public class JumpCommand : ICommand
    {
        public void Execute(Player player)
        {
            player.Jump(); 
        }
    }
}
