using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KochLineAudio : KochGeneratorAudio
{
    //[Range(0,1)]
    //public float lerpAmount;
    public float generateMultiplier;

    private LineRenderer lineRenderer;
    private Vector3[] lerpPosition;
    private float[] lerpAudio;

    [Header("Audio")]
    public AudioPeer audioPeer;
    public int[] audioBand;
    public Material material;
    public Color color;
    private Material materialInstance; // 
    public int audioBandMaterial;
    public float emissionMultipler;

    private void Start()
    {
        lerpAudio = new float[initiatorPointAmount];
        lineRenderer = GetComponent<LineRenderer>();
        // initialize line rendrer
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);
        lerpPosition = new Vector3[position.Length];

        //apply material
        materialInstance = new Material(material);
        lineRenderer.material = materialInstance;
    }

    private void Update()
    {
        materialInstance.SetColor("_EmissionColor", color * AudioPeer.audioBandBuffer[audioBandMaterial] * emissionMultipler);


        if (generationCount !=0)
        {
            int count =0;
            for (int i = 0; i < initiatorPointAmount; i++)
            {
                lerpAudio[i] = AudioPeer.audioBandBuffer[audioBand[i]];
                for (int j = 0; j < (position.Length-1)/initiatorPointAmount; j++)
                {
                    lerpPosition[count] = Vector3.Lerp(position[count], targetPosition[count], lerpAudio[i]);
                    count++;
                }
            }
            // last point
            lerpPosition[count] = Vector3.Lerp(position[count], targetPosition[count], lerpAudio[initiatorPointAmount-1]);

            /*
            for (int i = 0; i < position.Length; i++)
            {
                // lerpPosition[i] = Vector3.Lerp(position[i], targetPosition[i], lerpAmount);
                lerpPosition[i] = Vector3.Lerp(position[i], targetPosition[i], AudioPeer.audioBandBuffer[audioBand]);
            }*/

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

        /*
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
        */
    }
}
