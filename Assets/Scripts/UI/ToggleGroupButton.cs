using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupButton : Toggle
{
    [SerializeField] private Sprite FullSprite;
    [SerializeField] private Sprite PartialSprite;
    [SerializeField] private Sprite EmptySprite;
    [SerializeField] private Image CheckImage;
    [SerializeField] private Image BackgroundImage;

    public event Action<ToggleGroupButton, bool> OnToggledGroup;

    public TextMeshProUGUI Label;
    public CategoryGroup categoryGroup;
    public Transform Content;

    public List<ToggleButton> toggleButtons = new List<ToggleButton>();
    private FillState state = FillState.Full;

    public FillState State
    {
        get => state;
        private set
        {
            switch (value)
            {
                case FillState.Full:
                    CheckImage.sprite = FullSprite;
                    BackgroundImage.sprite = EmptySprite;
                    this.isOn = true;
                    break;
                case FillState.Partial:
                    CheckImage.sprite = PartialSprite;
                    BackgroundImage.sprite = PartialSprite;
                    break;
                case FillState.Empty:
                    CheckImage.sprite = PartialSprite;
                    BackgroundImage.sprite = EmptySprite;
                    this.isOn = false;
                    break;
            }
            state = value;
        }
    }

    protected override void Start()
    {
        base.Start();
        this.onValueChanged.AddListener(OnToggledGroupChanged);
    }
    public void ChangeState()
    {
        State = toggleButtons.All(button => button.isOn) ?
            FillState.Full : toggleButtons.All(button => !button.isOn) ?
            FillState.Empty : FillState.Partial;
    }

    private void OnToggledGroupChanged(bool isToggled)
    {
        OnToggledGroup?.Invoke(this, isToggled);
    }

    public enum FillState
    {
        Empty,
        Partial,
        Full
    }
}
