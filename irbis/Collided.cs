using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Collided
{
    public int Count
    {
        get
        {
            return
                bottomCollided.Count +
                rightCollided.Count  +
                leftCollided.Count   +
                topCollided.Count    ;
        }
    }

    public int RightmostBottomCollision
    {
        get
        {
            int rightmost = int.MinValue;
            foreach (ICollisionObject c in bottomCollided)
            {
                if (rightmost < c.Collider.Right)
                { rightmost = c.Collider.Right; }
            }
            return rightmost;
        }
    }

    public int LeftmostBottomCollision
    {
        get
        {
            int leftmost = int.MaxValue;
            foreach (ICollisionObject c in bottomCollided)
            {
                if (leftmost > c.Collider.Left)
                { leftmost = c.Collider.Left; }
            }
            return leftmost;
        }
    }

    public List<ICollisionObject> topCollided;
    public List<ICollisionObject> leftCollided;
    public List<ICollisionObject> rightCollided;
    public List<ICollisionObject> bottomCollided;

    public Collided()
    {
        topCollided = new List<ICollisionObject>();
        leftCollided = new List<ICollisionObject>();
        rightCollided = new List<ICollisionObject>();
        bottomCollided = new List<ICollisionObject>();
    }

    public void Add(ICollisionObject objectToAdd, Side side)
    {
        switch (side)
        {
            case Side.Bottom:
                bottomCollided.Add(objectToAdd);
                break;
            case Side.Right:
                rightCollided.Add(objectToAdd);
                break;
            case Side.Left:
                leftCollided.Add(objectToAdd);
                break;
            case Side.Top:
                topCollided.Add(objectToAdd);
                break;
        }
    }

    public bool Remove(ICollisionObject collisionObject, Side side)
    {
        switch (side)
        {
            case Side.Bottom:
                return bottomCollided.Remove(collisionObject);
            case Side.Right:
                return rightCollided.Remove(collisionObject);
            case Side.Left:
                return leftCollided.Remove(collisionObject);
            case Side.Top:
                return topCollided.Remove(collisionObject);
        }
        return false;
    }

    public bool RemoveAll(ICollisionObject collisionObject)
    {
        if (bottomCollided.Contains(collisionObject))
        { bottomCollided.Remove(collisionObject); }
        if (rightCollided.Contains(collisionObject))
        { rightCollided.Remove(collisionObject); }
        if (leftCollided.Contains(collisionObject))
        { leftCollided.Remove(collisionObject); }
        if (topCollided.Contains(collisionObject))
        { topCollided.Remove(collisionObject); }
        return true;
    }

    public bool Intersects(Rectangle rectangle)
    {
        foreach (ICollisionObject b in bottomCollided)
        {
            if (b.Collider.Intersects(rectangle))
            { return true; }
        }
        foreach (ICollisionObject r in rightCollided)
        {
            if (r.Collider.Intersects(rectangle))
            { return true; }
        }
        foreach (ICollisionObject l in leftCollided)
        {
            if (l.Collider.Intersects(rectangle))
            { return true; }
        }
        foreach (ICollisionObject t in topCollided)
        {
            if (t.Collider.Intersects(rectangle))
            { return true; }
        }
        return false;
    }

    public bool Contains(ICollisionObject collisionObject)
    {
        if (bottomCollided.Contains(collisionObject))
        { return true; }
        if (rightCollided.Contains(collisionObject))
        { return true; }
        if (leftCollided.Contains(collisionObject))
        { return true; }
        if (topCollided.Contains(collisionObject))
        { return true; }
        return false;
    }
}
