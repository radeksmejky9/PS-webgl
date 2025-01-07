using AsImpL;
using System;
using System.Collections.Generic;
using UnityEngine;


public class ModelManager : MonoBehaviour
{
    public static event Action<HashSet<Category>> OnModelsLoaded;
    public Dictionary<GameObject, List<string>> keyValuePairs;

    private ModelData currentModel;
    [SerializeField]
    private List<Transform> models = new List<Transform>();

    [SerializeField]
    private List<ModelData> modelDatas = new List<ModelData>();

    private void OnEnable()
    {
        ToggleButtonManager.OnCategoryToggled += OnCategoryToggled;
        ContentLoader.OnModelLoad += LoadModels;
        ContentLoader.OnModelLoadStart += OnModelLoadStart;
        ObjectImporter.ImportingStart += OnModelImportStart;
        ObjectImporter.ImportedModel += OnModelImported;
        ObjectBuilder.ObjectBuilt += OnObjectBuilt;
    }

    private void OnDisable()
    {
        ToggleButtonManager.OnCategoryToggled -= OnCategoryToggled;
        ContentLoader.OnModelLoad -= LoadModels;
        ContentLoader.OnModelLoadStart -= OnModelLoadStart;
        ObjectImporter.ImportingStart -= OnModelImportStart;
        ObjectImporter.ImportedModel -= OnModelImported;
        ObjectBuilder.ObjectBuilt -= OnObjectBuilt;

    }
    private void OnModelLoadStart()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        models.Clear();
        modelDatas.Clear();
    }

    private void OnModelImportStart()
    {
        currentModel = new ModelData(categories: ContentLoader.Instance.Categories);
        modelDatas.Add(currentModel);
    }
    private void OnModelImported(GameObject model, string path)
    {
        currentModel.Model = model;
        models.Add(model.transform);
        LoadModels(models);
        currentModel.ProccessFittings();
        currentModel = null;
    }

    private void OnObjectBuilt(GameObject element)
    {
        if (currentModel == null) return;

        currentModel.BuildElement(element);
    }

    private void EnableModels(SnappingPoint sp)
    {
        models.ForEach(model => { model.gameObject.SetActive(true); });
    }

    private void OnCategoryToggled(Category category, bool isActive)
    {
        foreach (var data in modelDatas)
        {
            if (isActive)
            {

                data.Filter.Add(category);
            }
            else
            {
                data.Filter.Remove(category);
            }
            UpdateVisibility(data);
        }
    }
    private void UpdateVisibility(ModelData data)
    {
        foreach (var element in data.GetElementsOfType<ModelElement>())
        {
            element.gameObject.SetActive(data.Filter.Contains(element.Category));
        }
    }

    private void LoadModels(List<Transform> models)
    {
        HashSet<Category> combinedCategories = new HashSet<Category>();

        foreach (var data in modelDatas)
        {
            combinedCategories.UnionWith(data.UsedCategories);
            data.Model.transform.parent = transform;
            data.Model.SetActive(true);
        }
        OnModelsLoaded?.Invoke(combinedCategories);
    }
}
