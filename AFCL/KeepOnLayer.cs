using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class KeepOnLayer : MonoBehaviour
    {
        public FVRPhysicalObject Object;
        public string LayerName;
        public Collider[] Colliders;

        private static Dictionary<FVRPhysicalObject, KeepOnLayer> _keepOnLayers = new Dictionary<FVRPhysicalObject, KeepOnLayer>();

#if !(UNITY_EDITOR || UNITY_5 )

        static KeepOnLayer()
        {
            On.FistVR.FVRPhysicalObject.EndInteraction += FVRPhysicalObject_EndInteraction;
        }

        private static void FVRPhysicalObject_EndInteraction(On.FistVR.FVRPhysicalObject.orig_EndInteraction orig, FVRPhysicalObject self, FVRViveHand hand)
        {
            orig(self, hand);
            KeepOnLayer _keepOnLayer;
            if(_keepOnLayers.TryGetValue(self, out _keepOnLayer))
            {
                _keepOnLayer.UpdateColliderLayers();
            }

        }

        public void UpdateColliderLayers()
        {
            foreach(Collider collider in Colliders)
            {
                collider.gameObject.layer = LayerMask.NameToLayer(LayerName);
            }
        }


        public void Awake()
        {
            _keepOnLayers.Add(Object, this);
        }

        public void OnDestroy()
        {
            _keepOnLayers.Remove(Object);
        }

#endif
    }
}



