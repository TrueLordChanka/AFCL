using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class TransferRBtoParentOnAttach : MonoBehaviour
    {
        public FVRFireArmAttachment Attachment;
        public Joint Joint;

        public GameObject ProxyWaggleBit;

        private FVRPhysicalObject _attachedObj;
        private GameObject _waggleTransform;

#if !(UNITY_EDITOR || UNITY_5)

        public void Awake()
        {
            _waggleTransform = new GameObject();
            _waggleTransform.transform.localPosition = Joint.transform.localPosition;
            _waggleTransform.transform.localRotation = Joint.transform.localRotation;
        }


        public void OnEnable() //This script is made to go on the interface so itll turn on
        {
            //get the fvr object its attached to
            _attachedObj = Attachment.GetRootObject();
            //now that we have the root object, we must move our rigidbody to it. 
            Joint.connectedBody = _attachedObj.GetComponent<Rigidbody>();
            
            //Disable the proxy waggle bit
            ProxyWaggleBit.SetActive(false);
            Joint.gameObject.SetActive(true);
        }

        public void OnDisable()
        {
            //on disable, we wanna put the connected body back to the attachment.
            Joint.connectedBody = Attachment.GetComponent<Rigidbody>();
            Joint.transform.localPosition = _waggleTransform.transform.localPosition;
            Joint.transform.localRotation = _waggleTransform.transform.localRotation;

            //enable the proxy
            ProxyWaggleBit.SetActive(true);
            Joint.gameObject.SetActive(false);
        }



#endif
    }
}



