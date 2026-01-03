using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GenesisStudio
{
    public class FollowThePathState : State
    {
        Transform _destination;
        public Transform _startPoint;
        private float _waitTimer;
        private int _waypointIndex;
        private Path path;
        
        
        public FollowThePathState(Path path, Transform startPoint = null)
        {
            _startPoint = startPoint;
            
            if (path == null || path.points.Count == 0)
            {
                Debug.LogWarning("Path is null or has no points.");
                return;
            }
            this.path = path;
        }

        private int currentIndex = 0;

        public override void Enter()
        {
            currentIndex = 0;
            if(_startPoint != null)
            {
                _destination = _startPoint;
                // _enemy.MoveInstantly(_startPoint.position);
            }
        }

        public override void Perform()
        {
            if (_destination == null && path.points.Count > 0)
            {
                _destination = path.points[currentIndex];
                _brain.MoveInstantly(_destination.position);
            }

            
            if (_destination != null && _brain.transform.IsNearThePoint(_destination, 2f))
            {
                _waitTimer += Time.deltaTime;
                if (_waitTimer > .5)
                {
                    if(path.loop)
                    {
                        currentIndex = (currentIndex + 1) % path.points.Count;
                    }
                    else
                    {
                        currentIndex++;
                    }
                    if (currentIndex >= path.points.Count)
                    {
                        _brain.stateMachine.ChangeState(new IdleState());
                        path.onLastPointArrivedForNPC?.Invoke();
                    }
                    _destination = null;
                    _waitTimer = 0;
                }
            }
        }


        public override void Exit()
        {
            _brain.SetPath(null);
        }

    }
}
