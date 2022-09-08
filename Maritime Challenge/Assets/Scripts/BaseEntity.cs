using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public class BaseEntity
{
    protected int hp;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    //protected int def;
    //public int DEF
    //{
    //    get { return def; }
    //    set { def = value; }
    //}

    protected int atk;
    public int ATK
    {
        get { return ATK; }
        set { ATK = value; }
    }

    protected int atkspd;
    public int ATKSPD
    {
        get { return atkspd; }
        set { atkspd = value; }
    }

    protected int critrate;
    public int CRIT_RATE
    {
        get { return critrate; }
        set { critrate = value; }
    }

    protected int critdmg;
    public int CRIT_DMG
    {
        get { return critdmg; }
        set { critdmg = value; }
    }

    protected int movespd;
    public int MOVESPD
    {
        get { return movespd; }
        set { movespd = value; }
    }
}
