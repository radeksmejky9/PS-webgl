using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "ModelTypes", menuName = "ScriptableObjects/ModelTypes")]
public class ModelTypes : ScriptableObject
{
    public ModelType DefaultType;
    [SerializeField] private List<ModelType> types = new List<ModelType>();

    public List<ModelType> Types => types;

    public string GetRegexByType(string typeName)
    {
        foreach (var modelType in types)
        {
            if (modelType.Type == typeName)
            {
                return $"@\"{modelType.Regex}\"";
            }
        }
        return "";
    }

    public void AddModelType(ModelType modelType)
    {
        if (!types.Exists(t => t.Type == modelType.Type))
        {
            types.Add(modelType);
        }
        else
        {
            Debug.LogWarning($"ModelType with Type '{modelType.Type}' already exists.");
        }
    }
    public ModelType GetModelTypeByName(string elementName)
    {
        foreach (var modelType in Types)
        {
            if (Regex.IsMatch(elementName, modelType.Regex, RegexOptions.IgnoreCase))
            {
                return modelType;
            }
        }
        return DefaultType;
    }
}
