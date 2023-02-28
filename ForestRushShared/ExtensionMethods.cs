using Psilibrary.SpriteClasses;
using Psilibrary.TileEngine;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ForestRushShared
{
    public static class ExtensionMethods
    {
        public static void LockToSprite(this Camera camera, FrameAnimatedSprite sprite, TileMap map)
        {
                camera.Position = new((sprite.Position.X + 128 / 2)
                    - (Engine.TargetWidth / 2), (sprite.Position.Y + 128 / 2)
                    - (Engine.TargetHeight / 2));

                camera.LockCamera(map);
        }
    }
}
