using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;
using System.Reflection;

[Serializable]
public class Trigger
{
    public Rectangle Collider
    {
        get
        { return collider; }
    }

    public int repeat;
    private string serializedFunction = null;
    private int[] rect;
    private string inst;
    private Type type;

    [NonSerialized]
    private MethodInfo function;
    [NonSerialized]
    private Rectangle collider;
    [NonSerialized]
    private object instance;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Function">function to run upon triggering (function must accept a passed object)</param>
    /// <param name="Instance">Instance of the function to invoke. Usually "this"</param>
    /// <param name="Area">Collider area</param>
    /// <param name="Count">How many times can this trigger run. Negative for infinte</param>
    public Trigger(MethodInfo Function, string Instance, Rectangle Area, int Count)
	{
        function = Function;
        inst = Instance;
        instance = Irbis.Irbis.game.GetType().GetField(inst).GetValue(Irbis.Irbis.game);
        collider = Area;
        repeat = Count;
	}

    public void Update(ICollisionObject CollisionObject)
    {
        if (repeat != 0)
        {
            if (collider.Intersects(CollisionObject.Collider))
            {
                function.Invoke(instance, new object[]{CollisionObject});
                if (repeat > 0)
                { repeat--; }
            }
        }
    }

    public bool Check()
    {
        bool check = true;
        if (function == null)
        {
            function = type.GetMethod(serializedFunction);
            if (function == null)
            { check = false; }
        }
        if (instance == null)
        {
            instance = Irbis.Irbis.game.GetType().GetField(inst).GetValue(Irbis.Irbis.game);
            if (instance == null)
            { check = false; Irbis.Irbis.WriteLine("inst:" + inst); }
        }
        return check;
    }

    [OnSerializing]
    internal void OnSerializingMethod(StreamingContext context)
    {
        serializedFunction = function.Name;
        rect = new int[4];
        rect[0] = collider.X;
        rect[1] = collider.Y;
        rect[2] = collider.Width;
        rect[3] = collider.Height;
        type = function.DeclaringType;
    }

    [OnSerialized]
    internal void OnSerializedMethod(StreamingContext context)
    {
        serializedFunction = null;
        rect = null;
        type = null;
    }

    /*
    [OnDeserializing()]
    internal void OnDeserializingMethod(StreamingContext context)
    { function = instance.GetType().GetMethod(serializedFunction); }
    */

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        collider = new Rectangle(rect[0], rect[1], rect[2], rect[3]);
        //(T) Convert.ChangeType(input, typeof(T));
        //type = typeof(LizardGuy);
        instance = Irbis.Irbis.game.GetType().GetField(inst).GetValue(Irbis.Irbis.game);
        function = type.GetMethod(serializedFunction);
        serializedFunction = null;
        rect = null;
        //type = null;
    }

    public override string ToString()
    {
        return
            "{function:" + function.Name +
            " Collider:" + collider +
            " instance:" + instance +
            " inst:" + inst +
            " type:" + type + "}";
    }

    public void Draw(SpriteBatch sb)
    { //ff7f00 // Color(255, 127, 0)
        RectangleBorder.Draw(sb, collider, Color.DarkOrange, true);
    }
}
