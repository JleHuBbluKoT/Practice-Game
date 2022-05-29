using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Rect
{
    public int bottom;
    public int left;
    public int top;
    public int right;

    public Rect()
    {
        bottom = 0;
        left = 0;
        top = 0;
        right = 0;
    }
    public Rect(Rect rect)
    {
        bottom = rect.bottom;
        left = rect.left;
        top = rect.top;
        right = rect.right;
    }

    public Rect(int _bottom, int _left, int _top, int _right)
    {
        bottom = _bottom;
        left = _left;
        top = _top;
        right = _right;
    }

    public int Width()
    {
        return right - left +1;
    }
    public int Height()
    {
        return top - bottom +1;
    }

    public int Area()
    {
        return Height() * Width();
    }

    public Rect Set( int _left, int _bottom, int _right, int _top)
    {
        bottom = _bottom;
        left = _left;
        top = _top;
        right = _right;
        return this;
    }

    public Rect SetPosition(int x, int y)
    {
        top = y + (top - bottom);
        right = x + (right - left);
        bottom = y;
        left = x;
        return this;
    }
    public Rect Shift(int x, int y)
    {
        bottom += y;
        left += x;
        top += y;
        right += x;
        return this;
    }

    public Rect Resize(int x, int y)
    {
        right += x;
        top += y;
        return this;
    }

    public Rect intersect(Rect other)
    {
        Rect result = new Rect();
        result.left = Math.Max(left, other.left);
        result.right = Math.Min(right, other.right);
        result.top = Math.Min(top, other.top);
        result.bottom = Math.Max(bottom, other.bottom);
        return result;
    }

    public bool CheckIntersect(Rect other)
    {
        //(RoomA.coords.x < RoomB.secondPoint().x) && (RoomA.secondPoint().x > RoomB.coords.x) && (RoomA.coords.y < RoomB.secondPoint().y) && (RoomA.secondPoint().y > RoomB.coords.y)
        if ((left < other.right + 1) && (right + 1 > other.left) && (bottom < other.top + 1) && (top + 1 > other.bottom))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Rect union(Rect other)
    {
        Rect result = new Rect();
        result.left = Math.Min(left, other.left);
        result.right = Math.Max(right, other.right);
        result.top = Math.Max(top, other.top);
        result.bottom = Math.Min(bottom, other.bottom);
        return result;
    }
    public bool inside(int x, int y)
    {
        return ( left <= x && x <= right && bottom <= y && y <= top);
    }

    public Vector2Int center()
    {
        return new Vector2Int(
                (left + right) / 2 + (right - left) % 2,
                (top + bottom) / 2 + (top - bottom) % 2);
    }

    public Rect shrink(int d)
    {
        return new Rect(bottom + d, left + d, top - d, right - d );
    }

}
