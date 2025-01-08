using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


public class ModelData
{
    private readonly List<Category> categories;
    private readonly List<ModelElement> elements = new List<ModelElement>();
    private readonly HashSet<Category> usedCategories = new HashSet<Category>();

    private GameObject model;
    private List<Category> filter;
    private ModelTypes modelTypes;

    public GameObject Model { get => model; set => model = value; }
    public List<Category> Filter { get => filter; set => filter = value; }
    public HashSet<Category> UsedCategories => usedCategories;
    public List<ModelElement> GetElementsOfType(ModelType modelType)
    {
        return elements.Where(e => e.ModelType == modelType).ToList();
    }
    public List<ModelElement> GetElementsOfCategory(Category category)
    {
        return elements.Where(e => e.Category == category).ToList();
    }
    public List<ModelElement> GetElements()
    {
        return elements;
    }

    public ModelData(ModelTypes modelTypes, List<Category> categories = null)
    {
        this.categories = categories ?? new List<Category> { };
        this.Filter = this.categories.ToList();
        this.modelTypes = modelTypes;
    }

    public void BuildElement(GameObject element)
    {
        Transform elementTransform = element.transform;
        if (element.TryGetComponent<ModelElement>(out _)) return;

        string elementName = element.name;
        ModelType modelType = modelTypes.GetModelTypeByName(elementName);

        if (Regex.IsMatch(elementName, modelType.Regex, RegexOptions.IgnoreCase))
        {
            Match match = Regex.Match(elementName, @":([^:]+):", RegexOptions.IgnoreCase);
            switch (modelType.Type)
            {
                case "Pipe":
                    CreatePipe(match, elementTransform, modelType);
                    break;

                case "Connection":
                    CreateFitting(match, elementTransform, modelType);
                    break;

                case "Wall":
                    CreateGenericCategory(match, elementTransform, "Wall", modelType);
                    break;

                case "Door":
                    CreateGenericCategory(match, elementTransform, "Door", modelType);
                    break;

                case "Window":
                    CreateGenericCategory(match, elementTransform, "Window", modelType);
                    break;

                default:
                    CreateGenericCategory(match, elementTransform, "Environment", modelType);
                    break;
            }
        }
        else
        {
            CreateGenericCategory(Regex.Match(elementName, string.Empty, RegexOptions.IgnoreCase), elementTransform, "Environment", modelType);
        }
    }

    private void CreateGenericCategory(Match match, Transform element, string categoryType, ModelType modelType)
    {
        element.gameObject.AddComponent<BoxCollider>();
        ModelElement modelElement = element.gameObject.AddComponent<ModelElement>();
        modelElement.ModelType = modelType;

        string elementCategory = match.Groups[1].Value.Replace(" ", "");
        elementCategory = elementCategory.Replace("_", "");

        if (categories.Exists(category => string.Equals(category.ToString(), elementCategory, StringComparison.OrdinalIgnoreCase)))
        {
            AddCategory(modelElement, elementCategory);
        }
        else
        {
            AddCategory(modelElement, categoryType);
        }
        usedCategories.Add(modelElement.Category);
        elements.Add(modelElement);
    }

    private void CreatePipe(Match match, Transform element, ModelType modelType)
    {
        element.gameObject.AddComponent<BoxCollider>();
        ModelElement pipe = element.gameObject.AddComponent<ModelElement>();
        pipe.ModelType = modelType;

        string pipeCategory = match.Groups[1].Value.Replace(" ", "");
        pipeCategory = pipeCategory.Replace("_", "");

        if (categories.Exists(category => string.Equals(category.ToString(), pipeCategory, StringComparison.OrdinalIgnoreCase)))
        {
            AddCategory(pipe, pipeCategory);
        }
        else
        {
            AddCategory(pipe, "Undefined");
        }
    }

    private void CreateFitting(Match match, Transform element, ModelType modelType)
    {
        element.gameObject.AddComponent<BoxCollider>();
        ModelElement fitting = element.gameObject.AddComponent<ModelElement>();
        fitting.ModelType = modelType;

        string fittingCategory = match.Groups[1].Value.Replace(" ", "");
        fittingCategory = fittingCategory.Replace("_", "");

        if (categories.Exists(category => string.Equals(category.ToString(), fittingCategory, StringComparison.OrdinalIgnoreCase)))
        {
            AddCategory(fitting, fittingCategory);
        }
        else
        {
            ModelElement closestPipe = fitting.transform.GetClosestPipe(GetElementsOfType(modelTypes.GetModelTypeByName("Pipe")).ToArray());
            if (closestPipe != null)
            {
                AddCategory(fitting, closestPipe.Category.name);
            }
            else
            {
                AddCategory(fitting, "Undefined");
            }
        }
    }
    private Category FindCategory(string categoryName)
    {
        return categories.FirstOrDefault(c => c.name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
    }
    private void AddCategory<T>(T element, string categoryName = "", Category category = null) where T : ModelElement
    {
        element.Category = category ?? FindCategory(categoryName) ?? FindCategory("Undefined");
        usedCategories.Add(element.Category);
        elements.Add(element);
    }

    public void ProccessFittings()
    {
        var fittings = GetElementsOfType(modelTypes.GetModelTypeByName("Fitting"));
        var pipes = GetElementsOfType(modelTypes.GetModelTypeByName("Pipe")).ToArray();
        foreach (var element in fittings)
        {
            ModelElement closestPipe = element.transform.GetClosestPipe(pipes);
            if (closestPipe != null)
            {
                AddCategory(element, category: closestPipe.Category);
            }
        }
    }
}
