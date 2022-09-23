using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameHandler : NetworkBehaviour
{
    #region Singleton
    public static GameHandler Instance = null;
    #endregion
    private readonly SyncList<Player> onlinePlayers = new SyncList<Player>();

    private void Awake()
    {
        Instance = this;
    }

    [Server]
    public void OnNewPlayerJoined(NetworkConnectionToClient conn)
    {
        StartCoroutine(SetPlayerOnline(conn));
    }
    IEnumerator SetPlayerOnline(NetworkConnectionToClient conn)
    {
        while (conn.identity == null)
            yield return null;
        onlinePlayers.Add(conn.identity.gameObject.GetComponent<Player>());
    }
    [Server]
    public void OnPlayerQuit(NetworkConnectionToClient conn)
    {
        onlinePlayers.Remove(conn.identity.gameObject.GetComponent<Player>());
       // StartCoroutine(SetPlayerOffline(conn));
    }
    //IEnumerator SetPlayerOffline(NetworkConnectionToClient conn)
    //{
    //}
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
        Vector3 spriteScreenPos = UIManager.Instance.Camera.GetComponent<Camera>().WorldToScreenPoint(spritePos);
        if (touchPos.x < spriteScreenPos.x + spriteSize.x * 0.5f && touchPos.x > spriteScreenPos.x - spriteSize.x * 0.5f
            && touchPos.y > spriteScreenPos.y - spriteSize.y * 0.5f && touchPos.y < spriteScreenPos.y + spriteSize.y * 0.5f)
        {
            return true;
        }

        return false;
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