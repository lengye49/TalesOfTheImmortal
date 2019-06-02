using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid
{
    public Vector2Int Position;
    public bool Occupied;

    //public bool Walkable{ get { return !Occupied; }}

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;

        if (ReferenceEquals(this, obj)) return true;

        if (obj.GetType() != this.GetType()) return false;
        return Equals(obj);
    }

    public bool Equals(BattleGrid other){
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (this.Position != other.Position) return false;
        return true;
    }

    public BattleGrid(Vector2Int _pos)
    {
        Position = _pos;
        Occupied = false;
    }

    public BattleGrid(int xCount,int yCount){
        Position = new Vector2Int(xCount, yCount);
        Occupied = false;
    }

    public Vector2Int LeftUp{
        get{
            if(Position.x%2==1)
                return new Vector2Int(Position.x - 1, Position.y);
            else
                return new Vector2Int(Position.x - 1, Position.y-1);
        }
    }

    public Vector2Int LeftDown{
        get
        {
            if (Position.x % 2 == 1)
                return new Vector2Int(Position.x - 1, Position.y + 1);
            else
                return new Vector2Int(Position.x - 1, Position.y);
        }
    }
   
    public Vector2Int RightUp{
        get
        {
            if (Position.x % 2 == 1)
                return new Vector2Int(Position.x + 1, Position.y);
            else
                return new Vector2Int(Position.x + 1, Position.y - 1);
        }
    }

    public Vector2Int RightDown{
        get
        {
            if (Position.x % 2 == 1)
                return new Vector2Int(Position.x + 1, Position.y + 1);
            else
                return new Vector2Int(Position.x + 1, Position.y);
        }
    }

    public Vector2Int Up{
        get { return new Vector2Int(Position.x, Position.y - 1); }
    }

    public Vector2Int Down
    {
        get { return new Vector2Int(Position.x, Position.y + 1); }
    }
}

public enum GridDirection{
    LeftUp,
    LeftDown,
    RightUp,
    RightDown,
    Up,
    Down,
    None
}

