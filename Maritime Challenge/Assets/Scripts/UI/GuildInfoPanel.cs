using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GuildInfoPanel : MonoBehaviour
{
    [SerializeField]
    private Text guildNameText, guildDescriptionText, guildOwnerText;

    [SerializeField]
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGuildInfoPanel(string guildName, string guildDescription, int ownerUID)
    {
        guildNameText.text = "Guild Name: " + guildName;
        guildDescriptionText.text = "Guild Description: " + guildDescription;

        if (ownerUID == 0)
            guildOwnerText.text = "Guild Owner: None";
        else
        {
            //if (ownerUID == PlayerData.UID)
            //    guildOwnerText.text = PlayerData.Name;
            //else if (ownerUID < PlayerData.UID)
            //    guildOwnerText.text = PlayerData.PhonebookData[ownerUID - 1].Name;
            //else if (ownerUID > PlayerData.UID)
            //    guildOwnerText.text = PlayerData.PhonebookData[ownerUID - 2].Name;


            StartCoroutine(GetUsername(ownerUID));
        }
    }

    IEnumerator GetUsername(int ownerUID)
    {
        string url = ServerDataManager.URL_getUsername;
        Debug.Log(url);

        //Need replace with the actual guild id later
        WWWForm form = new WWWForm();
        form.AddField("UID", ownerUID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize and instantiate somehow idk tbh
                guildOwnerText.text = "Guild Owner: " + webreq.downloadHandler.text;
                StartCoroutine(UIManager.ToggleFlyInAnim(rectTransform, new Vector2(0, -910), Vector2.zero, 0.5f, null));
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError(webreq.downloadHandler.text);
                break;
        }
    }

    public void OnBackButtonClicked()
    {
        StartCoroutine(BackButtonClicked());
    }

    IEnumerator BackButtonClicked()
    {
        yield return StartCoroutine(UIManager.ToggleFlyOutAnim(rectTransform, Vector2.zero, new Vector2(0, -910), 0.5f, null));

        gameObject.SetActive(false);
    }
}
