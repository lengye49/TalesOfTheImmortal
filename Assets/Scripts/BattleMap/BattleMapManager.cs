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

    private BattleMapView mapView;

    void Start(){
        mapView = this.GetComponent<BattleMapView>();
    }



    //**************
    //初始化地图
    //**************
    public void InitBattleMap(){
        if (!isExist)
        {
            mapGrids = new BattleGrid[20][];
            for (int i = 0; i < mapGrids.Length; i++)
            {
                mapGrids[i] = new BattleGrid[10];
                for (int j = 0; j < mapGrids[i].Length; j++)
                {
                    mapGrids[i][j] = new BattleGrid(i, j);
                }
            }
            isExist = true;
        }
        //standingGrid = mapGrids[0][0];
        //targetGrid = mapGrids[0][0];
        mapView.InitBattleMapView();
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
        return mapView.GetCellPosition(end.x, end.y);
    }

    public Vector2 UnitsIntoBattle(Vector2Int standingPoint){
        mapGrids[standingPoint.x][standingPoint.y].Occupied = true;
        return mapView.GetCellPosition(standingPoint.x, standingPoint.y);
    }


    ///**************
    //角色进入回合后的可行走状态
    ///**************
    public void StartRound(BattleUnit unit){
        standingGrid = mapGrids[unit.Position.x][unit.Position.y];
        targetGrid = standingGrid;
        walkableRange = CircleTargets(unit.Steps);
        Debug.Log("walkableRange.Count = " + walkableRange.Count);
        mapView.SetWalkingState(standingGrid, walkableRange);
    }

    /// <summary>
    /// 找到最近的目标
    /// </summary>
    BattleGrid GetNearestEnemy(){
        return new BattleGrid(0,0);
    }

    ///**************
    //选择目标
    ///**************

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
        if (pos.x >= 0 && pos.y >= 0)
            return new BattleGrid(pos.x, pos.y);
        else
            return null;
    }

    /// <summary>
    /// 根据方向和格子数获得线形区域内的格子，不包含目标点
    /// </summary>
    public List<BattleGrid> LineTargets(GridDirection direction, int count = 1)
    {
        List<BattleGrid> grids = new List<BattleGrid>();
        BattleGrid lastGrid = standingGrid;
        for (int i = 0; i < count; i++)
        {
            BattleGrid nextGrid = GetGridByDirection(lastGrid, direction);
            grids.Add(nextGrid);
            lastGrid = nextGrid;
        }
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

        Debug.Log("↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓Processing↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓");
        for (int i = 0; i < count;i++){
            surroundings = temp;
            temp = new List<BattleGrid>();
            foreach(BattleGrid battleGrid in surroundings)
            {
                Debug.Log("Handling " + battleGrid.Position);
                List<BattleGrid> neighbours = GetNeighbour(battleGrid);
                foreach(BattleGrid neighbour in neighbours){
                    if (neighbour == null)
                        continue;
                    Debug.Log("New neighbout + " + neighbour.Position);
                    if(!grids.Contains(neighbour)){
                        grids.Add(neighbour);
                        temp.Add(neighbour);
                        Debug.Log("Add New Neighbour " + neighbour.Position);
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

