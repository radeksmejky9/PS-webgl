using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.UI;


public class Uploader : MonoBehaviour
{
    public ModelManager manager;
    public string serverURL;

    private Button saveButton;
    private void Start()
    {
        saveButton = GetComponent<Button>();
    }
    public void DoExport()
    {
        List<UpdateInfo> updatesList = new List<UpdateInfo>();

        foreach (ModelElement modelElement in manager.ElementUpdates)
        {
            UpdateInfo update = new UpdateInfo
            {
                name = modelElement.name,
                id = modelElement.ID
            };
            updatesList.Add(update);
        }

        string json = JsonConvert.SerializeObject(new { updates = updatesList }, Formatting.Indented);



        StartCoroutine(Upload(json, $"{serverURL}/{ContentLoader.Instance.UID}"));
    }

    private IEnumerator Upload(string updateJSON, string serverURL)
    {
        saveButton.interactable = false;

        using UnityWebRequest www = UnityWebRequest.Put(serverURL, updateJSON);
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload successful: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error uploading: " + www.error);
        }

        saveButton.interactable = true;
    }
}

[System.Serializable]
public class UpdateInfo
{
    public string name;
    public int id;
}