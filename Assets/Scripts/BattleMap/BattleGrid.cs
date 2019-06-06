using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid
{
    public Vector2Int Position;
    public bool Occupied;
    public bool Reachable;

    public bool Walkable{ get { return (!Occupied)&Reachable; }}

    public BattleGrid(Vector2Int _pos)
    {
        Position = _pos;
        Reachable = false;
        Occupied = false;
    }

    public BattleGrid(int xCount,int yCount){
        Position = new Vector2Int(xCount, yCount);
        Reachable = false;
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

