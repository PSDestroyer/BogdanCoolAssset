using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenesisStudio;
using UnityEngine;
using UnityEngine.Events;

namespace GenesisStudio
{
    public class Path : MonoBehaviour
    {
        public UnityEvent onLastPointArrivedForPlayer;
        public UnityEvent onLastPointArrivedForNPC;

        public Color gizmosColor = Color.green;
        public float gizmoSize = 0.2f;
        public bool loop = false;

        public List<Transform> points
        {
            get { return transform.Cast<Transform>().ToList(); }
        }

        private void OnDrawGizmos()
        {
            var pts = points;
            if (pts.Count == 0) return;

            for (int i = 0; i < pts.Count; i++)
            {
                if (pts[i] == null) continue;

                Gizmos.color = gizmosColor;
                Gizmos.DrawSphere(pts[i].position, gizmoSize);

                // Draw line to next point
                if (i < pts.Count - 1)
                {
                    Gizmos.DrawLine(pts[i].position, pts[i + 1].position);
                }
            }

            // Close the loop if enabled
            if (loop && pts.Count > 1)
            {
                Gizmos.DrawLine(pts[pts.Count - 1].position, pts[0].position);
            }
        }

        
    }
}