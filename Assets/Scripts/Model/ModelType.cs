using UnityEngine;

[CreateAssetMenu(fileName = "NewModelType", menuName = "Model Types/Model Type")]
public class ModelType : ScriptableObject
{
    public string Type;
    [TextArea]
    public string Regex;

    public ModelElement Create()
    {
        ModelElement element = new ModelElement();
        element.ModelType = this;
        return element;
    }
}