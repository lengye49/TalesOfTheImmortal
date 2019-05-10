using UnityEngine;
using System.Collections;

public class BattleUnit 
{
    public string Name;
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

    public BattleUnit(BattleUnitView _view){
        View = _view;
    }

    public int Heal(int value)
    {
        int v = Mathf.Min(value, HpMax - HP);
        HP += v;
        //View.UpdateHp(HP, HpMax);
        return v;
    }

    public int Damage(int value)
    {
        int v = Mathf.Min(value, HP);
        HP -= v;
        //View.UpdateHp(HP, HpMax);
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
