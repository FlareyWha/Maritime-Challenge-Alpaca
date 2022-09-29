using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RedemptionRequestManager : MonoBehaviour
{
    [SerializeField]
    private Transform redemptionRequestListContent;

    [SerializeField]
    private GameObject redemptionRequestPrefab, loadingScreenSpin;

    public void GetRedemptionRequests()
    {
        loadingScreenSpin.SetActive(true);
        StartCoroutine(DoGetRedemptionRequests());
    }

    IEnumerator DoGetRedemptionRequests()
    {
        string url = ServerDataManager.URL_getRedemptionRequests;
        Debug.Log(url);

        using UnityWebRequest webreq = UnityWebRequest.Get(url);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                List<JSONRedemptionRequest> redemptionRequestsJSON = JSONDeseralizer.DeseralizeRedemptionRequests(webreq.downloadHandler.text);

                string prevName = "", prevItemName = "";
                RedemptionRequest previousRedemptionRequest = null;
                int count = 1;

                foreach (JSONRedemptionRequest redemptionRequestJSON in redemptionRequestsJSON)
                {
                    //If the prevName or prevItemName is not the same as before, finalise the initialsation of the previous redemption request and add new one, else increase the count
                    if (prevName != redemptionRequestJSON.sUsername || prevItemName != redemptionRequestJSON.sRedemptionItemName)
                    {
                        if (previousRedemptionRequest != null)
                            previousRedemptionRequest.InitRedemptionRequest(prevName, prevItemName, count);

                        RedemptionRequest redemptionRequest = Instantiate(redemptionRequestPrefab, redemptionRequestListContent).GetComponent<RedemptionRequest>();

                        prevName = redemptionRequestJSON.sUsername;
                        prevItemName = redemptionRequestJSON.sRedemptionItemName;
                        previousRedemptionRequest = redemptionRequest;
                        count = 1;
                    }
                    else
                        count++;
                }

                //Make sure to do this one more time since it is the last one
                previousRedemptionRequest.InitRedemptionRequest(prevName, prevItemName, count);

                loadingScreenSpin.SetActive(false);
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
