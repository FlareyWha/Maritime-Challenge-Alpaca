using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class GameSettings
{
    public static bool LOCK_JOYSTICK = false;


    public static int NumGiftsDaily = 5;
    public static int MinNumGiftTokens = 5;
    public static int MaxNumGiftTokens = 30;

    public static DateTime DailyResetTime = new DateTime(2022, 01, 01, 00, 00, 00);

    public static float WORLD_MIN_X = -160.0f;
    public static float WORLD_MAX_X = 192.0f;
    public static float WORLD_MIN_Y = -86.0f;
    public static float WORLD_MAX_Y = 86.0f;

    public static int XP_PER_RIGHTSHIPEDIA_ENTRY = 300;
    public static int XP_PER_ENEMY_KILL = 100;

    public static int GetEXPRequirement(int level)
    {
        return (level * level * 2) + level * 100 + 300;
    }

    public static int GetFriendshipXPRequirement(int level)
    {
        return (level * level) + level * 200 + 100;
    }

    public static int CompareDateTime(DateTime t1, DateTime t2)
    {
        //Less than zero : If t1 is earlier than t2.
        //Zero: If t1 is the same as t2.
        //Greater than zero : If t1 is later than t2.
        return DateTime.Compare(t1, t2);
    }
}


public static class SpriteHandler
{
    public static Vector2 GetSpriteSizeInPixels(SpriteRenderer sprite)
    {
        Vector2 spriteSize = GetSpriteSize(sprite);

        return DisplayUtility.ConvertWorldToScreen(spriteSize);
    }

    public static Vector2 GetSpriteSize(SpriteRenderer sprite)
    {
        Vector2 spriteSize = sprite.bounds.size;
        //spriteSize.x *= transform.lossyScale.x;
        //spriteSize.y *= transform.lossyScale.y;
        return spriteSize;
    }
    public static bool IsWithinSprite(Vector3 spritePos, SpriteRenderer sprite)
    {
        // Get Player Sprite Size
        Vector2 spriteSize = SpriteHandler.GetSpriteSizeInPixels(sprite);

        Vector2 touchPos = InputManager.GetTouchPos();
        Vector3 spriteScreenPos = PlayerFollowCamera.Instance.GetComponent<Camera>().WorldToScreenPoint(spritePos);
        if (touchPos.x < spriteScreenPos.x + spriteSize.x * 0.5f && touchPos.x > spriteScreenPos.x - spriteSize.x * 0.5f
            && touchPos.y > spriteScreenPos.y - spriteSize.y * 0.5f && touchPos.y < spriteScreenPos.y + spriteSize.y * 0.5f)
        {
            return true;
        }

        return false;
    }

    public static float GetSpriteRadius(SpriteRenderer spriteR)
    {
        Vector2 spriteSize = SpriteHandler.GetSpriteSize(spriteR);
        return (spriteSize.x + spriteSize.y) * 0.25f;
    }

    public static float GetSpriteSizeMax(SpriteRenderer spriteR)
    {
        Vector2 spriteSize = SpriteHandler.GetSpriteSize(spriteR);
        return spriteSize.x > spriteSize.y ? spriteSize.x : spriteSize.y;
    }

    public static float GetSpriteSizeMin(SpriteRenderer spriteR)
    {
        Vector2 spriteSize = SpriteHandler.GetSpriteSize(spriteR);
        return spriteSize.x < spriteSize.y ? spriteSize.x : spriteSize.y;
    }

}

public static class DisplayUtility
{
    public static Vector2 ConvertWorldToScreen(Vector2 screenVec)
    {
        float cam_world_units_x = Camera.main.orthographicSize * 2.0f * ((float)Screen.width / Screen.height);
        float cam_world_units_y = Camera.main.orthographicSize * 2.0f;
        float sizeX = screenVec.x * (Screen.width / cam_world_units_x);
        float sizeY = screenVec.y * (Screen.height / cam_world_units_y);
        return new Vector2(sizeX, sizeY);
    }

    public static Vector2 ConvertScreenToWorld(Vector2 screenVec)
    {
        float cam_world_units_x = Camera.main.orthographicSize * 2.0f * ((float)Screen.width / Screen.height);
        float cam_world_units_y = Camera.main.orthographicSize * 2.0f;
        float sizeX = screenVec.x * (cam_world_units_x / Screen.width);
        float sizeY = screenVec.y * (cam_world_units_y / Screen.height);
        return new Vector2(sizeX, sizeY);
    }

}
