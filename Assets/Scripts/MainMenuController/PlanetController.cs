using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlanetController : MonoBehaviour
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PlanetController))]
    public class PlanetControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Manual Setup"))
            {
                (target as PlanetController).ShowLines();
            }
        }
    }



    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 dir = Quaternion.Euler(points[i].eulerAngles) * Vector3.right;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(root.position + (dir * (root.localScale.x * 0.5f)), 0.2f);
        }
    }
#endif

    [System.Serializable]
    public class Point
    {
        public Vector3 eulerAngles;
        public string name;
        public Sprite sprite;
        public Sprite customIcon;
        public GameObject selectedPrefab;
    }

    private List<BezierCurve> curves;

    [SerializeField]
    private Transform root;
    [SerializeField]
    private BezierCurve curvePrefab;
    [SerializeField]
    private int lineDefinition = 10;
    [SerializeField]
    private Transform cameraPivot;

    [SerializeField]
    private LevelSelector levelSelector;

    [Header("Points:")]
    [SerializeField]
    private Point[] points;

    public int LastWatchedIdx { get; private set; }

    private void Awake()
    {
        curves = new List<BezierCurve>();

        ShowLines();
    }
    private void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose);
    }

    public void MoveCameraTo(int idx, TweenCallback onUpdate)
    {
        LastWatchedIdx = idx;
        cameraPivot.DORotate(points[idx].eulerAngles, 0.8f, RotateMode.Fast).OnUpdate(onUpdate);
    }
    public void SetImageRect(UnityEngine.UI.Image img, int idx)
    {
        img.sprite = points[idx].sprite;
    }
    public void SetupCurveObject(int idx)
    {
        for (int i = 0; i < curves.Count; i++)
            curves[i].SetupSelected(idx == i);
    }
   
    public Vector3 GetLastWatchedPointWorldPosition()
    {
        return curves[LastWatchedIdx].GetLookAtPosition();
    }

    public void ShowLines()
    {
        bool shouldReinitialize = curves.Count == 0;

        //first startup + setup objects
        for (int i = 0; i < points.Length; i++)
        {
            BezierCurve c = null;
            if (shouldReinitialize)
            {
                //curve object
                c = Instantiate(curvePrefab, root);
                curves.Add(c);

                //3d model
                c.InstantiateModel(points[i].selectedPrefab);
            }
            else
                c = curves[i];

            c.transform.localEulerAngles = points[i].eulerAngles;

            levelSelector.AddLevel(points[i]);

            if (i == 0)
                continue;

            c.Setup(curves[i - 1].GetLookAtPosition());
            c.ShowLine(root.position, lineDefinition);
        }
    }
}
