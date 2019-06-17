using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapManager:MonoBehaviour {

    private static BattleMapManager _instance;
    public static BattleMapManager Instance{
        get{
            if (_instance == null)
                _instance = GameObject.Find("BattleMapContainer").GetComponent<BattleMapManager>();
            return _instance;
        }
    }

    private BattleGrid[][] mapGrids;
    private BattleGrid standingGrid;
    private BattleGrid targetGrid;
    private List<BattleGrid> walkableRange;
    private List<BattleGrid> enemyGrids;
    private List<BattleGrid> allyGrids;

    private bool isExist = false;

    public BattleMapView MapView;

    void Start(){
        MapView = this.GetComponent<BattleMapView>();
    }


    //**************
    //初始化地图
    //**************
    public void InitBattleMap(){
        if (!isExist)
        {
            mapGrids = new BattleGrid[DefaultConfigs.BattleMapSize.x][];
            for (int i = 0; i < mapGrids.Length; i++)
            {
                mapGrids[i] = new BattleGrid[DefaultConfigs.BattleMapSize.y];
                for (int j = 0; j < mapGrids[i].Length; j++)
                {
                    mapGrids[i][j] = new BattleGrid(i, j);
                }
            }
            MapView.InitBattleMapView();
            isExist = true;
        }else{
            for (int i = 0; i < mapGrids.Length; i++)
            {
                for (int j = 0; j < mapGrids[i].Length; j++)
                {
                    mapGrids[i][j].Occupied = false;
                    mapGrids[i][j].Reachable = false;
                }
            }
        }
        //standingGrid = mapGrids[0][0];
        //targetGrid = mapGrids[0][0];
    }

    //**************
    //角色移动、入场
    //**************

    /// <summary>
    /// 单位移动到某位置，返回目标位置坐标
    /// </summary>
    /// <returns>The move to.</returns>
    public Vector2 UnitMoveTo(Vector2Int start, Vector2Int end)
    {
        mapGrids[start.x][start.y].Occupied = false;
        mapGrids[end.x][end.y].Occupied = true;
        standingGrid = mapGrids[end.x][end.y];
        return MapView.GetCellPosition(end.x, end.y);
    }

    public Vector2 UnitsIntoBattle(Vector2Int standingPoint){
        mapGrids[standingPoint.x][standingPoint.y].Occupied = true;
        return MapView.GetCellPosition(standingPoint.x, standingPoint.y);
    }


    ///**************
    //角色进入回合时的可行走状态
    ///**************
    public void StartRound(BattleUnit unit)
    {
        standingGrid = mapGrids[unit.Position.x][unit.Position.y];
        targetGrid = standingGrid;

        //重置一下可到达状态
        if (walkableRange != null)
            for (int i = 0; i < walkableRange.Count; i++)
                GetBattleGridByPos(walkableRange[i].Position).Reachable = false;

        walkableRange = CircleTargets(unit.Steps);

        //改变可到达状态
        for (int i = 0; i < walkableRange.Count; i++)
        {
            GetBattleGridByPos(walkableRange[i].Position).Reachable = true;
            //参数传递过程中，新建了BattleGrid
            //walkableRange[i].Reachable = true;
            //Debug.Log(walkableRange[i].Position + " Reachable = true");
            //Debug.Log(GetBattleGridByPos(walkableRange[i].Position).Position + " Reachable = " + GetBattleGridByPos(walkableRange[i].Position).Reachable);
        }
        MapView.SetWalkingState(standingGrid, walkableRange);
    }

    /// <summary>
    /// 找到离目标最近的移动点
    /// </summary>
    public Vector2Int GetMovingTargetPos(BattleUnit unit,Vector2Int targetPos){
        standingGrid = mapGrids[unit.Position.x][unit.Position.y];
        targetGrid = standingGrid;
        walkableRange = CircleTargets(unit.Steps);
        int distance = 0;
        int temp;
        int index = 0;
        bool isStraight;
        bool tempStraight;

        distance = MathCalculator.GetBattleGridDistance(targetPos, walkableRange[0].Position, out isStraight);
        Debug.Log("Total range = " + walkableRange.Count + "," + walkableRange[0].Position + "," + distance + "," + isStraight);

        for (int i = 1; i < walkableRange.Count;i++){
            Debug.Log(walkableRange[i].Position+"," + walkableRange[i].Walkable);
            if (!walkableRange[i].Walkable)
                continue;
            temp = MathCalculator.GetBattleGridDistance(targetPos, walkableRange[i].Position,out tempStraight);
            Debug.Log(walkableRange[i].Position + "," + distance + "," + isStraight);
            if (temp < distance || (!isStraight && tempStraight)){
                distance = temp;
                isStraight = tempStraight;
                index = i;
            }
        }
        return walkableRange[index].Position;
    }

    ///**************
    //角色进入选择目标状态
    ///**************
    public void SelectingTarget(List<BattleGrid> battleGrids){
        MapView.ResetState();
        MapView.SetSelectingState(standingGrid, battleGrids);
    }

    public BattleGrid GetBattleGridByPos(Vector2Int pos){
        return mapGrids[pos.x][pos.y];
    }

    /// <summary>
    /// 获得目标点某方向上相邻的格子
    /// </summary>
    BattleGrid GetGridByDirection(BattleGrid _grid, GridDirection _direction){
        Vector2Int pos;
        switch (_direction)
        {
            case GridDirection.LeftUp:
                pos = _grid.LeftUp;
                break;
            case GridDirection.LeftDown:
                pos = _grid.LeftDown;
                break;
            case GridDirection.RightUp:
                pos = _grid.RightUp;
                break;
            case GridDirection.RightDown:
                pos = _grid.RightDown;
                break;
            case GridDirection.Up:
                pos = _grid.Up;
                break;
            case GridDirection.Down:
                pos = _grid.Down;
                break;
            default:
                pos = Vector2Int.zero;
                break;
        }
        if (pos.x >= 0 && pos.y >= 0 && pos.x < mapGrids.Length && pos.y < mapGrids[0].Length)
            return GetBattleGridByPos(pos);
        else
            return null;
    }

    /// <summary>
    /// 根据方向和格子数获得线形区域内的格子，不包含目标点
    /// </summary>
    public List<BattleGrid> LineTargets(GridDirection direction, int count = 1)
    {
        string str = "Standing on " + standingGrid.Position;
        List<BattleGrid> grids = new List<BattleGrid>();
        BattleGrid lastGrid = standingGrid;
        for (int i = 0; i < count; i++)
        {
            BattleGrid nextGrid = GetGridByDirection(lastGrid, direction);
            if (nextGrid != null)
            {
                grids.Add(nextGrid);
                str += "," + nextGrid.Position;
                lastGrid = nextGrid;
            }
        }
        Debug.Log(str);
        return grids;
    }

    /// <summary>
    /// 根据方向和格子数获得扇形区域内的格子，包含目标点
    /// </summary>
    public List<BattleGrid> FanTargets(GridDirection direction, int count)
    {
        List<BattleGrid> grids = new List<BattleGrid>();
        grids.Add(targetGrid);

        List<BattleGrid> surroundings = new List<BattleGrid>();
        List<BattleGrid> temp = new List<BattleGrid>();
        temp.Add(targetGrid);

        List<GridDirection> directions = GetFanDirections(direction);

        for (int i = 0; i < count; i++)
        {
            surroundings = temp;
            temp = new List<BattleGrid>();
            foreach (BattleGrid battleGrid in surroundings)
            {
                List<BattleGrid> neighbours = GetFanNeighbour(battleGrid,directions);
                foreach (BattleGrid neighbour in neighbours)
                {
                    if (neighbour == null)
                        continue;
                    if (!grids.Contains(neighbour))
                    {
                        grids.Add(neighbour);
                        temp.Add(neighbour);
                    }
                }
            }
        }
        return grids;
    }

    /// <summary>
    /// 根据格子数获得圆形区域内的格子，包含目标点
    /// </summary>
    public List<BattleGrid> CircleTargets(int count=1)
    {
        Debug.Log("Finding Circle Targets with range = " + count + ", Target position = " + targetGrid.Position);
        List<BattleGrid> grids = new List<BattleGrid>();
        grids.Add(targetGrid);

        List<BattleGrid> surroundings = new List<BattleGrid>();
        List<BattleGrid> temp = new List<BattleGrid>();
        temp.Add(targetGrid);

        for (int i = 0; i < count;i++){
            surroundings = temp;
            temp = new List<BattleGrid>();
            foreach(BattleGrid battleGrid in surroundings)
            {
                List<BattleGrid> neighbours = GetNeighbour(battleGrid);
                foreach(BattleGrid neighbour in neighbours){
                    if (neighbour == null)
                        continue;
                    if(!grids.Contains(neighbour)){
                        grids.Add(neighbour);
                        temp.Add(neighbour);
                    }
                }
            }
        }
        return grids;
    }

    List<BattleGrid> GetNeighbour(BattleGrid grid){
        return new List<BattleGrid>
        {
            GetGridByDirection(grid, GridDirection.LeftUp),
            GetGridByDirection(grid, GridDirection.LeftDown),
            GetGridByDirection(grid, GridDirection.RightUp),
            GetGridByDirection(grid, GridDirection.RightDown),
            GetGridByDirection(grid, GridDirection.Up),
            GetGridByDirection(grid, GridDirection.Down)
        };
    }


    List<BattleGrid> GetFanNeighbour(BattleGrid grid, List<GridDirection> directions)
    {
        List<BattleGrid> grids = new List<BattleGrid>();

        if (directions.Contains(GridDirection.LeftUp))
            GetGridByDirection(grid, GridDirection.LeftUp);
        if (directions.Contains(GridDirection.LeftDown))
            GetGridByDirection(grid, GridDirection.LeftDown);
        if (directions.Contains(GridDirection.RightUp))
            GetGridByDirection(grid, GridDirection.RightUp);
        if (directions.Contains(GridDirection.RightDown))
            GetGridByDirection(grid, GridDirection.RightDown);
        if (directions.Contains(GridDirection.Up))
            GetGridByDirection(grid, GridDirection.Up);
        if (directions.Contains(GridDirection.Down))
            GetGridByDirection(grid, GridDirection.Down);
        return grids;
    }

    List<GridDirection> GetFanDirections(GridDirection main){
        List<GridDirection> directions = new List<GridDirection>();
        directions.Add(main);
        switch(main){
            case GridDirection.LeftUp:
                directions.Add(GridDirection.LeftDown);
                directions.Add(GridDirection.Up);
                break;
            case GridDirection.LeftDown:
                directions.Add(GridDirection.LeftUp);
                directions.Add(GridDirection.Down);
                break;
            case GridDirection.RightUp:
                directions.Add(GridDirection.RightDown);
                directions.Add(GridDirection.Up);
                break;
            case GridDirection.RightDown:
                directions.Add(GridDirection.RightUp);
                directions.Add(GridDirection.Down);
                break;
            case GridDirection.Up:
                directions.Add(GridDirection.LeftUp);
                directions.Add(GridDirection.RightUp);
                break;
            case GridDirection.Down:
                directions.Add(GridDirection.LeftDown);
                directions.Add(GridDirection.RightDown);
                break;
        }
        return directions;
    }

}

