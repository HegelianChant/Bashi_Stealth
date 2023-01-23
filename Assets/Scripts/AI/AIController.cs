using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Beinuci
{
    public class AIController : MonoBehaviour
    {
        NavMeshAgent agent;
        new Rigidbody rigidbody;
        Animator animator;

		public int index;
		public Waypoint[] waypoints;
		Waypoint currentWaypoint;
		Transform mTransform;

		public bool isAggressive;

		float waitTimer;

		public float rotateSpeed = .5f;

		private void Start()
		{
			agent = GetComponent<NavMeshAgent>();
			rigidbody = GetComponent<Rigidbody>();
			animator = GetComponentInChildren<Animator>();
			currentWaypoint = waypoints[index];
			mTransform = this.transform;
		}

		private void Update()
		{
			float delta = Time.deltaTime;

			if (!isAggressive)
			{
				HandleNormalLogic(delta);
			}
			else
			{

			}
		}
	
		public void HandleNormalLogic(float delta)
		{
			currentWaypoint = waypoints[index];

			float dis = Vector3.Distance(mTransform.position, currentWaypoint.targetPosition.position);
			if (dis > agent.stoppingDistance)
			{
				animator.SetFloat("movement", 1, 0.2f, delta);
				agent.updateRotation = true;

				if (agent.hasPath == false)
					agent.SetDestination(currentWaypoint.targetPosition.position);
			}
			else
			{
				animator.SetFloat("movement", 0, 0.2f, delta);

				agent.updateRotation = false;
				Quaternion targetRot = Quaternion.Euler(currentWaypoint.lookEulers);
				mTransform.rotation = Quaternion.Slerp(mTransform.rotation, targetRot, delta / rotateSpeed);

				if (waitTimer < currentWaypoint.waitTime)
				{
					waitTimer += delta;
				}
				else
				{
					waitTimer = 0;
					index++;
					if (index > waypoints.Length - 1)
					{
						index = 0;
					}
				}

				
			}
		}
	}
		

	[System.Serializable]
	public class Waypoint
	{
		public Transform targetPosition;
		public Vector3 lookEulers;
		public float waitTime;

	}
}
