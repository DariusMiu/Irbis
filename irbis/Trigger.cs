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

[DataContract]
[Serializable]
public class Trigger
{
    public Rectangle Collider
    {
        get
        { return collider; }
    }
    private int[] rect;

    [DataMember]
    public int repeat;
    [DataMember]
    private string serializedFunction = null;
    [DataMember]
    private string inst;
    [DataMember]
    private string typename;
    private Type type;

    [NonSerialized]
    [DataMember]
    private Rectangle collider;

    [NonSerialized]
    private MethodInfo function;
    [NonSerialized]
    private object instance;

    /// <summary>
    /// does something when entered by player
    /// </summary>
    /// <param name="Function">function to run upon triggering. ie: typeof(LizardGuy).GetMethod("StartUp") (function must accept a passed object)</param>
    /// <param name="Instance">Instance of the function to invoke (relative to Irbis.Irbis.game). ie: "this" = Irbis.Irbis.game</param>
    /// <param name="Area">Collider area</param>
    /// <param name="Count">How many times can this trigger run. Negative for infinte</param>
    public Trigger(MethodInfo Function, string Instance, Rectangle Area, int Count)
	{
        function = Function;
        inst = Instance;
        instance = Irbis.Irbis.game.GetType().GetField(inst).GetValue(Irbis.Irbis.game);
        collider = Area;
        if (Count == 0)
        { repeat = -1; }
        else
        { repeat = Count; }
    }

    public void Update(ICollisionObject CollisionObject)
    {
        if (repeat != 0)
        {
            if (collider.Intersects(CollisionObject.Collider))
            {
                try
                { function.Invoke(instance, new object[] { CollisionObject }); }
                catch (Exception e)
                {
                    Irbis.Irbis.WriteLine("Trigger Exception: " + e.Message);
                    Irbis.Irbis.WriteLine("Stacktrace:\n" + e.StackTrace + "\n");
                    Irbis.Irbis.DisplayInfoText("Trigger Exception: " + e.Message, Color.Red);
                }
                if (repeat > 0)
                { repeat--; }
            }
        }
    }

    public bool Check()
    {
        bool check = true;
        if (!string.IsNullOrWhiteSpace(typename))
        { function = Type.GetType(typename).GetMethod(serializedFunction); }
        else if (type != null)
        { function = type.GetMethod(serializedFunction); }
        if (function == null)
        { check = false; }
        instance = Irbis.Irbis.game.GetType().GetField(inst).GetValue(Irbis.Irbis.game);
        if (instance == null)
        { check = false; }
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
        typename = function.DeclaringType.FullName;
    }

    [OnSerialized]
    internal void OnSerializedMethod(StreamingContext context)
    {
        /*serializedFunction = null;
        rect = null;
        type = null;
        typename = null;*/
    }

    //[OnDeserializing]
    //internal void OnDeserializingMethod(StreamingContext context)
    //{ Irbis.Irbis.WriteLine("deserializing Trigger..."); }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        if (rect != null)
        {
            collider = new Rectangle(rect[0], rect[1], rect[2], rect[3]);
            rect = null;
        }
        //(T) Convert.ChangeType(input, typeof(T));
        //type = typeof(LizardGuy);
        instance = Irbis.Irbis.game.GetType().GetField(inst).GetValue(Irbis.Irbis.game);
        if (!string.IsNullOrWhiteSpace(typename))
        { function = Type.GetType(typename).GetMethod(serializedFunction); }
        else if (type != null)
        { function = type.GetMethod(serializedFunction); }
        //serializedFunction = null;
        //type = null;
        //Irbis.Irbis.WriteLine("done. " + this.ToString());
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
        RectangleBorder.Draw(sb, collider, Color.Cyan, true);
    }
}
