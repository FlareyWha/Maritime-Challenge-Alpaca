using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title
{
    public int titleID;
    public string titleName;

    public TitleSO LinkedTitle;

    public Title(int ID, string name)
    {
        titleID = ID;
        titleName = name;
    }
}
