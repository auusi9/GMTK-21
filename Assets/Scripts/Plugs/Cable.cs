using System.Collections.Generic;
using UnityEngine;

namespace Cable
{
    public class Cable : MonoBehaviour
    {
        [SerializeField] private Transform StartPoint;
        [SerializeField] private Transform EndPoint;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float ropeSegLen = 0.25f;
        [SerializeField] private int segmentLength = 35;
        [SerializeField] private float lineWidth = 0.1f;
        private List<RopeSegment> ropeSegments = new List<RopeSegment>();


        private void Start()
        {
            this.lineRenderer = this.GetComponent<LineRenderer>();
            Vector3 ropeStartPoint = StartPoint.position;

            for (int i = 0; i < segmentLength; i++)
            {
                this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
                ropeStartPoint.y -= ropeSegLen;
            }
        }
        private void Update()
        {
            this.DrawRope();
        }

        private void FixedUpdate()
        {
            this.Simulate();
        }

        private void Simulate()
        {
            // SIMULATION
            Vector2 forceGravity = new Vector2(0f, -1f);

            for (int i = 1; i < this.segmentLength - 1; i++)
            {
                RopeSegment segment = this.ropeSegments[i];
                Vector2 velocity = segment.posNow - segment.posOld;
                segment.posOld = segment.posNow;
                segment.posNow += velocity;
                segment.posNow += forceGravity * Time.fixedDeltaTime;
                this.ropeSegments[i] = segment;
            }

            //CONSTRAINTS
            for (int i = 0; i < 50; i++)
            {
                ApplyConstraint();
            }
            
            //Constrant to First Point 
            RopeSegment firstSegment = ropeSegments[0];
            firstSegment.posNow = StartPoint.position;
            this.ropeSegments[0] = firstSegment;


            //Constrant to Second Point 
            RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
            endSegment.posNow = this.EndPoint.position;
            this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;
        }

        private void ApplyConstraint()
        {
            //Constrant to First Point 
            RopeSegment firstSegment = this.ropeSegments[0];
            firstSegment.posNow = this.StartPoint.position;
            this.ropeSegments[0] = firstSegment;


            //Constrant to Second Point 
            RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
            endSegment.posNow = this.EndPoint.position;
            this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

            for (int i = 0; i < this.segmentLength - 1; i++)
            {
                RopeSegment firstSeg = this.ropeSegments[i];
                RopeSegment secondSeg = this.ropeSegments[i + 1];

                float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
                float error = Mathf.Abs(dist - this.ropeSegLen);
                Vector2 changeDir = Vector2.zero;

                if (dist > ropeSegLen)
                {
                    changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
                }
                else if (dist < ropeSegLen)
                {
                    changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
                }

                Vector2 changeAmount = changeDir * error;
                if (i != 0)
                {
                    firstSeg.posNow -= changeAmount * 0.5f;
                    this.ropeSegments[i] = firstSeg;
                    secondSeg.posNow += changeAmount * 0.5f;
                    this.ropeSegments[i + 1] = secondSeg;
                }
                else
                {
                    secondSeg.posNow += changeAmount;
                    this.ropeSegments[i + 1] = secondSeg;
                }
            }
        }

        private void DrawRope()
        {
            float lineWidth = this.lineWidth;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;

            Vector3[] ropePositions = new Vector3[this.segmentLength];
            for (int i = 0; i < this.segmentLength; i++)
            {
                ropePositions[i] = this.ropeSegments[i].posNow;
            }

            lineRenderer.positionCount = ropePositions.Length;
            lineRenderer.SetPositions(ropePositions);
        }

        private struct RopeSegment
        {
            public Vector2 posNow;
            public Vector2 posOld;

            public RopeSegment(Vector2 pos)
            {
                this.posNow = pos;
                this.posOld = pos;
            }
        }
    }
}