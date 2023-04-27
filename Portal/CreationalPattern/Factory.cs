using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.CreationalPattern
{
    public abstract class Factory
    {
        /// <summary>
        /// Method for creating a gameobject
        /// </summary>
        /// <param name="type">of a type based on an enum</param>
        /// <returns>the created gameobject</returns>
        public abstract GameObject Create(Enum type);
    }
}
