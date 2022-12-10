using FistVR;
using UnityEngine;


namespace AndrewFTW
{ 
    public class AirdropVehicle: MonoBehaviour
    {
        public Transform DropSpawnPt;
        public GameObject DestructableComponent;
        public GameObject DeliveryVoiceObj;

        [HideInInspector]
        public bool DestructableDestroyed = false;

#if !(UNITY_EDITOR || UNITY_5)

        public void Update()
        {
           if (DestructableComponent == null)
           {
                //Debug.Log("Destroyd");
                DestructableDestroyed = true;
           }
        }

        public void ActivateVoice()
        {
            DeliveryVoiceObj.SetActive(true);
        }

#endif
    }
}



