using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private UILevelSelectorItem itemPrefab;
    [SerializeField]
    private RectTransform itemsRoot;
    [SerializeField]
    private RectTransform circleSelector;
    [SerializeField]
    private Image imageRect;
    [SerializeField]
    private Animation animShake;


    [SerializeField]
    private Color selectedBG, nonSelectedBG, selectedPoint, nonSelectedPoint;

    public Color SelectedBGColor { get { return selectedBG; } }
    public Color NonSelectedBGColor { get { return nonSelectedBG; } }
    public Color SelectedPointColor { get { return selectedPoint; } }
    public Color NonSelectedPointColor { get { return nonSelectedPoint; } }

    int selected;
    List<UILevelSelectorItem> items;

    // Start is called before the first frame update
    void Awake()
    {
        items = new List<UILevelSelectorItem>();

        var img1 = circleSelector.GetChild(0).GetComponent<Image>();
        var img2 = circleSelector.GetChild(1).GetComponent<Image>();

        //DOTween.ToAlpha().SetLoops(-1);

    }

    public void AddLevel(PlanetController.Point point)
    {
        var i = Instantiate(itemPrefab, itemsRoot);
        i.Setup(this, point.customIcon, items.Count, point.name, false);
        items.Add(i);

        if (items.Count == 1)
            SelectLevel(0, false);
    }
    public void SelectLevel(int idx, bool shake = true)
    {
        items[selected].Deselect(this);
        selected = idx;
        items[selected].Select(this);
        var planet = FindObjectOfType<PlanetController>();
        planet.MoveCameraTo(idx, () =>
         {
             SetCirclePosition(planet.GetLastWatchedPointWorldPosition());
             planet.SetImageRect(this.imageRect, idx);
             planet.SetupCurveObject(idx);
         });
        if (shake)
        {
            var rt = this.imageRect.transform.parent as RectTransform;
            rt.DOPunchAnchorPos(new Vector2(50, 0), 0.1f);
        }
    }
    void SetCirclePosition(Vector3 worldPosition)
    {
        var screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        circleSelector.position = screenPos;
    }
}
