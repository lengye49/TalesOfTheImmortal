using UnityEngine;
using System.Collections;

public class LoadFiles : MonoBehaviour
{
    public static Skill GetSkill(SkillInfo info){return new Skill();}

    public static Item GetItem() { return new Item(); }

    public static string GetLanguage(int id){
        string[][] strs = LoadFile("Specs/Language");
        for (int i = 0; i < strs.Length - 5;i++){
            int _id= int.Parse(GetDataByRowAndCol(strs, i + 4, 0));
            if(_id==id)
                return GetDataByRowAndCol(strs, i + 4, 1);
        }
        return id.ToString();
    }



//    public static CardData ReadCardData(int id)
//    {
//        //        Debug.Log("Reading Card : " + id);
//        string[][] strs = ReadTxt.ReadText("Configs/cards");
//        CardData c = ExcuteReadCardData(id, strs);
//        return c;
//    }

//    static CardData ExcuteReadCardData(int id, string[][] strs)
//    {
//        CardData c = new CardData();
//        for (int i = 0; i < strs.Length - 1; i++)
//        {

//            c.Id = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 0));
//            if (c.Id != id)
//                continue;
//            c.Name = ReadTxt.GetDataByRowAndCol(strs, i + 1, 1);
//            c.Description = ReadTxt.GetDataByRowAndCol(strs, i + 1, 2);
//            c.Price = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 3));
//            c.Level = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 4));
//            c.MaxLevel = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 5));
//            c.ActionCost = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 6));
//            c.MpCost = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 7));
//            c.Type = (CardType)int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 8));
//            c.Condition = (CardPlayCondition)int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 9));
//            int[] effectId = ReadString.GetInts(ReadTxt.GetDataByRowAndCol(strs, i + 1, 10));
//            c.Effects = ReadCardEffect(effectId);
//            c.DefaultTarget = 0;
//            c.PRI = 0;
//            return c;
//        }
//        Debug.Log("Cannot find CardData where id = " + id);
//        return c;
//    }

//    //    public CardEffect ReadCardEffect(int id){
//    //        string[][] strs = ReadTxt.ReadText("cardeffects");
//    //        CardEffect ce = ExcuteReadCardEffect(id, strs);
//    //        return ce;
//    //    }

//    public static CardEffect[] ReadCardEffect(int[] id)
//    {
//        string[][] strs = ReadTxt.ReadText("Configs/cardeffect");
//        CardEffect[] ces = new CardEffect[id.Length];
//        for (int i = 0; i < ces.Length; i++)
//        {
//            ces[i] = ExcuteReadCardEffect(id[i], strs);
//        }
//        return ces;
//    }

//    static CardEffect ExcuteReadCardEffect(int id, string[][] strs)
//    {
//        //        Debug.Log("Reading CardEffect : " + id);
//        CardEffect ce = new CardEffect();
//        for (int i = 0; i < strs.Length; i++)
//        {

//            ce.Id = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 0));
//            if (ce.Id != id)
//                continue;
//            ce.Type = (CardEffectType)int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 1));
//            ce.Param = ReadTxt.GetDataByRowAndCol(strs, i + 1, 2);
//            return ce;
//        }
//        Debug.Log("Cannot find CardEffect where id = " + id);
//        return ce;
//    }

//    //    void LoadMonsters(){
//    //        Debug.Log("Loading Monsters...");
//    //        string[][] strs = ReadTxt.ReadText("monsters");
//    //        for (int i = 0; i < strs.Length-1; i++)
//    //        {
//    //            Monster m = new Monster();
//    //            m.Id = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 0));
//    //            m.Name = ReadTxt.GetDataByRowAndCol (strs, i + 1, 1);
//    //            m.Description = ReadTxt.GetDataByRowAndCol (strs, i + 1, 2);
//    //            m.Gold = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 3));
//    //            m.Exp = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 4));
//    //            m.Action = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 5));
//    //            m.Mana = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 6));
//    //            m.Hp = float.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 7)); 
//    //            m.Level = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 8));
//    //            m.IsBoss = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 9));
//    //            m.Deck = int.Parse (ReadTxt.GetDataByRowAndCol (strs, i + 1, 10));
//    //
//    //            MonsterDictionary.Add(m.Id, m);
//    //        }
//    //    }
//}





    public static string[][] LoadFile(string fildPath)
    {
        string[][] textArray;
        TextAsset binAsset = Resources.Load(fildPath, typeof(TextAsset)) as TextAsset;
        //string[] lineArray = binAsset.text.Split("\r"[0]);//split the txt by return("/r"[0]);
        string[] lineArray = binAsset.text.Split("\n"[0]);

        textArray = new string[lineArray.Length][];

        for (int i = 0; i < lineArray.Length; i++)
        {
            //Debug.Log(lineArray[i]);
            textArray[i] = lineArray[i].Split(','); //split the line by ','
        }

        return textArray;

    }

    public static string GetDataByRowAndCol(string[][] textArray, int nRow, int nCol)
    {
        if (textArray.Length <= 0 || nRow >= textArray.Length)
            return "";
        if (nCol >= textArray[0].Length)
            return "";

        return textArray[nRow][nCol];
    }
}
