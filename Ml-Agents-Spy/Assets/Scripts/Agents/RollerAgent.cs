using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Agents
{
    public class RollerAgent : Agent
    {
        private Rigidbody rb;
        public float speed;
        public Transform Target;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        /*
     * This will be called to set up the environment at the start of each
     * 'episode', in which the Agent either fails or succeeds in its task
     */
        public override void OnEpisodeBegin()
        {
            if (this.transform.localPosition.y < 0)
            {
                // zero momentum
                this.rb.angularVelocity = Vector3.zero;
                this.rb.velocity = Vector3.zero;
                // move back to centre
                this.transform.localPosition = new Vector3(0, 0.5f, 0);
            }
        
            // Move target to new spot 
            Target.localPosition = new Vector3( 
                Random.value * 8-4,
                0.5f,
                Random.value * 8 - 4
            );
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // positional awareness of itself and the target 
            sensor.AddObservation(Target.localPosition);
            sensor.AddObservation(this.transform.localPosition);

            // Agent velocity
            sensor.AddObservation(rb.velocity.x);
            sensor.AddObservation(rb.velocity.y);
        }

        // receives actions and assigns the reward
        public override void OnActionReceived(float[] vectorAction)
        {
            // we will give two actions to the agent x + z axis movement
            // the action vector is of size 2
            Vector3 controlSignal = Vector3.zero;
            controlSignal.x = vectorAction[0];
            controlSignal.z = vectorAction[1];
            rb.AddForce(controlSignal * speed);


            SetReward(-0.1f);
            //rewards
            float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

            // when reaching target
            if (distanceToTarget < 1.42f)
            {
                // sets the reward at 1
                SetReward((1.0f));
                //Ends the episode 
                EndEpisode();
            }

            // if the agent falls
            if (this.transform.localPosition.y < 0)
            {
                EndEpisode();
            }
        }

        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = Input.GetAxis("Horizontal");
            actionsOut[1] = Input.GetAxis("Vertical");
        }
    }
}
