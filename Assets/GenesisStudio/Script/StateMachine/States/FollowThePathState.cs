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
        Queue<Transform> points;
        Coroutine pathCoroutine;
        Transform _destination;
        private float _waitTimer;
        private int _waypointIndex;
        private Path path;

        public FollowThePathState(Path path)
        {
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
        }

        public override void Perform()
        {
            if (_destination == null && path.points.Count > 0)
            {
                _destination = path.points[currentIndex];
                _brain.Move(_destination.position);
            }

            if (_destination != null && _brain.Agent.remainingDistance < _brain.Agent.stoppingDistance)
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
