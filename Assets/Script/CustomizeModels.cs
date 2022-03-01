using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CustomizeModels : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] heads;
    private int currentHead = 0;

    public GameObject[] belts;
    private int currentBelt = 0;


    [Serializable]
    public struct Game
    {
        public string name;
        public string category;
        public string link;
    }
    Game[] allGames;

    void Start()
    {
        StartCoroutine(GetGames());
    }

    IEnumerator GetGames()
    {
        string url = "https://ipfs.infura.io/ipfs/QmdSmp7g66BcoReSq4y4U2h27rh5YkeEkVWQDPE92oNi5p";
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:

            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(": Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                allGames = JsonHelper.GetArray<Game>(webRequest.downloadHandler.text);

                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

                StartCoroutine(GetText(allGames[0].link));
                // Debug.Log("Game    : " + allGames[1].name);
                // Debug.Log("Game    : " + allGames[1].category);
                // Debug.Log("Game    : " + allGames[1].link);
                break;
        }

        // if (webRequest.isNetworkError || webRequest.isHttpError)
        // {
        //     //show message "no internet " 
        //     Debug.Log("no internet ");
        // }
        // else
        // {
        //     if (webRequest.isDone)
        //     {
        //         allGames = JsonHelper.GetArray<Game>(webRequest.downloadHandler.text);
        //         Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

        //         Debug.Log("Game    : " + allGames[1].name);
        //         Debug.Log("Game    : " + allGames[1].category);
        //         Debug.Log("Game    : " + allGames[1].link);
        //     }
        // }
    }

    IEnumerator GetText(string file_name)
    {
        string url = file_name;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.Send();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Success");

                string savePath = string.Format("{0}/{1}.fbx", Application.persistentDataPath, file_name);

                Debug.Log("savePath  :" + savePath);

                // System.IO.File.WriteAllText(savePath, www.downloadHandler.text);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < heads.Length; i++)
        {
            if (i == currentHead)
            {
                heads[i].SetActive(true);
            }
            else
            {
                heads[i].SetActive(false);
            }
        }

        for (int i = 0; i < belts.Length; i++)
        {
            if (i == currentBelt)
            {
                belts[i].SetActive(true);
            }
            else
            {
                belts[i].SetActive(false);
            }
        }
    }

    public void SwitchHeads()
    {
        if (currentHead == heads.Length - 1)
        {
            currentHead = 0;
        }
        else
        {
            currentHead++;
        }
    }
    public void SwitchBelts()
    {
        if (currentBelt == belts.Length - 1)
        {
            currentBelt = 0;
        }
        else
        {
            currentBelt++;
        }
    }
}