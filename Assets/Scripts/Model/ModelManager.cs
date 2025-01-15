using AsImpL;
using System;
using System.Collections.Generic;
using UnityEngine;


public class ModelManager : MonoBehaviour
{
    public static event Action<HashSet<Category>> OnModelsLoaded;
    public ModelTypes ModelTypes;

    [SerializeField] private Transform model = null;
    [SerializeField] private ModelData modelData = null;

    [SerializeField]
    private List<ModelElement> elementUpdates = new List<ModelElement>();
    public List<ModelElement> ElementUpdates => elementUpdates;

    private void OnEnable()
    {
        ToggleButtonManager.OnCategoryToggled += OnCategoryToggled;
        ContentLoader.OnModelLoad += LoadModel;
        ContentLoader.OnModelLoadStart += OnModelLoadStart;
        ObjectImporter.ImportingStart += OnModelImportStart;
        ObjectImporter.ImportedModel += OnModelImported;
        ObjectBuilder.ObjectBuilt += OnObjectBuilt;
        Pointer.OnModelElementUpdated += OnModelElementUpdated;
    }

    private void OnDisable()
    {
        ToggleButtonManager.OnCategoryToggled -= OnCategoryToggled;
        ContentLoader.OnModelLoad -= LoadModel;
        ContentLoader.OnModelLoadStart -= OnModelLoadStart;
        ObjectImporter.ImportingStart -= OnModelImportStart;
        ObjectImporter.ImportedModel -= OnModelImported;
        ObjectBuilder.ObjectBuilt -= OnObjectBuilt;
        Pointer.OnModelElementUpdated -= OnModelElementUpdated;

    }
    private void OnModelLoadStart()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        model = null;
        modelData = null;
    }

    private void OnModelImportStart()
    {
        modelData = new ModelData(ModelTypes, categories: ContentLoader.Instance.Categories);
    }

    private void OnModelImported(GameObject model, string path)
    {
        modelData.Model = model;
        this.model = model.transform;
        LoadModel(this.model);
        modelData.ProccessFittings();
    }

    private void OnObjectBuilt(GameObject element)
    {
        if (modelData == null) return;

        modelData.BuildElement(element);
    }

    private void OnCategoryToggled(Category category, bool isActive)
    {
        if (isActive)
        {
            modelData.Filter.Add(category);
        }
        else
        {
            modelData.Filter.Remove(category);
        }

        foreach (var element in modelData.GetElements())
        {
            element.gameObject.SetActive(modelData.Filter.Contains(element.Category));
        }
    }

    private void LoadModel(Transform model)
    {
        modelData.Model.transform.parent = transform;
        modelData.Model.SetActive(true);
        OnModelsLoaded?.Invoke(modelData.UsedCategories);
    }
    private void OnModelElementUpdated(ModelElement element)
    {
        if (!elementUpdates.Contains(element))
            elementUpdates.Add(element);
    }
}
