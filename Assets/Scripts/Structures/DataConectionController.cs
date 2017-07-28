using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataFlow { Upload, Download};

[RequireComponent(typeof(LineRenderer))]
public class DataConectionController : MonoBehaviour {
    public LineRenderer myLineRenderer;
    public float minWidth = 0.2f;
    public float maxWidth = 0.5f;
    public Color downloadColor;
    public Color uploadColor;

    void Awake ()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        CloseDataConnection();
    }

    public void SetupDataConection(Vector3 target, DataFlow direction)
    {
        myLineRenderer.positionCount = 3;
        myLineRenderer.SetPosition(0, transform.position);
        myLineRenderer.SetPosition(1, new Vector3(transform.position.x, target.y, transform.position.z));
        myLineRenderer.SetPosition(2, target);
        myLineRenderer.startColor = (direction == DataFlow.Upload) ? uploadColor : downloadColor;
        myLineRenderer.endColor = myLineRenderer.startColor;
    }
    public void AnimateDataConnection(float t)
    {
        if (t < 0.5f)
        {
            float newSize = Mathf.Lerp(minWidth, maxWidth, t * 2);
            myLineRenderer.startWidth = newSize;
            myLineRenderer.endWidth = newSize;
        }
        else
        {
            float newSize = Mathf.Lerp(maxWidth, minWidth, t - 0.5f);
            myLineRenderer.startWidth = newSize;
            myLineRenderer.endWidth = newSize;
        }
    }
    public void CloseDataConnection()
    {
        myLineRenderer.positionCount = 0;
    }
}
