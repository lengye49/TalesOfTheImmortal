using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MathCalculator
{
    public static bool IsDodge(int rate)
    {
        return Random.Range(0, 10000) < rate * 100;
    }

    /// <summary>
    /// 自定义的六边形网络距离
    /// </summary>
    /// <returns>The battle grid distance.</returns>
    /// <param name="alpha">目标点.</param>
    /// <param name="beta">选择点.</param>
    /// <param name="isStraight">是否直线</param>
    public static int GetBattleGridDistance(Vector2Int alpha,Vector2Int beta,out bool isStraight){
        int distance = 0;
        int x1 = alpha.x;
        int x2 = beta.x;
        int y1 = alpha.y;
        int y2 = beta.y;
        int yMax;
        int yMin;
        int xDis;

        if (x1==x2){
            isStraight = true;
            distance = Mathf.Abs(y2 - y1);
        }else{
            xDis = Mathf.Abs(x2 - x1);

            if(x1%2==0){
                if(x2%2==0){
                    yMax = y1 + xDis / 2;
                    yMin = y1 - xDis / 2;
                }
                else{
                    yMax = y1 + xDis / 2;
                    yMin = y1 - xDis / 2 - 1;
                }
            }else{
                if(x2%2==0){
                    yMax = y1 + xDis / 2 + 1;
                    yMin = y1 - xDis / 2;
                }else{
                    yMax = y1 + xDis / 2;
                    yMin = y1 - xDis / 2;
                }
            }

            if (y2 == yMax || y2 == yMin)
            {
                isStraight = true;
                distance = xDis;
            }
            else if (y2 < yMax && y2 > yMin)
            {
                isStraight = false;
                distance = xDis;
            }
            else
            {
                isStraight = false;
                distance = xDis + Mathf.Abs(y2 - y1);
            }
        }

        isStraight = false;
        return distance;
    }

}
