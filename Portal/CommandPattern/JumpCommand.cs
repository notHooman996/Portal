﻿using Portal.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.CommandPattern
{
    public class JumpCommand : ICommand
    {
        public void Execute(Player player)
        {
            player.Jump(); 
        }
    }
}