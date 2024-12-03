using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : Toggle
{
    [SerializeField]
    private Image Image;

    public event Action<Category, CategoryGroup, bool> OnToggleChanged;
    public TextMeshProUGUI Label;
    public Category category;
    public CategoryGroup categoryGroup;


    protected override void Start()
    {
        base.Start();
        Image.color = category.Material.color;
        this.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isToggled)
    {
        OnToggleChanged?.Invoke(category, categoryGroup, isToggled);
    }
}
