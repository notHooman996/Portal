using Portal.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.CommandPattern
{
    public interface ICommand
    {
        /// <summary>
        /// method for implementation
        /// </summary>
        /// <param name="player"></param>
        public void Execute(Player player);
    }
}
