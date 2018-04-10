using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public struct ObjectCollided
{
    public ICollisionObject collisionObject;
    public Side collisionSide;

    public ObjectCollided(ICollisionObject objectCollided, Side sideCollided)
	{
        collisionObject = objectCollided;
        collisionSide = sideCollided;
    }
}
