using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GuildInfo : Interactable
{
    private JSONGuildInfo guildInfo;

    [SerializeField]
    private GuildInfoPanel guildInfoPanel;

    // Start is called before the first frame update
    void Start()
    {
        interactMessage = "View Guild Info?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        StartCoroutine(GetGuildInfo());
    }

    IEnumerator GetGuildInfo()
    {
        string url = ServerDataManager.URL_getGuildInfo;
        Debug.Log(url);

        //Need replace with the actual guild id later
        WWWForm form = new WWWForm();
        form.AddField("iGuildID", 1);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize and instantiate somehow idk tbh
                guildInfo = JSONDeseralizer.DeseralizeGuildInfo(webreq.downloadHandler.text);
                guildInfoPanel.UpdateGuildInfoPanel(guildInfo.sGuildName, guildInfo.sGuildDescription, guildInfo.iOwnerUID);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError(webreq.downloadHandler.text);
                break;
        }
    }
}
