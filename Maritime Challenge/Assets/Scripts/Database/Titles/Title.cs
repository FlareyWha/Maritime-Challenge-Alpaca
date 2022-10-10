using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title
{
    public int TitleID;
    public string TitleName;

    public TitleSO LinkedTitle;

    public Title(int ID, string name)
    {
        TitleID = ID;
        TitleName = name;
    }
}
