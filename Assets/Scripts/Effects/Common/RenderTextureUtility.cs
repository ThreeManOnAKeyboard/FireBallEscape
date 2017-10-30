using System.Collections.Generic;
using UnityEngine;

namespace Effects.Common
{
    public class RenderTextureUtility
    {
        //Temporary render texture handling
        private List<RenderTexture> mTemporaryRTs = new List<RenderTexture>();

        public RenderTexture GetTemporaryRenderTexture(int width, int height, int depthBuffer = 0, RenderTextureFormat format = RenderTextureFormat.ARGBHalf, FilterMode filterMode = FilterMode.Bilinear)
        {
            var rt = RenderTexture.GetTemporary(width, height, depthBuffer, format);
            rt.filterMode = filterMode;
            rt.wrapMode = TextureWrapMode.Clamp;
            rt.name = "RenderTextureUtilityTempTexture";
            mTemporaryRTs.Add(rt);
            return rt;
        }

        public void ReleaseTemporaryRenderTexture(RenderTexture rt)
        {
            if (rt == null)
                return;

            if (!mTemporaryRTs.Contains(rt))
            {
                Debug.LogErrorFormat("Attempting to remove texture that was not allocated: {0}", rt);
                return;
            }

            mTemporaryRTs.Remove(rt);
            RenderTexture.ReleaseTemporary(rt);
        }

        public void ReleaseAllTemporaryRenderTextures()
        {
            for (int i = 0; i < mTemporaryRTs.Count; ++i)
                RenderTexture.ReleaseTemporary(mTemporaryRTs[i]);

            mTemporaryRTs.Clear();
        }
    }
}
