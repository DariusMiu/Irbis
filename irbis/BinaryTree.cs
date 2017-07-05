using System;
using System.Collections.Generic;

public class BinaryTree<T> where T : System.IComparable<T>
{
    private bool rootIsNull;
    private BinaryTree<T> parent;
    private T data;
    private BinaryTree<T> left;
    private BinaryTree<T> right;

    public BinaryTree<T> Parent
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

    public BinaryTree<T> Left
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

    public BinaryTree<T> Right
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

    public BinaryTree(T value, BinaryTree<T> parentNode)
    {
        parent = parentNode;
        data = value;
        left = null;
        right = null;
        rootIsNull = false;
    }

    public BinaryTree(T value)
    {
        parent = null;
        data = value;
        left = null;
        right = null;
        rootIsNull = false;
    }

    public BinaryTree()
    {
        parent = null;
        data = default(T);
        left = null;
        right = null;
        rootIsNull = true;
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

    public void Clear()
    {
        //root = null;
    }

    public void Add(T value)
    {
        if (!rootIsNull)
        {
            bool placeLeft = false;
            bool placeRight = false;
            BinaryTree<T> node = this;
            while (node != null)
            {
                if (Comparer<T>.Default.Compare(value, node.Data) < 0)
                {
                    if (node.Left == null)
                    {
                        placeLeft = true;
                        break;
                    }
                    else
                    {
                        node = node.Left;
                    }
                }
                else
                {
                    if (node.Right == null)
                    {
                        placeRight = true;
                        break;
                    }
                    else
                    {
                        node = node.Right;
                    }
                }
            }

            if (placeLeft)
            {
                node.Left = new BinaryTree<T>(value, node);
            }
            else if (placeRight)
            {
                node.Right = new BinaryTree<T>(value, node);
            }
        }
        else
        {
            data = value;
            rootIsNull = false;
        }
    }

    public BinaryTree<T> GetLeftmost(BinaryTree<T> treeRoot)
    {
        BinaryTree<T> node = treeRoot;
        while (node != null)
        {
            if (node.Left == null)
            {
                break;
            }
            else
            {
                node = node.Left;
            }
        }
        return node;
    }

    public BinaryTree<T> GetNext(BinaryTree<T> treeRoot)
    {
        BinaryTree<T> node = treeRoot;
        while (node != null)
        {
            if (node.Left == null)
            {
                break;
            }
            else
            {
                node = node.Left;
            }
        }
        return node;
    }

    public System.Collections.IEnumerator GetEnumerator()
    {
        if (this.left != null)
        {
            foreach (float f in this.left)
            {
                yield return f;
            }
        }
        yield return this.data;
        if (this.right != null)
        {
            foreach (float f in this.right)
            {
                yield return f;
            }
        }
    }
}
