﻿using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessView.View
{
    public interface IObject2D: IDisposable
    {
        Sprite ObjectSprite
        {
            get;
        }

        Vector2f Position
        {
            get;
            set;
        }

        void DrawIn(RenderWindow window);

        // Part animations
        void SetCanevas(IntRect newCanevas);

        void SetZoom(float newZoom);
    }
}
