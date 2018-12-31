using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerFractale : MonoBehaviour
{
    public Material material;
    public Vector2 position;
    public float scale,angle;

    private Vector2 smoothPos;
    private float smoothScale, smoothAngle;

    private void FixedUpdate()
    {
        HandleInput();
        UpdateShader();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            scale *= 0.99f;
        }
        else if (Input.GetKey(KeyCode.KeypadMinus))
        {
            scale *= 1.01f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            angle -= 0.01f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            angle += 0.01f;
        }

        Vector2 direction = new Vector2(0.01f * scale,0f);
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);

        direction = new Vector2(direction.x * c , direction.x * s );

        if (Input.GetKey(KeyCode.A))
        {
            position -= direction;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            position += direction;
        }

        direction = new Vector2(-direction.y, direction.x);

        if (Input.GetKey(KeyCode.S))
        {
            position -= direction;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            position += direction;
        }
    }

    private void UpdateShader()
    {
        smoothPos = Vector2.Lerp(smoothPos, position, 0.03f);
        smoothScale = Mathf.Lerp(smoothScale, scale, 0.03f);
        smoothAngle = Mathf.Lerp(smoothAngle, angle, 0.03f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if (aspectRatio > 1f) // take less on y direction
        {
            scaleY /= aspectRatio;
        }
        else
        {
            scaleX *= aspectRatio;
        }

        material.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        material.SetFloat("_Angle", smoothAngle);
    }
}
