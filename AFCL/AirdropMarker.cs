using FistVR;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace AndrewFTW
{ 
    public class AirdropMarker : MonoBehaviour
    {
        [Header("Drop Vehicle")]
        public GameObject DropVehicle;
        public float WaitToSpawn = 0;
        public float SpawnAltitude = 100;
        public float SpawnRadius = 1000;
        public float TravelSpeed = 100;
        public float ZeroingOffset = 0;

        [Header("Drop Prefab")]
        public GameObject DropPrefab;
        public float DropSpeedInheritPercent = 0.05f;
        //public float MaxInaccuracy = 0.5f;



        private Transform _vehicleInstPt;
        private Transform _centerPt;
        private GameObject _vehicleInstance;
        private bool _droppedPayload = false;
        //private float _inaccuracy;

#if !(UNITY_EDITOR || UNITY_5)


        public void Start()
        {
            //I need to get the point to spawn the vehicle at. This point will be the altitude up, and the radius away.
            //For now, we will be just moving (+Altitude, +Radius) relative to the marker point
            

            //pick some random rotation in order to spin the marker in order to then spawn the insert pt at a random rotation
            float _randomRot = UnityEngine.Random.Range(0, 360);
            transform.localEulerAngles = new Vector3(0, _randomRot, 0);
            
            _vehicleInstPt = new GameObject("VehicleInstPt").transform;
            _vehicleInstPt.position = new Vector3( transform.localPosition.x, transform.localPosition.y + SpawnAltitude , transform.localPosition.z - SpawnRadius );

            //Create a center point of which to aim our vehicle at
            _centerPt = new GameObject("CenterPoint").transform;
            _centerPt.position = new Vector3(transform.position.x, transform.position.y + SpawnAltitude, transform.position.z);

            //set the parents of the two points to be the marker for cleanup later
            _vehicleInstPt.SetParent(transform, true);
            _centerPt.SetParent(transform,true);

            // We set the position to be the x pos of the marker, the y+alt of the makrer, and then z-radius cause positive z goes to the right and I want it to go left to right

            _vehicleInstance = Instantiate(DropVehicle, _vehicleInstPt); //Spawn the vehicle at the inst point
            _vehicleInstance.transform.localPosition = new Vector3(0, 0, 0); //Ensure the vehicle spawns at the correct spot
            _vehicleInstance.transform.SetParent(null, true); //This will set the vehicle free from the point, the vehicle is now on its own in the world

            //_inaccuracy = UnityEngine.Random.Range(-MaxInaccuracy, MaxInaccuracy);

            if(Physics.Raycast(transform.position, _centerPt.position))
            {
                //Theres somthing above, abort
                //Debug.Log("No Line of sight");
                EndDrop();
            } else
            {
                //Debug.Log("Line of sight");
            }
           
        }

        

        public void FixedUpdate()
        {
            //move the thing at speed
            _vehicleInstance.GetComponent<Rigidbody>().velocity = _vehicleInstance.transform.forward * TravelSpeed;

            //when it reaches the center spawn the box (or the value of the inaccuracy) Skipped for now
            // TODO Fix inaccuracy. I want to have the negative values work... I guess I could have both the distance from center and spawn, and when the 
            // inaccuracy is negative, use that number...
            //
            // Lets make it spawn first...
            // 

            if(Vector3.Distance(_vehicleInstance.transform.position, _centerPt.transform.position) <= TravelSpeed*0.04 + ZeroingOffset && !_droppedPayload)
            {   // If its close spawn the payload, at 95% altitude
                //if youre here youre close enought to spawn it
                Debug.Log("Dropped");
                Transform _dropPt = _vehicleInstance.transform;
                //_dropPt.position = new Vector3(_dropPt.position.x, _dropPt.position.y - SpawnAltitude * 0.05f , _dropPt.position.z);
                GameObject _dropInstance = Instantiate(DropPrefab, _dropPt );
                _dropInstance.transform.position = new Vector3(_dropInstance.transform.position.x, _dropInstance.transform.position.y - SpawnAltitude * 0.02f, _dropInstance.transform.position.z);
                _dropInstance.transform.SetParent(null, true);
                _dropInstance.GetComponent<Rigidbody>().velocity = _vehicleInstance.GetComponent<Rigidbody>().velocity * DropSpeedInheritPercent;

                _droppedPayload = true;
            }

            if((Vector3.Distance(_vehicleInstance.transform.position, _centerPt.transform.position) > SpawnRadius * 2f))
            {
                //Debug.Log("End");
                EndDrop();
            }



        }


        public void EndDrop()
        {
            Destroy(_vehicleInstance);
            Destroy(gameObject);
        }

#endif
    }
}



