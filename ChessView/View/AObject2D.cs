using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessView.Animation;
using ChessView.View.Helpers;
using SFML.Graphics;
using SFML.System;

namespace ChessView.View
{
    public abstract class AObject2D : IObject2D
    {
        protected static RectangleShape filter;

        protected static AnimationManager animationManager;

        protected Sprite sprite;

        public Sprite ObjectSprite
        {
            get
            {
                return this.sprite;
            }

            protected set
            {
                this.sprite = value;
            }
        }

        public Vector2f Position
        {
            get
            {
                return this.ObjectSprite.Position;
            }

            set
            {
                this.ObjectSprite.Position = value;
            }
        }

        static AObject2D()
        {
            AObject2D.animationManager = new AnimationManager();
        }

        public AObject2D()
        {
            this.sprite = new Sprite();
        }       

        public virtual void Dispose()
        {
            
        }

        public virtual void DrawIn(RenderWindow window)
        {
            //float ratioAltitude = 1 - Math.Abs(this.ratioAltitude);
            //byte colorAltitude = (byte)(ratioAltitude * ratioAltitude * 255f);

            //this.ObjectSprite.Color = new Color(colorAltitude, colorAltitude, colorAltitude, this.ObjectSprite.Color.A);

            window.Draw(this.ObjectSprite);
        }

        // Part animations.
        public void PlayPositionAnimation(float durationSec, Vector2f toPosition)
        {
            IAnimation animation = new PositionAnimation(this.Position, toPosition, Time.FromSeconds(durationSec), AnimationType.ONETIME, InterpolationMethod.SIGMOID);

            AObject2D.animationManager.PlayAnimation(this, animation);
        }

        public static void UpdateAnimationManager(Time deltaTime)
        {
            AObject2D.animationManager.Run(deltaTime);
        }

        public virtual void SetCanevas(IntRect newCanevas)
        {
            this.sprite.TextureRect = newCanevas;
        }

        public void SetZoom(float newZoom)
        {
            this.sprite.Scale = new Vector2f(newZoom, newZoom);
        }
    }
}
