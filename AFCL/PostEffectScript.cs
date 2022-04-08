using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndrewFTW
{
    [ExecuteInEditMode]
    public class PostEffectScript : MonoBehaviour
    {

        public Material mat;


        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, mat);
        }
    }
}
