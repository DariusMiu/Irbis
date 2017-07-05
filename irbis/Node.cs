
public class Node<T> where T : System.IComparable<T>
{
    private Node<T> parent;
    private T data;
    private Node<T> left;
    private Node<T> right;

    public Node<T> Parent
    {
        get
        {
            return parent;
        }
    }

    public T Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
        }
    }

    public Node<T> Left
    {
        get
        {
            return left;
        }
        set
        {
            left = value;
        }
    }

    public Node<T> Right
    {
        get
        {
            return right;
        }
        set
        {
            right = value;
        }
    }

    public Node(T value, Node<T> parentNode)
    {
        parent = parentNode;
        data = value;
        left = null;
        right = null;
    }

    public Node(T value)
    {
        parent = null;
        data = value;
        left = null;
        right = null;
    }

    public Node()
    {
        parent = null;
        data = default(T);
        left = null;
        right = null;
    }

    public override string ToString()
    {
        string returnstring = "{parent:";
        if (parent != null)
        {
            returnstring += parent.data + " data:" + data;
        }
        else
        {
            returnstring += "null" + " data:" + data;
        }
        returnstring += " left:";
        if (left != null)
        {
            returnstring += left.data;
        }
        else
        {
            returnstring += "null";
        }
        returnstring += " right:";
        if (right != null)
        {
            returnstring += right.data;
        }
        else
        {
            returnstring += "null";
        }
        returnstring += "}";
        return returnstring;
    }

    public string Print()
    {
        return "{parent:" + parent + " data:" + data + " left:" + left + " right:" + right + "}";
    }
}

