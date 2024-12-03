using UnityEngine;

public class SnappingPointAttribute : PropertyAttribute
{
    public readonly bool positionEditable;
    public readonly bool rotationEditable;

    public SnappingPointAttribute(bool positionEditable = true, bool rotationEditable = true)
    {
        this.positionEditable = positionEditable;
        this.rotationEditable = rotationEditable;
    }
}
