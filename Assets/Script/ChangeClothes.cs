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
                StartCoroutine(FetchGameObject());
            }
        }
    }

    IEnumerator FetchGameObject()
    {
        for (int i = 0; i < listClothes.Length; i++)
        {
            Debug.Log(listClothes[i].name);
            string url3DObject = listClothes[i].link;
            UnityWebRequest request3DObject = UnityWebRequest.Get(url3DObject);
            yield return request3DObject.SendWebRequest();

            byte[] result = request3DObject.downloadHandler.data;
            Debug.Log(result);
        }
    }

}
