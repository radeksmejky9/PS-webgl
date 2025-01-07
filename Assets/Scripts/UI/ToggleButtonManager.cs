using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToggleButtonManager : MonoBehaviour
{
    public static event Action<Category, bool> OnCategoryToggled;
    public static event Action<Category> OnCategorySelected;

    public Transform parent;
    public CategoryButton categoryButtonPrefab;
    public GameObject toggleGroupButtonPrefab;
    public ToggleAllButton toggleAllButton;

    private List<ToggleGroupButton> toggleCategoryGroupButtons = new List<ToggleGroupButton>();
    private List<ToggleButton> toggleButtons = new List<ToggleButton>();
    private List<CategoryButton> categoryButtons = new List<CategoryButton>();

    private HashSet<Category> usedCategories;
    private bool displayAllCategories = false;

    [SerializeField]
    private CategoryButton currentCategorySelected = null;

    private void OnEnable()
    {
        toggleAllButton.OnToggleAll += OnToggleAll;
        ModelManager.OnModelsLoaded += CreateCategoryMenu;
    }

    private void OnDisable()
    {
        toggleAllButton.OnToggleAll -= OnToggleAll;
        ModelManager.OnModelsLoaded -= CreateCategoryMenu;
    }

    public void OnDisplayAllCategories(bool displayAll)
    {
        displayAllCategories = displayAll;
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }

        if (displayAllCategories)
        {
            CreateCategoryMenu(ContentLoader.Instance.Categories.ToHashSet());
        }
        else
        {
            CreateCategoryMenu(usedCategories);
        }
    }

    private void CreateCategoryMenu(HashSet<Category> usedCategories)
    {
        if (!displayAllCategories)
            this.usedCategories = usedCategories;

        foreach (CategoryGroup group in ContentLoader.Instance.CategoryGroups)
        {
            var groupCategories = usedCategories.Where(category => category.CategoryGroup == group);
            if (!groupCategories.Any()) continue;

            GameObject toggleGroupButton = Instantiate(toggleGroupButtonPrefab, parent);
            ToggleGroupButton groupToggleObj = toggleGroupButton.GetComponentInChildren<ToggleGroupButton>();
            groupToggleObj.Label.text = group.ToString();
            groupToggleObj.OnToggledGroup += OnCategoryGroupToggled;
            groupToggleObj.categoryGroup = group;
            toggleCategoryGroupButtons.Add(groupToggleObj);

            foreach (var category in groupCategories)
            {
                CategoryButton categoryButton = Instantiate(categoryButtonPrefab, groupToggleObj.Content.transform);
                categoryButton.ToggleButton.Label.text = category.ToString();
                categoryButton.ToggleButton.category = category;
                categoryButton.ToggleButton.categoryGroup = category.CategoryGroup;
                categoryButton.ToggleButton.OnToggleChanged += OnCategoryToggle;
                categoryButton.OnSelect += CategorySelected;
                toggleButtons.Add(categoryButton.ToggleButton);
                categoryButtons.Add(categoryButton);
                groupToggleObj.toggleButtons.Add(categoryButton.ToggleButton);
            }
        }

        foreach (var category in usedCategories.Where(c => c.CategoryGroup == null))
        {
            CategoryButton categoryButton = Instantiate(categoryButtonPrefab, parent);
            categoryButton.ToggleButton.Label.text = category.ToString();
            categoryButton.ToggleButton.category = category;
            categoryButton.ToggleButton.categoryGroup = null;
            categoryButton.ToggleButton.OnToggleChanged += OnCategoryToggle;
            categoryButton.OnSelect += CategorySelected;
            categoryButtons.Add(categoryButton);
            toggleButtons.Add(categoryButton.ToggleButton);
        }
    }
    private void OnCategoryToggle(Category category, CategoryGroup categoryGroup, bool isToggled)
    {
        OnCategoryToggled?.Invoke(category, isToggled);
        toggleCategoryGroupButtons.ForEach(button =>
        {
            if (button.categoryGroup == categoryGroup)
            {
                button.ChangeState();
            }
        });
        toggleAllButton.ChangeState(toggleButtons);
    }
    private void OnCategoryGroupToggled(ToggleGroupButton groupButton, bool isToggled)
    {
        groupButton.toggleButtons.ForEach(button => button.isOn = isToggled);
        groupButton.ChangeState();
        toggleAllButton.ChangeState(toggleButtons);
    }
    private void OnToggleAll(bool isToggled)
    {
        toggleButtons.ForEach(button => button.isOn = isToggled);
    }
    private void CategorySelected(CategoryButton btn)
    {
        if (currentCategorySelected == btn)
        {
            currentCategorySelected.IsSelected = false;
            currentCategorySelected = null;
            OnCategorySelected?.Invoke(null);
        }
        else
        {
            if (currentCategorySelected != null)
            {
                currentCategorySelected.IsSelected = false;
            }

            currentCategorySelected = btn;
            currentCategorySelected.IsSelected = true;
            OnCategorySelected?.Invoke(btn.ToggleButton.category);
        }
    }

}
