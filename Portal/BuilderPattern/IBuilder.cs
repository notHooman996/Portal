using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.BuilderPattern
{
    public interface IBuilder
    {
        /// <summary>
        /// for implementation
        /// </summary>
        public void BuildGameObject();

        /// <summary>
        /// for implementation
        /// </summary>
        /// <returns></returns>
        public GameObject GetResult();
    }
}
