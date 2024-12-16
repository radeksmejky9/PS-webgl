using UnityEngine;

public class Fitting : ModelElement
{
    public Pipe closestPipe;

    public override void OnClick()
    {
        closestPipe = null;
        base.OnClick();
    }
    protected override void Rename()
    {
        if (closestPipe != null)
            this.gameObject.name = $"{GetType().Name}:{closestPipe.Category.Material.name}:";
        else
            base.Rename();
    }
}