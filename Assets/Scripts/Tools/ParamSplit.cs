using UnityEngine;

public class ParamSplit
{
    public static Vector2Int StrToVector2Int(string str){
        if (str.Contains("|"))
        {
            string[] s = str.Split('|');
            int x = int.Parse(s[0]);
            int y = int.Parse(s[1]);
            return new Vector2Int(x, y);
        }
        else
            return Vector2Int.zero;

    }
}
