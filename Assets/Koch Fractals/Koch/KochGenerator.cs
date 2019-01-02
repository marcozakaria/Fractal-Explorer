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

    public struct LineSegment // line parameters
    {
        public Vector3 StartPostition { get; set; }
        public Vector3 EndPosition { get; set; }
        public Vector3 Direction { get; set; }
        public float Length { get; set; }
    }

    [SerializeField] // for the shape that will be created at each line
    protected AnimationCurve generator;
    protected Keyframe[] keys;
    protected int generationCount;

    [SerializeField]
    protected bool useBezierCurve;
    [SerializeField]
    [Range(8,24)]
    protected int bezierVertexCount;

    protected int initiatorPointAmount; // number of vertex

    private Vector3[] initiatorPoint; // vertex positions
    private Vector3 rotateVector; // vectors to apply rotation between vertex
    private Vector3 rotateAxis;
    private float initialRotation; // to make shape align on desired axis

    [SerializeField]
    protected float initiatorSize = 1f; // scale shape

    protected Vector3[] position;
    protected Vector3[] targetPosition;
    protected Vector3[] bezierPosition;
    private List<LineSegment> lineSegments;

    protected Vector3[] BezierCurve(Vector3[] points,int vertexCount)
    {
        var pointList = new List<Vector3>();
        for (int i = 0; i < points.Length; i+=2)
        {
            if (i+2 <= points.Length-1) // not last
            {
                for (float ratio = 0; ratio <= 1; ratio+=1.0f/vertexCount)
                {
                    var tangentLineVertex1 = Vector3.Lerp(points[i], points[i + 1], ratio);
                    var tangentLineVertex2 = Vector3.Lerp(points[i + 1], points[i + 2], ratio);
                    var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                    pointList.Add(bezierPoint);
                }
            }
        }

        return pointList.ToArray();
    }

    private void Awake()
    {
        GetInitiatorPoints();
        UpdateShapeVariables();
    }

    protected void UpdateShapeVariables()
    {
        //a assign list and arrays
        position = new Vector3[initiatorPointAmount + 1];
        targetPosition = new Vector3[initiatorPointAmount + 1];
        lineSegments = new List<LineSegment>();
        keys = generator.keys;

        // initiate private variables
        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector; // put initial rotation to allign with selected axis

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            position[i] = rotateVector * initiatorSize;
            // divide 360 by number of points to get complete edge shape , rotate on y axis
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }

        position[initiatorPointAmount] = position[0];
        targetPosition = position;
    }

    protected void KochGenerate(Vector3[] positions, bool outWards, float generatorMultiplier)
    {
        // creating lineSegments
        lineSegments.Clear();
        for (int i = 0; i < position.Length-1; i++)
        {
            LineSegment line = new LineSegment();
            line.StartPostition = positions[i];
            if (i == positions.Length-1) // last position
            {
                line.EndPosition = positions[0];
            }
            else
            {
                line.EndPosition = positions[i + 1];
            }
            line.Direction = (line.EndPosition - line.StartPostition).normalized;
            line.Length = Vector3.Distance(line.EndPosition, line.StartPostition);

            lineSegments.Add(line);
        }

        // add line segments points to a point array
        List<Vector3> newPos = new List<Vector3>();
        List<Vector3> targetPos = new List<Vector3>();

        for (int i = 0; i < lineSegments.Count; i++)
        {
            newPos.Add(lineSegments[i].StartPostition); // add first
            targetPos.Add(lineSegments[i].StartPostition);
            // loop on all keypoints created from generator
            for (int j = 1; j < keys.Length-1; j++)
            {
                float moveAmount = lineSegments[i].Length * keys[j].time; // time is x-axis
                float heightAmount = (lineSegments[i].Length * keys[j].value) * generatorMultiplier; // value is y_axis
                Vector3 movePos = lineSegments[i].StartPostition + (lineSegments[i].Direction * moveAmount);
                Vector3 direction;
                if (outWards)
                {
                    direction = Quaternion.AngleAxis(-90, rotateAxis) * lineSegments[i].Direction;
                }
                else
                {
                    direction = Quaternion.AngleAxis(90, rotateAxis) * lineSegments[i].Direction;
                }
                newPos.Add(movePos);
                targetPos.Add(movePos + (direction * heightAmount)); 
            }
        }
        newPos.Add(lineSegments[0].StartPostition); // add last
        targetPos.Add(lineSegments[0].StartPostition);
        position = new Vector3[newPos.Count];
        targetPosition = new Vector3[targetPos.Count];

        bezierPosition = BezierCurve(targetPosition, bezierVertexCount);

        position = newPos.ToArray();
        targetPosition = targetPos.ToArray();

        generationCount++;
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

    public void GetInitiatorPointsPublic()
    {
        GetInitiatorPoints();
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
