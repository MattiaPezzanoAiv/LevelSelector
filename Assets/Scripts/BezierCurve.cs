using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BezierCurve : MonoBehaviour
{
    private Vector3 p0, p1;
    private LineRenderer line;

    [SerializeField]
    private float deltaDistanceHeight;
    [SerializeField]
    private Transform sphere;
    [SerializeField]
    private Transform modelParent;

    private GameObject prefabInstance;

    private void Awake()
    {
        this.line = GetComponent<LineRenderer>();
    }

    public void Setup(Vector3 previousPoint)
    {
        p0 = sphere.position;
        p1 = previousPoint;
    }

    public void ShowLine(Vector3 sphereCenter, int nPoints)
    {
        var half = Vector3.Lerp(p1, p0, 0.5f);
        var dir = (half - sphereCenter).normalized;

        Vector3 middlePoint = half + dir * (deltaDistanceHeight * Vector3.Distance(p0, p1));

        Vector3[] points = new Vector3[nPoints];

        line.positionCount = nPoints + 1;
        for (int i = 0; i < nPoints; i++)
        {
            float t = i / (float)nPoints;
            float oneMt = 1 - t;
            var res = oneMt * (oneMt * p0 + (t * middlePoint));
            res += t * (oneMt * middlePoint + (t * p1));
            line.SetPosition(i, res);
        }
        line.SetPosition(nPoints, p1);
    }

    public void SetupSelected(bool selected)
    {
        sphere.gameObject.SetActive(!selected);
        prefabInstance.SetActive(selected);
    }
    public Vector3 GetLookAtPosition()
    {
        return prefabInstance.transform.position;
    }
    public void InstantiateModel(GameObject prefab)
    {
        prefabInstance = Instantiate(prefab, modelParent);
        prefabInstance.transform.up = sphere.right;
        prefabInstance.transform.localPosition = Vector3.zero;
    }
}
