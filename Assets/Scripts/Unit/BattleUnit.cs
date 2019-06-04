using UnityEngine;
using System.Collections.Generic;

public class BattleUnit 
{
    public string Name;
    public string Image;
    public UnitSide Side;
    public BattleUnitView View;

    public int HP;
    public int HpMax;

    public int MP;
    public int MpMax;

    public int Spirit;//影响暴击、命中
    public int SpiritMax;

    public int Speed;
    public int SpeedMax;

    public int Attack = 0;
    public int Defence = 0;//物理减伤
    //public int MigicalDenfence = 0;//魔法减伤将按照五行区分
    public int Dodge = 0;

    public int Shield = 0;
    public int Rebound = 0;//反伤
    public int DamageToManaShiled = 0;
    public bool Crippled;

    public List<Skill> Skills = new List<Skill>();

    public int Steps;
    public Vector2Int Position;
    public float SingTime = 0;
    public float CD = 0;
    public float WaitingTime{
        get { return CD + SingTime; }
    }

    public BattleUnit(string name,UnitSide side, BattleUnitView view,Vector2Int pos){
        Name = name;
        Image = "5";
        Side = side;
        View = view;
        Steps = 3;
        Position = pos;

        HP = 100;
        HpMax = 100;
        MP = 100;
        MpMax = 100;
        Spirit = 100;
        SpiritMax = 100;
        Speed = 5;
        SpeedMax = 5;
        Attack = 100;
        Defence = 10;
        Dodge = 0;
        Shield = 0;
        Rebound = 0;
        DamageToManaShiled = 0;
        Crippled = false;

        Skills = new List<Skill>();
        Skills.Add(new Skill());

        SingTime = 0;
        CD = 0;

        view.Init();
    }

    public int Heal(int value)
    {
        int v = Mathf.Min(value, HpMax - HP);
        HP += v;
        View.UpdateHp((float)(HP / HpMax));
        return v;
    }

    public int Damage(int value)
    {
        int v = Mathf.Min(value, HP);
        HP -= v;
        View.UpdateHp((float)(HP / HpMax));
        return v;
    }

    public int HealMp(int value)
    {
        MP += value;
        //View.UpdateMp(MP);
        return value;
    }

    public int DamageMp(int value)
    {
        int v = Mathf.Min(value, HP);
        MP -= v;
        //View.UpdateMp(MP);
        return v;
    }
}

public enum UnitSide{
    Ally,
    Enemy
}
