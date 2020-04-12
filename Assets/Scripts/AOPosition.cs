using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOPosition : System.IEquatable<AOPosition>
{
    public static AOPosition zero = new AOPosition(0, 0);
    public static AOPosition up = new AOPosition(0, -1);
    public static AOPosition down = new AOPosition(0, 1);
    public static AOPosition right = new AOPosition(1, 0);
    public static AOPosition left = new AOPosition(-1, 0);

    public int x = 0;
    public int y = 0;

    public AOPosition()
    {
        this.x = 0;
        this.y = 0;
    }

    public AOPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public AOPosition(Vector3Int vector)
    {
        this.x = vector.x;
        this.y = vector.y + 99;
    }

    public override bool Equals(object obj)
    {
        return this.Equals((AOPosition)obj);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }

    public static AOPosition ParseVector3(Vector3Int vector)
    {
        return new AOPosition(vector.x, 99 - vector.y);
    }

    public bool Equals(AOPosition position)
    {
        return x == position.x && y == position.y;
    }

    public bool NearlyEquals(AOPosition position, float tolerance = 0.0001f)
    {
        return Mathf.Abs(x - position.x) < tolerance && Mathf.Abs(y - position.y) < tolerance;
    }

    public Vector3Int MapPositionToVector3()
    {
        return new Vector3Int(x, 99 - y, 0);
    }

    //Operator overrides
    public static AOPosition operator +(AOPosition b, AOPosition c)
    {
        AOPosition tempPos = new AOPosition();
        tempPos.x = b.x + c.x;
        tempPos.y = b.y + c.y;
        
        return tempPos;
    }

    public static AOPosition operator -(AOPosition b, AOPosition c)
    {
        AOPosition tempPos = new AOPosition();
        tempPos.x = b.x - c.x;
        tempPos.y = b.y - c.y;

        return tempPos;
    }

    public static AOPosition operator *(AOPosition b, AOPosition c)
    {
        AOPosition tempPos = new AOPosition();
        tempPos.x = b.x * c.x;
        tempPos.y = b.y * c.y;

        return tempPos;
    }

    public static AOPosition operator /(AOPosition b, AOPosition c)
    {
        AOPosition tempPos = new AOPosition();
        tempPos.x = b.x / c.x;
        tempPos.y = b.y / c.y;

        return tempPos;
    }
}
