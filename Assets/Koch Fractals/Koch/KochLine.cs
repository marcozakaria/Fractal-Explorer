using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KochLine : KochGenerator
{
    [Range(0,1)]
    public float lerpAmount;
    public float generateMultiplier;

    private LineRenderer lineRenderer;
    private Vector3[] lerpPosition;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // initialize line rendrer
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);
    }

    private void Update()
    {
        if (generationCount !=0)
        {
            for (int i = 0; i < position.Length; i++)
            {
                lerpPosition[i] = Vector3.Lerp(position[i], targetPosition[i], lerpAmount);
            }

            if (useBezierCurve)
            {
                bezierPosition = BezierCurve(lerpPosition, bezierVertexCount);
                lineRenderer.positionCount = bezierPosition.Length;
                lineRenderer.SetPositions(bezierPosition);
            }
            else
            {
                lineRenderer.positionCount = lerpPosition.Length;
                lineRenderer.SetPositions(lerpPosition);
            }

            
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            KochGenerate(targetPosition, true, generateMultiplier);
            lerpPosition = new Vector3[position.Length];
            lineRenderer.positionCount = position.Length;
            lineRenderer.SetPositions(position);
            lerpAmount = 0;
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            KochGenerate(targetPosition, false, generateMultiplier);
            lerpPosition = new Vector3[position.Length];
            lineRenderer.positionCount = position.Length;
            lineRenderer.SetPositions(position);
            lerpAmount = 0;
        }
    }
}
