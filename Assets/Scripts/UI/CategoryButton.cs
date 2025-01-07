using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton : Toggle
{
    public new event Action<CategoryButton> OnSelect;
    public ToggleButton ToggleButton;

    [SerializeField]
    private Image SelectionPanel;
    private bool isSelected = false;

    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            SelectionPanel.color = isSelected ? Color.red : new Color(1, 1, 1, 0);
        }
    }

    protected override void Start()
    {
        base.Start();
        this.onValueChanged.AddListener(OnToggleValueChanged);
    }
    private void OnToggleValueChanged(bool isToggled)
    {
        IsSelected = isToggled;
        OnSelect?.Invoke(this);
    }
}
