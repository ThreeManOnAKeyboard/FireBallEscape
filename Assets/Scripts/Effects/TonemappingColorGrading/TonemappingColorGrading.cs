using System;
using Effects.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Effects.TonemappingColorGrading
{
	[ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Cinematic/Tonemapping and Color Grading")]
#if UNITY_5_4_OR_NEWER
    [ImageEffectAllowedInSceneView]
#endif
    public class TonemappingColorGrading : MonoBehaviour
    {
#if UNITY_EDITOR
        // EDITOR ONLY call for allowing the editor to update the histogram
        public UnityAction<RenderTexture> onFrameEndEditorOnly;

        [SerializeField]
        private ComputeShader mHistogramComputeShader;
        public ComputeShader HistogramComputeShader
        {
            get
            {
                if (mHistogramComputeShader == null)
                    mHistogramComputeShader = Resources.Load<ComputeShader>("HistogramCompute");

                return mHistogramComputeShader;
            }
        }

        [SerializeField]
        private Shader mHistogramShader;
        public Shader HistogramShader
        {
            get
            {
                if (mHistogramShader == null)
                    mHistogramShader = Shader.Find("Hidden/TonemappingColorGradingHistogram");

                return mHistogramShader;
            }
        }

        [SerializeField]
        public bool histogramRefreshOnPlay = false;
#endif

        #region Attributes
        [AttributeUsage(AttributeTargets.Field)]
        public class SettingsGroup : Attribute
        {}

        public class IndentedGroup : PropertyAttribute
        {}

        public class ChannelMixer : PropertyAttribute
        {}

        public class ColorWheelGroup : PropertyAttribute
        {
            public int minSizePerWheel = 60;
            public int maxSizePerWheel = 150;

            public ColorWheelGroup()
            {}

            public ColorWheelGroup(int minSizePerWheel, int maxSizePerWheel)
            {
                this.minSizePerWheel = minSizePerWheel;
                this.maxSizePerWheel = maxSizePerWheel;
            }
        }

        public class Curve : PropertyAttribute
        {
            public Color color = Color.white;

            public Curve()
            {}

            public Curve(float r, float g, float b, float a) // Can't pass a struct in an attribute
            {
                color = new Color(r, g, b, a);
            }
        }
        #endregion

        #region Settings
        [Serializable]
        public struct EyeAdaptationSettings
        {
            public bool enabled;

            [Min(0f), Tooltip("Midpoint Adjustment.")]
            public float middleGrey;

            [Tooltip("The lowest possible exposure value; adjust this value to modify the brightest areas of your level.")]
            public float min;

            [Tooltip("The highest possible exposure value; adjust this value to modify the darkest areas of your level.")]
            public float max;

            [Min(0f), Tooltip("Speed of linear adaptation. Higher is faster.")]
            public float speed;

            [Tooltip("Displays a luminosity helper in the GameView.")]
            public bool showDebug;

            public static EyeAdaptationSettings DefaultSettings
            {
                get
                {
                    return new EyeAdaptationSettings
                    {
                        enabled = false,
                        showDebug = false,
                        middleGrey = 0.5f,
                        min = -3f,
                        max = 3f,
                        speed = 1.5f
                    };
                }
            }
        }

        public enum Tonemapper
        {
            Aces,
            Curve,
            Hable,
            HejlDawson,
            Photographic,
            Reinhard,
            Neutral
        }

        [Serializable]
        public struct TonemappingSettings
        {
            public bool enabled;

            [Tooltip("Tonemapping technique to use. ACES is the recommended one.")]
            public Tonemapper tonemapper;

            [Min(0f), Tooltip("Adjusts the overall exposure of the scene.")]
            public float exposure;

            [Tooltip("Custom tonemapping curve.")]
            public AnimationCurve curve;

            // Neutral settings
            [Range(-0.1f, 0.1f)]
            public float neutralBlackIn;

            [Range(1f, 20f)]
            public float neutralWhiteIn;

            [Range(-0.09f, 0.1f)]
            public float neutralBlackOut;

            [Range(1f, 19f)]
            public float neutralWhiteOut;

            [Range(0.1f, 20f)]
            public float neutralWhiteLevel;

            [Range(1f, 10f)]
            public float neutralWhiteClip;

            public static TonemappingSettings DefaultSettings
            {
                get
                {
                    return new TonemappingSettings
                    {
                        enabled = false,
                        tonemapper = Tonemapper.Neutral,
                        exposure = 1f,
                        curve = CurvesSettings.DefaultCurve,
                        neutralBlackIn = 0.02f,
                        neutralWhiteIn = 10f,
                        neutralBlackOut = 0f,
                        neutralWhiteOut = 10f,
                        neutralWhiteLevel = 5.3f,
                        neutralWhiteClip = 10f
                    };
                }
            }
        }

        [Serializable]
        public struct LutSettings
        {
            public bool enabled;

            [Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
            public Texture texture;

            [Range(0f, 1f), Tooltip("Blending factor.")]
            public float contribution;

            public static LutSettings DefaultSettings
            {
                get
                {
                    return new LutSettings
                    {
                        enabled = false,
                        texture = null,
                        contribution = 1f
                    };
                }
            }
        }

        [Serializable]
        public struct ColorWheelsSettings
        {
            [ColorUsage(false)]
            public Color shadows;

            [ColorUsage(false)]
            public Color midtones;

            [ColorUsage(false)]
            public Color highlights;

            public static ColorWheelsSettings DefaultSettings
            {
                get
                {
                    return new ColorWheelsSettings
                    {
                        shadows = Color.white,
                        midtones = Color.white,
                        highlights = Color.white
                    };
                }
            }
        }

        [Serializable]
        public struct BasicsSettings
        {
            [Range(-2f, 2f), Tooltip("Sets the white balance to a custom color temperature.")]
            public float temperatureShift;

            [Range(-2f, 2f), Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
            public float tint;

            [Space, Range(-0.5f, 0.5f), Tooltip("Shift the hue of all colors.")]
            public float hue;

            [Range(0f, 2f), Tooltip("Pushes the intensity of all colors.")]
            public float saturation;

            [Range(-1f, 1f), Tooltip("Adjusts the saturation so that clipping is minimized as colors approach full saturation.")]
            public float vibrance;

            [Range(0f, 10f), Tooltip("Brightens or darkens all colors.")]
            public float value;

            [Space, Range(0f, 2f), Tooltip("Expands or shrinks the overall range of tonal values.")]
            public float contrast;

            [Range(0.01f, 5f), Tooltip("Contrast gain curve. Controls the steepness of the curve.")]
            public float gain;

            [Range(0.01f, 5f), Tooltip("Applies a pow function to the source.")]
            public float gamma;

            public static BasicsSettings DefaultSettings
            {
                get
                {
                    return new BasicsSettings
                    {
                        temperatureShift = 0f,
                        tint = 0f,
                        contrast = 1f,
                        hue = 0f,
                        saturation = 1f,
                        value = 1f,
                        vibrance = 0f,
                        gain = 1f,
                        gamma = 1f
                    };
                }
            }
        }

        [Serializable]
        public struct ChannelMixerSettings
        {
            public int currentChannel;
            public Vector3[] channels;

            public static ChannelMixerSettings DefaultSettings
            {
                get
                {
                    return new ChannelMixerSettings
                    {
                        currentChannel = 0,
                        channels = new[]
                        {
                            new Vector3(1f, 0f, 0f),
                            new Vector3(0f, 1f, 0f),
                            new Vector3(0f, 0f, 1f)
                        }
                    };
                }
            }
        }

        [Serializable]
        public struct CurvesSettings
        {
            [Curve]
            public AnimationCurve master;

            [Curve(1f, 0f, 0f, 1f)]
            public AnimationCurve red;

            [Curve(0f, 1f, 0f, 1f)]
            public AnimationCurve green;

            [Curve(0f, 1f, 1f, 1f)]
            public AnimationCurve blue;

            public static CurvesSettings DefaultSettings
            {
                get
                {
                    return new CurvesSettings
                    {
                        master = DefaultCurve,
                        red = DefaultCurve,
                        green = DefaultCurve,
                        blue = DefaultCurve
                    };
                }
            }

            public static AnimationCurve DefaultCurve
            {
                get { return new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f)); }
            }
        }

        public enum ColorGradingPrecision
        {
            Normal = 16,
            High = 32
        }

        [Serializable]
        public struct ColorGradingSettings
        {
            public bool enabled;

            [Tooltip("Internal LUT precision. \"Normal\" is 256x16, \"High\" is 1024x32. Prefer \"Normal\" on mobile devices.")]
            public ColorGradingPrecision precision;

            [Space, ColorWheelGroup]
            public ColorWheelsSettings colorWheels;

            [Space, IndentedGroup]
            public BasicsSettings basics;

            [Space, ChannelMixer]
            public ChannelMixerSettings channelMixer;

            [Space, IndentedGroup]
            public CurvesSettings curves;

            [Space, Tooltip("Use dithering to try and minimize color banding in dark areas.")]
            public bool useDithering;

            [Tooltip("Displays the generated LUT in the top left corner of the GameView.")]
            public bool showDebug;

            public static ColorGradingSettings DefaultSettings
            {
                get
                {
                    return new ColorGradingSettings
                    {
                        enabled = false,
                        useDithering = false,
                        showDebug = false,
                        precision = ColorGradingPrecision.Normal,
                        colorWheels = ColorWheelsSettings.DefaultSettings,
                        basics = BasicsSettings.DefaultSettings,
                        channelMixer = ChannelMixerSettings.DefaultSettings,
                        curves = CurvesSettings.DefaultSettings
                    };
                }
            }

            internal void Reset()
            {
                curves = CurvesSettings.DefaultSettings;
            }
        }

        [SerializeField, SettingsGroup]
        private EyeAdaptationSettings mEyeAdaptation = EyeAdaptationSettings.DefaultSettings;
        public EyeAdaptationSettings EyeAdaptation
        {
            get { return mEyeAdaptation; }
            set { mEyeAdaptation = value; }
        }

        [SerializeField, SettingsGroup]
        private TonemappingSettings mTonemapping = TonemappingSettings.DefaultSettings;
        public TonemappingSettings Tonemapping
        {
            get { return mTonemapping; }
            set
            {
                mTonemapping = value;
                SetTonemapperDirty();
            }
        }

        [SerializeField, SettingsGroup]
        private ColorGradingSettings mColorGrading = ColorGradingSettings.DefaultSettings;
        public ColorGradingSettings ColorGrading
        {
            get { return mColorGrading; }
            set
            {
                mColorGrading = value;
                SetDirty();
            }
        }

        [SerializeField, SettingsGroup]
        private LutSettings mLut = LutSettings.DefaultSettings;
        public LutSettings Lut
        {
            get { return mLut; }
            set { mLut = value; }
        }
        #endregion

        private Texture2D mIdentityLut;
        private RenderTexture mInternalLut;
        private Texture2D mCurveTexture;
        private Texture2D mTonemapperCurve;
        private float mTonemapperCurveRange;

        private Texture2D IdentityLut
        {
            get
            {
                if (mIdentityLut == null || mIdentityLut.height != LutSize)
                {
                    DestroyImmediate(mIdentityLut);
                    mIdentityLut = GenerateIdentityLut(LutSize);
                }

                return mIdentityLut;
            }
        }

        private RenderTexture InternalLutRt
        {
            get
            {
                if (mInternalLut == null || !mInternalLut.IsCreated() || mInternalLut.height != LutSize)
                {
                    DestroyImmediate(mInternalLut);
                    mInternalLut = new RenderTexture(LutSize * LutSize, LutSize, 0, RenderTextureFormat.ARGB32)
                    {
                        name = "Internal LUT",
                        filterMode = FilterMode.Bilinear,
                        anisoLevel = 0,
                        hideFlags = HideFlags.DontSave
                    };
                }

                return mInternalLut;
            }
        }

        private Texture2D CurveTexture
        {
            get
            {
                if (mCurveTexture == null)
                {
                    mCurveTexture = new Texture2D(256, 1, TextureFormat.ARGB32, false, true)
                    {
                        name = "Curve texture",
                        wrapMode = TextureWrapMode.Clamp,
                        filterMode = FilterMode.Bilinear,
                        anisoLevel = 0,
                        hideFlags = HideFlags.DontSave
                    };
                }

                return mCurveTexture;
            }
        }

        private Texture2D TonemapperCurve
        {
            get
            {
                if (mTonemapperCurve == null)
                {
                    TextureFormat format = TextureFormat.RGB24;
                    if (SystemInfo.SupportsTextureFormat(TextureFormat.RFloat))
                        format = TextureFormat.RFloat;
                    else if (SystemInfo.SupportsTextureFormat(TextureFormat.RHalf))
                        format = TextureFormat.RHalf;

                    mTonemapperCurve = new Texture2D(256, 1, format, false, true)
                    {
                        name = "Tonemapper curve texture",
                        wrapMode = TextureWrapMode.Clamp,
                        filterMode = FilterMode.Bilinear,
                        anisoLevel = 0,
                        hideFlags = HideFlags.DontSave
                    };
                }

                return mTonemapperCurve;
            }
        }

        [SerializeField]
        private Shader mShader;
        public Shader Shader
        {
            get
            {
                if (mShader == null)
                    mShader = Shader.Find("Hidden/TonemappingColorGrading");

                return mShader;
            }
        }

        private Material mMaterial;
        public Material Material
        {
            get
            {
                if (mMaterial == null)
                    mMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(Shader);

                return mMaterial;
            }
        }

        public bool IsGammaColorSpace
        {
            get { return QualitySettings.activeColorSpace == ColorSpace.Gamma; }
        }

        public int LutSize
        {
            get { return (int)ColorGrading.precision; }
        }

        private enum Pass
        {
            LutGen,
            AdaptationLog,
            AdaptationExpBlend,
            AdaptationExp,
            TonemappingOff,
            TonemappingAces,
            TonemappingCurve,
            TonemappingHable,
            TonemappingHejlDawson,
            TonemappingPhotographic,
            TonemappingReinhard,
            TonemappingNeutral,
            AdaptationDebug
        }

        public bool ValidRenderTextureFormat { get; private set; }
        public bool ValidUserLutSize { get; private set; }

        private bool mDirty = true;
        private bool mTonemapperDirty = true;

        private RenderTexture mSmallAdaptiveRt;
        private RenderTextureFormat mAdaptiveRtFormat;

        private int mAdaptationSpeed;
        private int mMiddleGrey;
        private int mAdaptationMin;
        private int mAdaptationMax;
        private int mLumTex;
        private int mToneCurveRange;
        private int mToneCurve;
        private int mExposure;
        private int mNeutralTonemapperParams1;
        private int mNeutralTonemapperParams2;
        private int mWhiteBalance;
        private int mLift;
        private int mGamma;
        private int mGain;
        private int mContrastGainGamma;
        private int mVibrance;
        private int mHsv;
        private int mChannelMixerRed;
        private int mChannelMixerGreen;
        private int mChannelMixerBlue;
        private int mCurveTex;
        private int mInternalLutTex;
        private int mInternalLutParams;
        private int mUserLutTex;
        private int mUserLutParams;

        public void SetDirty()
        {
            mDirty = true;
        }

        public void SetTonemapperDirty()
        {
            mTonemapperDirty = true;
        }

        private void Awake()
        {
            mAdaptationSpeed = Shader.PropertyToID("_AdaptationSpeed");
            mMiddleGrey = Shader.PropertyToID("_MiddleGrey");
            mAdaptationMin = Shader.PropertyToID("_AdaptationMin");
            mAdaptationMax = Shader.PropertyToID("_AdaptationMax");
            mLumTex = Shader.PropertyToID("_LumTex");
            mToneCurveRange = Shader.PropertyToID("_ToneCurveRange");
            mToneCurve = Shader.PropertyToID("_ToneCurve");
            mExposure = Shader.PropertyToID("_Exposure");
            mNeutralTonemapperParams1 = Shader.PropertyToID("_NeutralTonemapperParams1");
            mNeutralTonemapperParams2 = Shader.PropertyToID("_NeutralTonemapperParams2");
            mWhiteBalance = Shader.PropertyToID("_WhiteBalance");
            mLift = Shader.PropertyToID("_Lift");
            mGamma = Shader.PropertyToID("_Gamma");
            mGain = Shader.PropertyToID("_Gain");
            mContrastGainGamma = Shader.PropertyToID("_ContrastGainGamma");
            mVibrance = Shader.PropertyToID("_Vibrance");
            mHsv = Shader.PropertyToID("_HSV");
            mChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");
            mChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");
            mChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");
            mCurveTex = Shader.PropertyToID("_CurveTex");
            mInternalLutTex = Shader.PropertyToID("_InternalLutTex");
            mInternalLutParams = Shader.PropertyToID("_InternalLutParams");
            mUserLutTex = Shader.PropertyToID("_UserLutTex");
            mUserLutParams = Shader.PropertyToID("_UserLutParams");
        }

        private void OnEnable()
        {
            if (!ImageEffectHelper.IsSupported(Shader, false, true, this))
            {
                enabled = false;
                return;
            }

            SetDirty();
            SetTonemapperDirty();
        }

        private void OnDisable()
        {
            if (mMaterial != null)
                DestroyImmediate(mMaterial);

            if (mIdentityLut != null)
                DestroyImmediate(mIdentityLut);

            if (mInternalLut != null)
                DestroyImmediate(InternalLutRt);

            if (mSmallAdaptiveRt != null)
                DestroyImmediate(mSmallAdaptiveRt);

            if (mCurveTexture != null)
                DestroyImmediate(mCurveTexture);

            if (mTonemapperCurve != null)
                DestroyImmediate(mTonemapperCurve);

            mMaterial = null;
            mIdentityLut = null;
            mInternalLut = null;
            mSmallAdaptiveRt = null;
            mCurveTexture = null;
            mTonemapperCurve = null;
        }

        private void OnValidate()
        {
            SetDirty();
            SetTonemapperDirty();
        }

        private static Texture2D GenerateIdentityLut(int dim)
        {
            Color[] newC = new Color[dim * dim * dim];
            float oneOverDim = 1f / ((float)dim - 1f);

            for (int i = 0; i < dim; i++)
                for (int j = 0; j < dim; j++)
                    for (int k = 0; k < dim; k++)
                        newC[i + (j * dim) + (k * dim * dim)] = new Color((float)i * oneOverDim, Mathf.Abs((float)k * oneOverDim), (float)j * oneOverDim, 1f);

            Texture2D tex2D = new Texture2D(dim * dim, dim, TextureFormat.RGB24, false, true)
            {
                name = "Identity LUT",
                filterMode = FilterMode.Bilinear,
                anisoLevel = 0,
                hideFlags = HideFlags.DontSave
            };
            tex2D.SetPixels(newC);
            tex2D.Apply();

            return tex2D;
        }

        // An analytical model of chromaticity of the standard illuminant, by Judd et al.
        // http://en.wikipedia.org/wiki/Standard_illuminant#Illuminant_series_D
        // Slightly modifed to adjust it with the D65 white point (x=0.31271, y=0.32902).
        private float StandardIlluminantY(float x)
        {
            return 2.87f * x - 3f * x * x - 0.27509507f;
        }

        // CIE xy chromaticity to CAT02 LMS.
        // http://en.wikipedia.org/wiki/LMS_color_space#CAT02
        private Vector3 CiExyToLms(float x, float y)
        {
            float Y = 1f;
            float X = Y * x / y;
            float z = Y * (1f - x - y) / y;

            float l =  0.7328f * X + 0.4296f * Y - 0.1624f * z;
            float m = -0.7036f * X + 1.6975f * Y + 0.0061f * z;
            float s =  0.0030f * X + 0.0136f * Y + 0.9834f * z;

            return new Vector3(l, m, s);
        }

        private Vector3 GetWhiteBalance()
        {
            float t1 = ColorGrading.basics.temperatureShift;
            float t2 = ColorGrading.basics.tint;

            // Get the CIE xy chromaticity of the reference white point.
            // Note: 0.31271 = x value on the D65 white point
            float x = 0.31271f - t1 * (t1 < 0f ? 0.1f : 0.05f);
            float y = StandardIlluminantY(x) + t2 * 0.05f;

            // Calculate the coefficients in the LMS space.
            Vector3 w1 = new Vector3(0.949237f, 1.03542f, 1.08728f); // D65 white point
            Vector3 w2 = CiExyToLms(x, y);
            return new Vector3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
        }

        private static Color NormalizeColor(Color c)
        {
            float sum = (c.r + c.g + c.b) / 3f;

            if (Mathf.Approximately(sum, 0f))
                return new Color(1f, 1f, 1f, 1f);

            return new Color
            {
                r = c.r / sum,
                g = c.g / sum,
                b = c.b / sum,
                a = 1f
            };
        }

        private void GenerateLiftGammaGain(out Color lift, out Color gamma, out Color gain)
        {
            Color nLift = NormalizeColor(ColorGrading.colorWheels.shadows);
            Color nGamma = NormalizeColor(ColorGrading.colorWheels.midtones);
            Color nGain = NormalizeColor(ColorGrading.colorWheels.highlights);

            float avgLift = (nLift.r + nLift.g + nLift.b) / 3f;
            float avgGamma = (nGamma.r + nGamma.g + nGamma.b) / 3f;
            float avgGain = (nGain.r + nGain.g + nGain.b) / 3f;

            // Magic numbers
            const float liftScale = 0.1f;
            const float gammaScale = 0.5f;
            const float gainScale = 0.5f;

            float liftR = (nLift.r - avgLift) * liftScale;
            float liftG = (nLift.g - avgLift) * liftScale;
            float liftB = (nLift.b - avgLift) * liftScale;

            float gammaR = Mathf.Pow(2f, (nGamma.r - avgGamma) * gammaScale);
            float gammaG = Mathf.Pow(2f, (nGamma.g - avgGamma) * gammaScale);
            float gammaB = Mathf.Pow(2f, (nGamma.b - avgGamma) * gammaScale);

            float gainR = Mathf.Pow(2f, (nGain.r - avgGain) * gainScale);
            float gainG = Mathf.Pow(2f, (nGain.g - avgGain) * gainScale);
            float gainB = Mathf.Pow(2f, (nGain.b - avgGain) * gainScale);

            const float minGamma = 0.01f;
            float invGammaR = 1f / Mathf.Max(minGamma, gammaR);
            float invGammaG = 1f / Mathf.Max(minGamma, gammaG);
            float invGammaB = 1f / Mathf.Max(minGamma, gammaB);

            lift = new Color(liftR, liftG, liftB);
            gamma = new Color(invGammaR, invGammaG, invGammaB);
            gain = new Color(gainR, gainG, gainB);
        }

        private void GenCurveTexture()
        {
            AnimationCurve master = ColorGrading.curves.master;
            AnimationCurve red = ColorGrading.curves.red;
            AnimationCurve green = ColorGrading.curves.green;
            AnimationCurve blue = ColorGrading.curves.blue;

            Color[] pixels = new Color[256];

            for (float i = 0f; i <= 1f; i += 1f / 255f)
            {
                float m = Mathf.Clamp(master.Evaluate(i), 0f, 1f);
                float r = Mathf.Clamp(red.Evaluate(i), 0f, 1f);
                float g = Mathf.Clamp(green.Evaluate(i), 0f, 1f);
                float b = Mathf.Clamp(blue.Evaluate(i), 0f, 1f);
                pixels[(int)Mathf.Floor(i * 255f)] = new Color(r, g, b, m);
            }

            CurveTexture.SetPixels(pixels);
            CurveTexture.Apply();
        }

        private bool CheckUserLut()
        {
            ValidUserLutSize = (Lut.texture.height == (int)Mathf.Sqrt(Lut.texture.width));
            return ValidUserLutSize;
        }

        private bool CheckSmallAdaptiveRt()
        {
            if (mSmallAdaptiveRt != null)
                return false;

            mAdaptiveRtFormat = RenderTextureFormat.ARGBHalf;

            if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf))
                mAdaptiveRtFormat = RenderTextureFormat.RGHalf;

            mSmallAdaptiveRt = new RenderTexture(1, 1, 0, mAdaptiveRtFormat);
            mSmallAdaptiveRt.hideFlags = HideFlags.DontSave;

            return true;
        }

        private void OnGUI()
        {
            if (Event.current.type != EventType.Repaint)
                return;

            int yoffset = 0;

            // Color grading debug
            if (mInternalLut != null && ColorGrading.enabled && ColorGrading.showDebug)
            {
                Graphics.DrawTexture(new Rect(0f, yoffset, LutSize * LutSize, LutSize), InternalLutRt);
                yoffset += LutSize;
            }

            // Eye Adaptation debug
            if (mSmallAdaptiveRt != null && EyeAdaptation.enabled && EyeAdaptation.showDebug)
            {
                mMaterial.SetPass((int)Pass.AdaptationDebug);
                Graphics.DrawTexture(new Rect(0f, yoffset, 256, 16), mSmallAdaptiveRt, mMaterial);
            }
        }

        private RenderTexture[] mAdaptRts = null;

        [ImageEffectTransformsToLDR]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
#if UNITY_EDITOR
            ValidRenderTextureFormat = true;

            if (source.format != RenderTextureFormat.ARGBHalf && source.format != RenderTextureFormat.ARGBFloat)
                ValidRenderTextureFormat = false;
#endif

            Material.shaderKeywords = null;

            RenderTexture rtSquared = null;

            if (EyeAdaptation.enabled)
            {
                bool freshlyBrewedSmallRt = CheckSmallAdaptiveRt();
                int srcSize = source.width < source.height ? source.width : source.height;

                // Fast lower or equal power of 2
                int adaptiveSize = srcSize;
                adaptiveSize |= (adaptiveSize >> 1);
                adaptiveSize |= (adaptiveSize >> 2);
                adaptiveSize |= (adaptiveSize >> 4);
                adaptiveSize |= (adaptiveSize >> 8);
                adaptiveSize |= (adaptiveSize >> 16);
                adaptiveSize -= (adaptiveSize >> 1);

                rtSquared = RenderTexture.GetTemporary(adaptiveSize, adaptiveSize, 0, mAdaptiveRtFormat);
                Graphics.Blit(source, rtSquared);

                int downsample = (int)Mathf.Log(rtSquared.width, 2f);

                int div = 2;

                if (mAdaptRts == null || mAdaptRts.Length != downsample)
                    mAdaptRts = new RenderTexture[downsample];

                for (int i = 0; i < downsample; i++)
                {
                    mAdaptRts[i] = RenderTexture.GetTemporary(rtSquared.width / div, rtSquared.width / div, 0, mAdaptiveRtFormat);
                    div <<= 1;
                }

                // Downsample pyramid
                var lumRt = mAdaptRts[downsample - 1];
                Graphics.Blit(rtSquared, mAdaptRts[0], Material, (int)Pass.AdaptationLog);
                for (int i = 0; i < downsample - 1; i++)
                {
                    Graphics.Blit(mAdaptRts[i], mAdaptRts[i + 1]);
                    lumRt = mAdaptRts[i + 1];
                }

                // Keeping luminance values between frames, RT restore expected
                mSmallAdaptiveRt.MarkRestoreExpected();

                Material.SetFloat(mAdaptationSpeed, Mathf.Max(EyeAdaptation.speed, 0.001f));

#if UNITY_EDITOR
                if (Application.isPlaying && !freshlyBrewedSmallRt)
                    Graphics.Blit(lumRt, mSmallAdaptiveRt, Material, (int)Pass.AdaptationExpBlend);
                else
                    Graphics.Blit(lumRt, mSmallAdaptiveRt, Material, (int)Pass.AdaptationExp);
#else
                Graphics.Blit(lumRt, m_SmallAdaptiveRt, material, freshlyBrewedSmallRt ? (int)Pass.AdaptationExp : (int)Pass.AdaptationExpBlend);
#endif

                Material.SetFloat(mMiddleGrey, EyeAdaptation.middleGrey);
                Material.SetFloat(mAdaptationMin, Mathf.Pow(2f, EyeAdaptation.min));
                Material.SetFloat(mAdaptationMax, Mathf.Pow(2f, EyeAdaptation.max));
                Material.SetTexture(mLumTex, mSmallAdaptiveRt);
                Material.EnableKeyword("ENABLE_EYE_ADAPTATION");
            }

            int renderPass = (int)Pass.TonemappingOff;

            if (Tonemapping.enabled)
            {
                if (Tonemapping.tonemapper == Tonemapper.Curve)
                {
                    if (mTonemapperDirty)
                    {
                        float range = 1f;

                        if (Tonemapping.curve.length > 0)
                        {
                            range = Tonemapping.curve[Tonemapping.curve.length - 1].time;

                            for (float i = 0f; i <= 1f; i += 1f / 255f)
                            {
                                float c = Tonemapping.curve.Evaluate(i * range);
                                TonemapperCurve.SetPixel(Mathf.FloorToInt(i * 255f), 0, new Color(c, c, c));
                            }

                            TonemapperCurve.Apply();
                        }

                        mTonemapperCurveRange = 1f / range;
                        mTonemapperDirty = false;
                    }

                    Material.SetFloat(mToneCurveRange, mTonemapperCurveRange);
                    Material.SetTexture(mToneCurve, TonemapperCurve);
                }
                else if (Tonemapping.tonemapper == Tonemapper.Neutral)
                {
                    const float scaleFactor = 20f;
                    const float scaleFactorHalf = scaleFactor * 0.5f;

                    float inBlack = Tonemapping.neutralBlackIn * scaleFactor + 1f;
                    float outBlack = Tonemapping.neutralBlackOut * scaleFactorHalf + 1f;
                    float inWhite = Tonemapping.neutralWhiteIn / scaleFactor;
                    float outWhite = 1f - Tonemapping.neutralWhiteOut / scaleFactor;
                    float blackRatio = inBlack / outBlack;
                    float whiteRatio = inWhite / outWhite;

                    const float a = 0.2f;
                    float b = Mathf.Max(0f, Mathf.LerpUnclamped(0.57f, 0.37f, blackRatio));
                    float c = Mathf.LerpUnclamped(0.01f, 0.24f, whiteRatio);
                    float d = Mathf.Max(0f, Mathf.LerpUnclamped(0.02f, 0.20f, blackRatio));
                    const float e = 0.02f;
                    const float f = 0.30f;

                    Material.SetVector(mNeutralTonemapperParams1, new Vector4(a, b, c, d));
                    Material.SetVector(mNeutralTonemapperParams2, new Vector4(e, f, Tonemapping.neutralWhiteLevel, Tonemapping.neutralWhiteClip / scaleFactorHalf));
                }

                Material.SetFloat(mExposure, Tonemapping.exposure);
                renderPass += (int)Tonemapping.tonemapper + 1;
            }

            if (ColorGrading.enabled)
            {
                if (mDirty || !mInternalLut.IsCreated())
                {
                    Color lift, gamma, gain;
                    GenerateLiftGammaGain(out lift, out gamma, out gain);
                    GenCurveTexture();

                    Material.SetVector(mWhiteBalance, GetWhiteBalance());
                    Material.SetVector(mLift, lift);
                    Material.SetVector(mGamma, gamma);
                    Material.SetVector(mGain, gain);
                    Material.SetVector(mContrastGainGamma, new Vector3(ColorGrading.basics.contrast, ColorGrading.basics.gain, 1f / ColorGrading.basics.gamma));
                    Material.SetFloat(mVibrance, ColorGrading.basics.vibrance);
                    Material.SetVector(mHsv, new Vector4(ColorGrading.basics.hue, ColorGrading.basics.saturation, ColorGrading.basics.value));
                    Material.SetVector(mChannelMixerRed, ColorGrading.channelMixer.channels[0]);
                    Material.SetVector(mChannelMixerGreen, ColorGrading.channelMixer.channels[1]);
                    Material.SetVector(mChannelMixerBlue, ColorGrading.channelMixer.channels[2]);
                    Material.SetTexture(mCurveTex, CurveTexture);
                    InternalLutRt.MarkRestoreExpected();
                    Graphics.Blit(IdentityLut, InternalLutRt, Material, (int)Pass.LutGen);
                    mDirty = false;
                }

                Material.EnableKeyword("ENABLE_COLOR_GRADING");

                if (ColorGrading.useDithering)
                    Material.EnableKeyword("ENABLE_DITHERING");

                Material.SetTexture(mInternalLutTex, InternalLutRt);
                Material.SetVector(mInternalLutParams, new Vector3(1f / InternalLutRt.width, 1f / InternalLutRt.height, InternalLutRt.height - 1f));
            }

            if (Lut.enabled && Lut.texture != null && CheckUserLut())
            {
                Material.SetTexture(mUserLutTex, Lut.texture);
                Material.SetVector(mUserLutParams, new Vector4(1f / Lut.texture.width, 1f / Lut.texture.height, Lut.texture.height - 1f, Lut.contribution));
                Material.EnableKeyword("ENABLE_USER_LUT");
            }

            Graphics.Blit(source, destination, Material, renderPass);

            // Cleanup for eye adaptation
            if (EyeAdaptation.enabled)
            {
                for (int i = 0; i < mAdaptRts.Length; i++)
                    RenderTexture.ReleaseTemporary(mAdaptRts[i]);

                RenderTexture.ReleaseTemporary(rtSquared);
            }

#if UNITY_EDITOR
            // If we have an on frame end callabck we need to pass a valid result texture
            // if destination is null we wrote to the backbuffer so we need to copy that out.
            // It's slow and not amazing, but editor only
            if (onFrameEndEditorOnly != null)
            {
                if (destination == null)
                {
                    RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height, 0);
                    Graphics.Blit(source, rt, Material, renderPass);
                    onFrameEndEditorOnly(rt);
                    RenderTexture.ReleaseTemporary(rt);
                    RenderTexture.active = null;
                }
                else
                {
                    onFrameEndEditorOnly(destination);
                }
            }
#endif
        }

        public Texture2D BakeLut()
        {
            Texture2D lut = new Texture2D(InternalLutRt.width, InternalLutRt.height, TextureFormat.RGB24, false, true);
            RenderTexture.active = InternalLutRt;
            lut.ReadPixels(new Rect(0f, 0f, lut.width, lut.height), 0, 0);
            RenderTexture.active = null;
            return lut;
        }
    }
}
