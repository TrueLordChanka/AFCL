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
        public GameObject[] DropPrefabs;
        public float DropSpeedInheritPercent = 0.05f;
        //public float MaxInaccuracy = 0.5f;

        [Header("Misc Settings")]
        public bool DoesRequireSkySight = false;
        public bool IsDestructable = false;
        

        private Transform _vehicleInstPt;
        private Transform _centerPt;
        private GameObject _vehicleInstance;
        private bool _droppedPayload = false;
        private float _sleighDistanceTraveled = 0f;
        private AirdropVehicle _airDropVehicle;
        

#if !(UNITY_EDITOR || UNITY_5)


        public void Start()
        {
            //I need to get the point to spawn the vehicle at. This point will be the altitude up, and the radius away.
            //For now, we will be just moving (+Altitude, +Radius) relative to the marker point
            

            //pick some random rotation in order to spin the marker in order to then spawn the insert pt at a random rotation
            float _randomRot = UnityEngine.Random.Range(0, 360);
            transform.localEulerAngles = new Vector3(0, _randomRot, 0);
            
            _vehicleInstPt = new GameObject("VehicleInstPt").transform;
            _vehicleInstPt.localPosition = new Vector3( SpawnRadius, SpawnAltitude , 0 );

            //Create a center point of which to aim our vehicle at
            _centerPt = new GameObject("CenterPoint").transform;
            _centerPt.localPosition = new Vector3(transform.localPosition.x, SpawnAltitude, transform.localPosition.z);

            //set the parents of the two points to be the marker for cleanup later
            _vehicleInstPt.SetParent(transform, false);
            _centerPt.SetParent(transform,true);

            // We set the position to be the x pos of the marker, the y+alt of the makrer, and then z-radius cause positive z goes to the right and I want it to go left to right

            _vehicleInstance = Instantiate(DropVehicle, _vehicleInstPt); //Spawn the vehicle at the inst point
            _vehicleInstance.transform.localPosition = new Vector3(0, 0, 0); //Ensure the vehicle spawns at the correct spot
            _vehicleInstance.transform.SetParent(null, true); //This will set the vehicle free from the point, the vehicle is now on its own in the world
            Debug.Log( Vector3.Distance(_vehicleInstance.transform.position, _centerPt.transform.position));
            Debug.Log(Vector3.Distance(_vehicleInstPt.transform.position, _centerPt.transform.position));
            //_inaccuracy = UnityEngine.Random.Range(-MaxInaccuracy, MaxInaccuracy);

            if (DoesRequireSkySight) //only do this if we require lign of sight
            {
                if (Physics.Raycast(transform.position, _centerPt.position))
                {
                    //Theres somthing above, abort
                    //Debug.Log("No Line of sight");
                    EndDrop();
                }
                else
                {
                    //Debug.Log("Line of sight");
                }
            }
            _airDropVehicle = _vehicleInstance.GetComponent<AirdropVehicle>();
            
            
        }

        

        public void FixedUpdate()
        {
            //move the thing at speed
            _vehicleInstance.GetComponent<Rigidbody>().velocity = _vehicleInstance.transform.forward * TravelSpeed;
            _sleighDistanceTraveled = Vector3.Distance(_vehicleInstance.transform.position, _vehicleInstPt.transform.position);

            //when it reaches the center spawn the box (or the value of the inaccuracy) Skipped for now
            // TODO Fix inaccuracy. I want to have the negative values work... I guess I could have both the distance from center and spawn, and when the 
            // inaccuracy is negative, use that number...
            //
            // Lets make it spawn first...
            // 
            

            if (_sleighDistanceTraveled >= SpawnRadius - ZeroingOffset && !_droppedPayload)
            {   // If its close spawn the payload, at 95% altitude
                //if youre here youre close enought to spawn it
                //Debug.Log("Dropped");
                Transform _dropTrans = gameObject.transform;
                foreach (GameObject dropitem in DropPrefabs)
                {
                    GameObject _dropInstance = Instantiate(dropitem, _dropTrans);
                    _dropInstance.transform.position = new Vector3(_vehicleInstance.transform.position.x, _vehicleInstance.transform.position.y - SpawnAltitude * 0.02f, _vehicleInstance.transform.position.z - 3);
                    _dropInstance.transform.SetParent(null, true);
                    _dropInstance.GetComponent<Rigidbody>().velocity = _vehicleInstance.GetComponent<Rigidbody>().velocity * DropSpeedInheritPercent;

                }
                _droppedPayload = true;
                _airDropVehicle.ActivateVoice();
            }


            if((Vector3.Distance(_vehicleInstance.transform.position, _centerPt.transform.position) > SpawnRadius * 2f))
            {
                //Debug.Log("End" + Vector3.Distance(_vehicleInstance.transform.position, _centerPt.transform.position));
                EndDrop();
            }


            if (IsDestructable)
            {
                //Debug.Log(_airDropVehicle.DestructableDestroyed);
                //Debug.Log(_airDropVehicle.name);
                if (_airDropVehicle.DestructableDestroyed == true)
                {
                    //Debug.Log("End");
                    EndDrop(); //End the drop, even if you didnt get your presents. Bah Humbug.
                }
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



