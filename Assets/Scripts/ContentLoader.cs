using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using AsImpL;

public class ContentLoader : MonoSingleton<ContentLoader>
{
    public static event Action<Transform> OnModelLoad;
    public static event Action OnModelLoadStart;
    public static Action OnDownloadStart;
    public static Action OnDownloadEnd;
    public static Action<float> OnDownloadProgressChanged;

    public List<Category> Categories
    {
        get
        {
            if (loadedCategories == null)
            {
                Debug.LogError("Categories accessed before initialization!");
            }
            Debug.Log($"Accessing Categories: {loadedCategories?.Count ?? 0} items");
            return loadedCategories;
        }
    }
    public List<CategoryGroup> CategoryGroups => loadedCategoryGroups;

    public string UID { get => uid; set => uid = value; }


    private MultiObjectImporter MultiObjectImporter;
    private List<Category> loadedCategories = new List<Category>();
    private List<CategoryGroup> loadedCategoryGroups = new List<CategoryGroup>();
    private string uid = string.Empty;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadDataCoroutine());
        MultiObjectImporter = this.GetComponent<MultiObjectImporter>();
    }

    private IEnumerator LoadDataCoroutine()
    {
        yield return LoadDataAsync("Category Group", loadedCategoryGroups);
        yield return LoadDataAsync("Category", loadedCategories);

#if UNITY_EDITOR
        LoadModelFromJS("6787b20984593d6323e49369,http://192.168.37.142:5000/files/6787b20984593d6323e49369/download/obj");
#endif
    }

    public IEnumerator LoadDataAsync<T>(string label, List<T> list)
    {
        AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);

        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            list.AddRange(handle.Result);
            Debug.Log($"Loaded {list.Count} items labeled '{label}'");
        }
        else
        {
            Debug.LogError($"Failed to load items with the label '{label}'");
        }
    }

    public void LoadModelFromJS(string message)
    {
        string[] data = message.Split(",");
        OnModelLoadStart?.Invoke();
        this.UID = data[0];
        MultiObjectImporter.ImportModelAsync(uid, data[1], this.transform, MultiObjectImporter.defaultImportOptions);
    }
}

