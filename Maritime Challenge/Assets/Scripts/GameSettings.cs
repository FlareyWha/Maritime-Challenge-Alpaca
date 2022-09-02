using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public static bool LOCK_JOYSTICK = false;


    public static int GetEXPRequirement(int level)
    {
        return (level * level * 2) + level * 50 + 300;
    }
}


