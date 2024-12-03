using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collapse : MonoBehaviour
{
    [SerializeField] private GameObject categoryContent;
    [SerializeField] private RectTransform categoryContainerRectTransform;
    [SerializeField] private GameObject collapseButton;
    [SerializeField] private RectTransform arrowTransform;

    private RectTransform parentRectTransform;
    private RectTransform categoryContentRectTransform;
    private List<GameObject> children = new List<GameObject>();
    private bool isCollapsed = true;

    private void Start()
    {
        foreach (Transform child in categoryContent.transform)
        {
            children.Add(child.gameObject);
        }
        categoryContentRectTransform = categoryContent.GetComponent<RectTransform>();
        parentRectTransform = categoryContainerRectTransform.GetComponentInParent<RectTransform>();
    }

    public void ToggleCollapse()
    {
        ToggleChildrenVisibility(isCollapsed);
        RotateArrow(isCollapsed);
        isCollapsed = !isCollapsed;

        LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(categoryContainerRectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(categoryContentRectTransform);
    }

    private void ToggleChildrenVisibility(bool collapsing)
    {
        foreach (var child in children)
        {
            child.SetActive(!collapsing);
        }
    }

    private void RotateArrow(bool collapsing)
    {
        float rotationAngle = collapsing ? 0 : -90;
        arrowTransform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }
}
