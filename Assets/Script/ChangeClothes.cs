using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChangeClothes : MonoBehaviour
{
    [SerializeField] GameObject[] listRings;
    int currentRings = 0;

    [SerializeField] GameObject[] listBelts;
    int currentBelt = 0;

    GameObject[] listGameObjectFromAPI;

    GameObject thePlayer;

    // Link list object:
    // https://ipfs.infura.io/ipfs/QmfENn7ZWZyXsGSxBaNwoYyaV94mzbiD2MXLkwkBnNDNry

    [Serializable]
    public struct SingleCloth
    {
        public string name;
        public string category;
        public string link;
    }

    SingleCloth[] listClothes;

    [System.Obsolete]
    IEnumerator Start()
    {
        string urlJSON = "https://ipfs.infura.io/ipfs/QmfENn7ZWZyXsGSxBaNwoYyaV94mzbiD2MXLkwkBnNDNry";

        thePlayer = GameObject.FindGameObjectsWithTag("Player")[0];

        UnityWebRequest request = UnityWebRequest.Get(urlJSON);
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            //show message "no internet "
        }
        else
        {
            if (request.isDone)
            {
                listClothes = JsonHelper.GetArray<SingleCloth>(request.downloadHandler.text);
                FetchGameObject();
            }
        }
    }

    [Obsolete]
    void FetchGameObject()
    {
        for (int i = 0; i < listClothes.Length; i++)
        {
            //Debug.Log(listClothes[i].name);
            int currentIndex = i;

            StartCoroutine(GetFileRequest(listClothes[currentIndex].link, (UnityWebRequest req) =>
            {
                if (req.isNetworkError || req.isHttpError)
                {
                    // Log any errors that may happen
                    Debug.Log($"{req.error} : {req.downloadHandler.text}");
                }
                else
                {
                    LoadModel(req.downloadHandler, listClothes[currentIndex].category);
                }
            }));

            //StartCoroutine(GetModelAsset(listClothes[currentIndex].link, (AssetBundle bundle) =>
            //{
            //    LoadAssetBundle(bundle, listClothes[currentIndex].category);
            //}));

        }
    }

    void LoadModel(DownloadHandler fileObject, string categoryOf)
    {
        foreach (Transform child in thePlayer.transform)
        {
            Debug.Log(child.gameObject.name);
        }
    }

    void LoadAssetBundle(AssetBundle fileObject, string categoryOf)
    {
        foreach (Transform child in thePlayer.transform)
        {
            //Debug.Log(child.gameObject.name);
            if (child.gameObject.name == categoryOf)
            {

            }
        }

    }

    IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(url);
            yield return req.SendWebRequest();
            callback(req);
        }
    }


    IEnumerator GetModelAsset(string urlAssetBundle, Action<AssetBundle> callback)
    {
        UnityWebRequest www = new UnityWebRequest(urlAssetBundle);
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
        www.downloadHandler = handler;
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Extracts AssetBundle
            AssetBundle bundle = handler.assetBundle;
            callback(bundle);
        }
    }

}
