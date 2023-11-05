using FistVR;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace AndrewFTW
{ 
    public class Airdrop_Vehicle : MonoBehaviour
    {
        public float SpawnRadius = 2000f;
        public float SpawnAltitude = 500f;
        public float VehicleSpeed = 10f;
        public Transform DropSpawnPt;

        public GameObject DeliveryVoiceObj;
        [HideInInspector]
        public Transform CallTransform;
        [HideInInspector]
        public Airdrop_Controller Controller;



        private float DistanceToDropPt;
        private Transform DropPt;
        private bool HasDropped = false;

#if !(UNITY_EDITOR || UNITY_5)

        public void Init()
        {
            Debug.Log("Bravo");
            //pick a random pt to spawn the craft
            // Generate a random angle between 0 and 2*pi (360 degrees)
            float randomAngle = Random.Range(0f, Mathf.PI*2f);
            // Calculate the x and y coordinates of the point on the circle's edge
            float x = SpawnRadius * Mathf.Cos(randomAngle);
            float z = SpawnRadius * Mathf.Sin(randomAngle);
            transform.position = new Vector3(CallTransform.position.x+x, SpawnAltitude, CallTransform.position.y+z);
            Debug.Log("Charlie");
            //reorient the craft towards the call point
            transform.LookAt(CallTransform.position);
            float y = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(0, y, 0); //reset rotation just keepying the y val
            Debug.Log("Delta");
            DropPt = new GameObject("DropPt").transform; //make the drop pt the same point but up in the sky
            DropPt.transform.position = new Vector3(CallTransform.position.x, SpawnAltitude, CallTransform.position.z);
            Debug.Log(DropPt.transform.position);
            Debug.Log(CallTransform.position);

        }


        public void FixedUpdate()
        {
            
            this.GetComponent<Rigidbody>().velocity = transform.forward * VehicleSpeed;
            DistanceToDropPt = Vector3.Distance(transform.position, DropPt.position);
            Debug.Log(DistanceToDropPt);
            if (DistanceToDropPt < 1 && !HasDropped)
            {
                Debug.Log("Drop");
                Controller.DropPayload(DropSpawnPt);
                HasDropped = true;
            }
            
            if(DistanceToDropPt > SpawnRadius*1.3)
            {
                gameObject.SetActive(false);
            }
            

        }

        public void ActivateVoice()
        {
            DeliveryVoiceObj.SetActive(true);
        }

        public void CheckCollision()
        {
            
        }

        public void CalculateDropPt()
        {

        }

#endif
    }
}



