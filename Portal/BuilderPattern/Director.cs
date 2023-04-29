using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.BuilderPattern
{
    public class Director
    {
        private IBuilder builder;

        /// <summary>
        /// constructor which takes 1 parameter
        /// </summary>
        /// <param name="builder">what builder to make</param>
        public Director(IBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Constructs the builder and builds the gameobject
        /// </summary>
        /// <returns>the gameobject that was built</returns>
        public GameObject Construct()
        {
            builder.BuildGameObject();

            return builder.GetResult();
        }
    }
}
