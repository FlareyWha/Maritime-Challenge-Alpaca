using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class JSONPlayerData
{
    public string sUsername;
    public string dBirthday;
    public int iLevel;
    public int iXP;
    public int iDepartment;
    public int iGuildID;
    public int iCountry;
    public int iTotalRightshipRollers;
    public int iTotalTokens;
    public int iTotalEventCurrency;
    public float fPlayerXPos;
    public float fPlayerYPos;
    public float fPlayerZPos;
}

[Serializable]
public class JSONPlayerDataList
{
    public List<JSONPlayerData> playerData = new List<JSONPlayerData>();
}
