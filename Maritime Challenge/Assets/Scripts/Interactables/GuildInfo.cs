using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GuildInfo : BaseInteractable
{
    private JSONGuildInfo guildInfo;

    private List<JSONGuildMember> guildMembers = new List<JSONGuildMember>();

    [SerializeField]
    private int guildID = 1;

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
        StartCoroutine(UpdateGuildPanel());
    }

    IEnumerator UpdateGuildPanel()
    {
        UIManager.Instance.GuildInfoPanel.gameObject.SetActive(true);
        UIManager.Instance.ToggleJoystick(false);

        CoroutineCollection coroutineCollectionManager = new CoroutineCollection();

        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetGuildInfo()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetGuildMembers()));

        yield return coroutineCollectionManager;
    }

    IEnumerator GetGuildInfo()
    {
        string url = ServerDataManager.URL_getGuildInfo;
        Debug.Log(url);

        //Need replace with the actual guild id later
        WWWForm form = new WWWForm();
        form.AddField("iGuildID", guildID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize and instantiate somehow idk tbh
                guildInfo = JSONDeseralizer.DeseralizeGuildInfo(webreq.downloadHandler.text);

                Debug.Log(UIManager.Instance);
                Debug.Log(UIManager.Instance.GuildInfoPanel);

                UIManager.Instance.GuildInfoPanel.UpdateGuildInfoPanel(guildInfo.sGuildName, guildInfo.sGuildDescription, guildInfo.iOwnerUID);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator GetGuildMembers()
    {
        string url = ServerDataManager.URL_getGuildMembers;
        Debug.Log(url);

        //Need replace with the actual guild id later
        WWWForm form = new WWWForm();
        form.AddField("iGuildID", guildID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                guildMembers = JSONDeseralizer.DeseralizeGuildMembers(webreq.downloadHandler.text);
                UIManager.Instance.GuildInfoPanel.UpdateGuildMembers(guildMembers);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }
}
