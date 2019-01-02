using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // for Enum Class

[RequireComponent(typeof(LineRenderer))]
public class KochLine : KochGenerator
{
    [Range(0,1)]
    public float lerpAmount;
    public float generateMultiplier;

    private LineRenderer lineRenderer;
    private Vector3[] lerpPosition;

    [Header("UI")]
    public Dropdown dropdownShape;
    public Slider GeneratorMultiplierSlider; // generator multiplier
    public Text SliderText;
    public Slider LerpSlider; // lerp amount
    public Text SliderTextLerp;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // initialize line rendrer
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);

        // UI Values
        MakeDropDownList();
        UpdateNextGenerationSize();
        UpdateLerpSliderValues();
    }

    private void OnValidate()
    {
        /*
        GetInitiatorPointsPublic();
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);*/
        UpdateLineCurves();
    }

    private void Update()
    {
       
        //UpdateLineCurves();

        if (Input.GetKeyUp(KeyCode.O))
        {
            Outwards();
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            Inwards();
        }
    }

    void UpdateLineCurves()
    {
        if (generationCount != 0)
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
    }

    public void MakeDropDownList()
    {
        string[] enumValues = Enum.GetNames(typeof(Initiator));
        List<String> names = new List<string>(enumValues);

        dropdownShape.AddOptions(names);
    }

    public void ChangeShape()
    {
        //Debug.Log(dropdownShape.value);
        initiator = (Initiator)dropdownShape.value;
        GetInitiatorPointsPublic();
        UpdateShapeVariables();
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);
    }

    public void SliderChangeNextSize() // change multiplier scale value
    {
        generateMultiplier = GeneratorMultiplierSlider.value;
        SliderText.text = "Generator Size : " + generateMultiplier.ToString();
    }

    void UpdateNextGenerationSize()
    {
        SliderText.text = generateMultiplier.ToString();
        GeneratorMultiplierSlider.value = generateMultiplier;
    }

    public void SliderLerp()
    {
        lerpAmount = LerpSlider.value;
        SliderTextLerp.text = "Current Size : " + lerpAmount.ToString();
        UpdateLineCurves();
    }

    void UpdateLerpSliderValues()
    {
        SliderTextLerp.text = "Current Size : " + lerpAmount.ToString();
        LerpSlider.value = lerpAmount;
    }

    public void Inwards()
    {
        KochGenerate(targetPosition, false, generateMultiplier);
        lerpPosition = new Vector3[position.Length];
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);
        lerpAmount = 0.5f;

        UpdateLerpSliderValues();
        UpdateLineCurves();
    }

    public void Outwards()
    {
        KochGenerate(targetPosition, true, generateMultiplier);
        lerpPosition = new Vector3[position.Length];
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);
        lerpAmount = 0.5f;

        UpdateLerpSliderValues();   
        UpdateLineCurves();
    }
}
