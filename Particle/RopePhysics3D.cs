using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePhysics3D : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int segementCount = 42;
    public int constrairLoop = 72;
    public float segmentLength = 0.1f;
    public float ropeWidth = 0.1f;
    public Vector3 gravity = new Vector3(0, 0, -9.81f);
    
    public Transform startTransform;
    List<Segement> segments = new List<Segement>();

    private void Reset()
    {
        TryGetComponent(out lineRenderer);
    }

    private void Awake()
    {
        Vector3 segmentPos = startTransform.position;
        for(int i = 0; i < segementCount; i++)
        {
            Debug.Log("segmentPos : " + segmentPos);
            segments.Add(new Segement(segmentPos));
            segmentPos.z -= segmentLength;
        }
    }

    private void FixedUpdate()
    {
        UpdateSegments();
        for(int i = 0; i < constrairLoop; i++)
            ApplyConstraint();
        DrawRope();
    }

    void DrawRope()
    {
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        Vector3[] segmentPositions = new Vector3[segments.Count];
        for(int i = 0; i < segments.Count; i++)
        {
            segmentPositions[i] = segments[i].position;
        }
        lineRenderer.positionCount = segmentPositions.Length;
        lineRenderer.SetPositions(segmentPositions);
    }

    void UpdateSegments()
    {
        for(int i = 0; i < segments.Count; i++)
        {
            segments[i].velocity = segments[i].position - segments[i].previousPos;
            segments[i].previousPos = segments[i].position;
            segments[i].position += gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
            segments[i].position += segments[i].velocity;
        }
    }

    void ApplyConstraint()
    {
        segments[0].position = startTransform.position;
        for(int i = 0; i < segments.Count - 1; i++)
        {
            float distance = (segments[i].position - segments[i + 1].position).magnitude;
            float difference = segmentLength - distance;
            
            Vector3 dir = (segments[i + 1].position - segments[i].position).normalized;
            Vector3 movemnt = dir * difference;
            if (i == 0)
                segments[i + 1].position += movemnt * 0.5f;
            else
            {
                segments[i].position -= movemnt * 0.5f;
                segments[i + 1].position += movemnt * 0.5f;

            }
        }
    }

    public class Segement
    {
        public Vector3 previousPos;
        public Vector3 position;
        public Vector3 velocity;

        public Segement(Vector3 _position)
        {
            previousPos = _position;
            position = _position;
            velocity = Vector3.zero;
        }
    }
}
