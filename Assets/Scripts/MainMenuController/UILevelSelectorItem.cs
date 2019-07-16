using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILevelSelectorItem : MonoBehaviour
{

    [SerializeField]
    private Text tfLevelName;
    [SerializeField]
    private Image selectedBG;
    [SerializeField]
    private Image point;
    [SerializeField]
    private Button selectButton;

    private Sprite myIcon;

    public void Setup(LevelSelector selector, Sprite iconSprite, int idx, string levelName, bool isSelected)
    {
        myIcon = iconSprite;

        tfLevelName.text = levelName;

        selectedBG.gameObject.SetActive(isSelected);
        //selectedBG.color = isSelected ? selector.SelectedBGColor : selector.NonSelectedBGColor;

        if (myIcon != null)
        {
            point.color = Color.white;
            point.sprite =
                myIcon;
        }
        else
        {
            point.color = isSelected ? selector.SelectedPointColor : selector.NonSelectedPointColor;
        }
        tfLevelName.color = isSelected ? Color.black : Color.white;

        selectButton.onClick.AddListener(() =>
        {
            selector.SelectLevel(idx);
            transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f, 10, 1f);
        });
    }

    public void Select(LevelSelector selector)
    {
        selectedBG.gameObject.SetActive(true);
        if (myIcon != null)
            point.sprite = myIcon;
        point.color = myIcon != null ? Color.white : selector.SelectedPointColor;
        tfLevelName.color = Color.black;
    }
    public void Deselect(LevelSelector selector)
    {
        selectedBG.gameObject.SetActive(false);
        if (myIcon != null)
            point.sprite = myIcon;
        point.color = myIcon != null ? Color.white : selector.NonSelectedPointColor;
        tfLevelName.color = Color.white;
    }
}
