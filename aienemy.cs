using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class aienemy : MonoBehaviour
    {

        public NavMeshAgent agent;
        public ThirdPersonCharacter character;

        public enum State
        {
            Patrol,
            Chase
        }

        public State state;
        private bool Alive;

        public GameObject[] waypoints;
        private int waypointsind;
        public float patrolspeed = 0.5f;

        public float chasespeed = 1f;
        public GameObject target;

        // Use this for initialization
        void Start()
        {

            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updatePosition = true;
            agent.updateRotation = false;

            waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            waypointsind = Random.Range(0, waypoints.Length);

            state = State.Patrol;
            Alive = true;

            StartCoroutine("FSM");
        }

        IEnumerator FSM()
        {
            while (Alive)
            {
                switch (state)
                {
                    case State.Patrol:
                        Patrol();
                        break;

                    case State.Chase:
                        Chase();
                        break;
                }
                yield return null;
            }
        }

        void Patrol()
        {
            agent.speed = patrolspeed;
            if (Vector3.Distance(this.transform.position, waypoints[waypointsind].transform.position) >= 2)
            {
                agent.SetDestination(waypoints[waypointsind].transform.position);
                character.Move(agent.desiredVelocity, false, false);
            }
            else if (Vector3.Distance(this.transform.position, waypoints[waypointsind].transform.position) <= 2)
            {
                waypointsind = Random.Range(0, waypoints.Length);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
            
        }
        void Chase()
        {
            agent.speed = chasespeed;
            agent.SetDestination(target.transform.position);
            character.Move(agent.desiredVelocity, false, false);
        }

        // Update is called once per frame

        void OnTriggerEnter(Collider coll)
        {
            if(coll.gameObject.tag == "Player")
            {
                state = State.Chase;
                target = coll.gameObject;
            }
        }
        //void OnCollisionEnter(Collision coll)
        //{
        //    if(coll.gameObject.tag == "Player")
        //    {
        //        hitpoints.heath -= 3;
        //    }
        //}
    }
}