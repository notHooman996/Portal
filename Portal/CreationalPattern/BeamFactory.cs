using Microsoft.Xna.Framework;
using PortalGame.ComponentPattern;
using PortalGame.ComponentPattern.Beams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.CreationalPattern
{
    public enum BeamType
    {
        Red, 
        Blue 
    }

    public class BeamFactory : Factory
    {
        private static BeamFactory instance;

        public static BeamFactory Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new BeamFactory();
                }
                return instance; 
            }
        }

        private GameObject redPrototype;
        private GameObject bluePrototype; 

        private BeamFactory()
        {
            CreateRedPrototype();
            CreateBluePrototype(); 
        }

        private void CreateRedPrototype()
        {
            redPrototype = new GameObject();
            SpriteRenderer spriteRenderer = (SpriteRenderer)redPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Beam\\redportal_beam");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;
            spriteRenderer.Color = new Color(255, 255, 255);
            redPrototype.AddComponent(new RedBeam());
            redPrototype.AddComponent(new Collider());
        }

        private void CreateBluePrototype()
        {
            bluePrototype = new GameObject();
            SpriteRenderer spriteRenderer = (SpriteRenderer)bluePrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Beam\\blueportal_beam");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;
            spriteRenderer.Color = new Color(255, 255, 255);
            bluePrototype.AddComponent(new BlueBeam());
            bluePrototype.AddComponent(new Collider());
        }

        public override GameObject Create(Enum type)
        {
            GameObject gameObject = new GameObject();
            Collider collider; 

            switch (type)
            {
                case BeamType.Red:
                    gameObject = (GameObject)redPrototype.Clone();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    RedBeam redBeam = gameObject.GetComponent<RedBeam>() as RedBeam;
                    collider.CollisionEvent.Attach(redBeam);
                    redBeam.BeamType = BeamType.Red; 
                    break;
                case BeamType.Blue:
                    gameObject = (GameObject)bluePrototype.Clone();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    BlueBeam blueBeam = gameObject.GetComponent<BlueBeam>() as BlueBeam;
                    collider.CollisionEvent.Attach(blueBeam);
                    blueBeam.BeamType = BeamType.Blue; 
                    break; 
            }

            return gameObject; 
        }
    }
}
