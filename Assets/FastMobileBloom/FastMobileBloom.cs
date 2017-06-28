using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/FastMobileBloom")]
	public class FastMobileBloom : PostEffectsBase
	{
		[Range(0.0f, 1.5f)] public float threshold = 0.25f;
		[HideInInspector] public bool normalizeThreshold = false;

		[Range(0.0f, 2.5f)] public float intensity = 1.0f;
		[Range(0.25f, 5.5f)] public float blurSize = 1.0f;
		[Range(1, 4)] public int blurIterations = 2;

		public Shader fastBloomShader = null;
		private Material fastBloomMaterial = null;

		public override bool CheckResources()
		{
			CheckSupport(false);

			fastBloomMaterial = CheckShaderAndCreateMaterial(fastBloomShader, fastBloomMaterial);

			if(!isSupported)
				ReportAutoDisable();
			return isSupported;
		}

		void OnDisable()
		{
			if(fastBloomMaterial)
				DestroyImmediate(fastBloomMaterial);
		}

		void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if(CheckResources() == false)
			{
				Graphics.Blit(source, destination);
				return;
			}

			int rtW = source.width / 4;
			int rtH = source.height / 4;

			
			//initial downsample
			RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format); rt.filterMode = FilterMode.Bilinear;

			if(normalizeThreshold == true)
				fastBloomMaterial.SetVector("_ThresholdParams", new Vector2(1.0f / (1.0f - threshold), -threshold * (1.0f / (1.0f - threshold))));
			else
				fastBloomMaterial.SetVector("_ThresholdParams", new Vector2(1.0f, -threshold));

			fastBloomMaterial.SetFloat("_Spread", blurSize);
			fastBloomMaterial.SetFloat("_BloomIntensity", intensity);
			Graphics.Blit(source, rt, fastBloomMaterial, 0);

			
			//downscale
			for(int i = 0; i < blurIterations-1; i++)
			{
				RenderTexture rt2 = RenderTexture.GetTemporary(rt.width / 2, rt.height / 2, 0, source.format); rt2.filterMode = FilterMode.Bilinear;

				fastBloomMaterial.SetFloat("_Spread", blurSize);
				Graphics.Blit(rt, rt2, fastBloomMaterial, 1);
				RenderTexture.ReleaseTemporary(rt);
				rt = rt2;
			}
			//upscale
			for(int i = 0; i < blurIterations-1; i++)
			{
				RenderTexture rt2 = RenderTexture.GetTemporary(rt.width * 2, rt.height * 2, 0, source.format); rt2.filterMode = FilterMode.Bilinear;

				fastBloomMaterial.SetFloat("_Spread", blurSize);
				Graphics.Blit(rt, rt2, fastBloomMaterial, 1);
				RenderTexture.ReleaseTemporary(rt);
				rt = rt2;
			}

			fastBloomMaterial.SetTexture("_BloomTex", rt);
			Graphics.Blit(source, destination, fastBloomMaterial, 3);

			RenderTexture.ReleaseTemporary(rt);
		}
	}
}