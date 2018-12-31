using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochGenerator : MonoBehaviour
{
    protected enum Axiss // rotation axiss
    {
        XAxis,
        YAxis,
        Zaxis
    };
    [SerializeField]
    protected Axiss axiss = new Axiss();

    protected enum Initiator // shapes
    {
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octagon
    };
    [SerializeField]
    protected Initiator initiator = new Initiator();

    protected int initiatorPointAmount; // number of vertex

    private Vector3[] initiatorPoint; // vertex positions
    private Vector3 rotateVector; // vectors to apply rotation between vertex
    private Vector3 rotateAxis;
    private float initialRotation; // to make shape align on desired axis

    [SerializeField]
    protected float initiatorSize = 1f; // scale shape

    protected Vector3[] position;

    private void Awake()
    {
        GetInitiatorPoints();
        position = new Vector3[initiatorPointAmount + 1];

        // initiate private variables
        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector; // put initial rotation to allign with selected axis

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            position[i] = rotateVector * initiatorSize;
            // divide 360 by number of points to get complete edge shape , rotate on y axis
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }

        position[initiatorPointAmount] = position[0];
    }

    private void GetInitiatorPoints()
    {
        switch (initiator) // initiate number of vertex of each shape
        {
            case Initiator.Triangle:
                initiatorPointAmount = 3;
                initialRotation = 0;
                break;
            case Initiator.Square:
                initiatorPointAmount = 4;
                initialRotation = 45;
                break;
            case Initiator.Pentagon:
                initiatorPointAmount = 5;
                initialRotation = 36;
                break;
            case Initiator.Hexagon:
                initiatorPointAmount = 6;
                initialRotation = 30;
                break;
            case Initiator.Heptagon:
                initiatorPointAmount = 7;
                initialRotation = 25.71428f;
                break;
            case Initiator.Octagon:
                initiatorPointAmount = 8;
                initialRotation = 22.5f;
                break;
            default:
                break;
        }

        switch (axiss)
        {
            case Axiss.XAxis:
                rotateVector = new Vector3(1, 0, 0); // rotate shape vertex on z-axis
                rotateAxis = new Vector3(0, 0, 1); // rotate angle on y-axis
                break;
            case Axiss.YAxis:
                rotateVector = new Vector3(0, 1, 0); // rotate shape vertex on z-axis
                rotateAxis = new Vector3(1, 0, 0); // rotate angle on y-axis
                break;
            case Axiss.Zaxis:
                rotateVector = new Vector3(0, 0, 1); // rotate shape vertex on z-axis
                rotateAxis = new Vector3(0, 1, 0); // rotate angle on y-axis
                break;
            default:
                rotateVector = new Vector3(0, 1, 0); // rotate shape vertex on z-axis
                rotateAxis = new Vector3(1, 0, 0); // rotate angle on y-axis
                break;
        }
    }

    private void OnDrawGizmos()
    {
        GetInitiatorPoints();
        initiatorPoint = new Vector3[initiatorPointAmount];

        // initiate private variables
        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector; // put initial rotation to allign with selected axis

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            initiatorPoint[i] = rotateVector * initiatorSize;
            // divide 360 by number of points to get complete edge shape , rotate on y axis
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }

        // drawing lines
        for (int i = 0; i < initiatorPointAmount; i++)
        {
            Gizmos.color = Color.white;
            // to apply changes in transform from editor
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;

            if (i < initiatorPointAmount-1) // not last one
            {
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[i + 1]);
            }
            else // last one
            {
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[0]);
            }
        }
    }

}
