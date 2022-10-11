using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarDisplay : MonoBehaviour
{
  
    private Player player = null;

    public void SetPlayer(Player player)
    {
        if (this.player != null)
            this.player.OnAvatarChanged -= OnPlayerAvatarUpdated;
        this.player = player;
        player.OnAvatarChanged += OnPlayerAvatarUpdated;
    }

    private void OnPlayerAvatarUpdated(BODY_PART_TYPE type, int cosmeticID)
    {

    }
}
