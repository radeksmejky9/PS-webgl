using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAllButton : Toggle
{
    [SerializeField] private Sprite FullSprite;
    [SerializeField] private Sprite PartialSprite;
    [SerializeField] private Sprite EmptySprite;
    [SerializeField] private Image CheckImage;
    [SerializeField] private Image BackgroundImage;

    public event Action<bool> OnToggleAll;

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
    public void ChangeState(List<ToggleButton> toggleButtons)
    {
        State = toggleButtons.All(button => button.isOn) ?
            FillState.Full : toggleButtons.All(button => !button.isOn) ?
            FillState.Empty : FillState.Partial;
    }

    private void OnToggledGroupChanged(bool isToggled)
    {
        OnToggleAll?.Invoke(isToggled);
    }

    public enum FillState
    {
        Empty,
        Partial,
        Full
    }
}
