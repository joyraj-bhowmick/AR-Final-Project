using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

#if ENABLE_WINMD_SUPPORT
using HL2UnityPlugin;
#endif

public class Cameras : MonoBehaviour
{
#if ENABLE_WINMD_SUPPORT
    HL2ResearchMode researchMode;
#endif

    public GameObject LFPreviewPlane = null;
    private Material LFMediaMaterial = null;
    private Texture2D LFMediaTexture = null;
    private byte[] LFFrameData = null;

    public GameObject RFPreviewPlane = null;
    private Material RFMediaMaterial = null;
    private Texture2D RFMediaTexture = null;
    private byte[] RFFrameData = null;

    public GameObject LLPreviewPlane = null;
    private Material LLMediaMaterial = null;
    private Texture2D LLMediaTexture = null;
    private byte[] LLFrameData = null;

    public GameObject RRPreviewPlane = null;
    private Material RRMediaMaterial = null;
    private Texture2D RRMediaTexture = null;
    private byte[] RRFrameData = null;

#if ENABLE_WINMD_SUPPORT
    Windows.Perception.Spatial.SpatialCoordinateSystem unityWorldOrigin;
#endif

    private void Awake()
    {
#if ENABLE_WINMD_SUPPORT
#if UNITY_2020_1_OR_NEWER // note: Unity 2021.2 and later not supported
        IntPtr WorldOriginPtr = UnityEngine.XR.WindowsMR.WindowsMREnvironment.OriginSpatialCoordinateSystem;
        unityWorldOrigin = Marshal.GetObjectForIUnknown(WorldOriginPtr) as Windows.Perception.Spatial.SpatialCoordinateSystem;
        //unityWorldOrigin = Windows.Perception.Spatial.SpatialLocator.GetDefault().CreateStationaryFrameOfReferenceAtCurrentLocation().CoordinateSystem;
#else
        IntPtr WorldOriginPtr = UnityEngine.XR.WSA.WorldManager.GetNativeISpatialCoordinateSystemPtr();
        unityWorldOrigin = Marshal.GetObjectForIUnknown(WorldOriginPtr) as Windows.Perception.Spatial.SpatialCoordinateSystem;
#endif
#endif
    }
    void Start()
    {   

        if (LFPreviewPlane != null)
        {
            LFMediaMaterial = LFPreviewPlane.GetComponent<MeshRenderer>().material;
            LFMediaTexture = new Texture2D(640, 480, TextureFormat.Alpha8, false);
            //LFMediaMaterial.mainTexture = LFMediaTexture;
        }

        if (RFPreviewPlane != null)
        {
            RFMediaMaterial = RFPreviewPlane.GetComponent<MeshRenderer>().material;
            RFMediaTexture = new Texture2D(640, 480, TextureFormat.Alpha8, false);
            //RFMediaMaterial.mainTexture = RFMediaTexture;
        }

        if (LLPreviewPlane != null)
        {
            LLMediaMaterial = LLPreviewPlane.GetComponent<MeshRenderer>().material;
            LLMediaTexture = new Texture2D(640, 480, TextureFormat.Alpha8, false);
            //LLMediaMaterial.mainTexture = LLMediaTexture;
        }

        if (RRPreviewPlane != null)
        {
            RRMediaMaterial = RRPreviewPlane.GetComponent<MeshRenderer>().material;
            RRMediaTexture = new Texture2D(640, 480, TextureFormat.Alpha8, false);
            //RRMediaMaterial.mainTexture = RRMediaTexture;
        }

#if ENABLE_WINMD_SUPPORT
        researchMode = new HL2ResearchMode();

        researchMode.InitializeSpatialCamerasFront();
        researchMode.InitializeSpatialCamerasSide();

        researchMode.SetReferenceCoordinateSystem(unityWorldOrigin);

        researchMode.StartSpatialCamerasFrontLoop();
        researchMode.StartSpatialCamerasSideLoop();
#endif
    }

    bool showRealtimeFeed = false;
    bool startRealtimePreview = true;

    void LateUpdate()
    {
#if ENABLE_WINMD_SUPPORT
        // update LF camera texture
        if (startRealtimePreview && LFPreviewPlane != null && researchMode.LFImageUpdated())
        {
            long ts;
            byte[] frameTexture = researchMode.GetLFCameraBuffer(out ts);
            if (frameTexture.Length > 0)
            {
                if (LFFrameData == null)
                {
                    LFFrameData = frameTexture;
                }
                else
                {
                    System.Buffer.BlockCopy(frameTexture, 0, LFFrameData, 0, LFFrameData.Length);
                }

                if (showRealtimeFeed)
                {
                    LFMediaMaterial.mainTexture = LFMediaTexture;   
                    LFMediaTexture.LoadRawTextureData(LFFrameData);
                    LFMediaTexture.Apply();   
                }
                else
                {
                    LFMediaMaterial.mainTexture = null;
                }
            }
        }
        // update RF camera texture
        if (startRealtimePreview && RFPreviewPlane != null && researchMode.RFImageUpdated())
        {
            long ts;
            byte[] frameTexture = researchMode.GetRFCameraBuffer(out ts);
            if (frameTexture.Length > 0)
            {
                if (RFFrameData == null)
                {
                    RFFrameData = frameTexture;
                }
                else
                {
                    System.Buffer.BlockCopy(frameTexture, 0, RFFrameData, 0, RFFrameData.Length);
                }

                if (showRealtimeFeed)
                {
                    RFMediaMaterial.mainTexture = RFMediaTexture;   
                    RFMediaTexture.LoadRawTextureData(RFFrameData);
                    RFMediaTexture.Apply();   
                }
                else
                {
                    RFMediaMaterial.mainTexture = null;
                }
            }
        }
        // update LL camera texture
        if (startRealtimePreview && LLPreviewPlane != null && researchMode.LLImageUpdated())
        {
            long ts;
            byte[] frameTexture = researchMode.GetLLCameraBuffer(out ts);
            if (frameTexture.Length > 0)
            {
                if (LLFrameData == null)
                {
                    LLFrameData = frameTexture;
                }
                else
                {
                    System.Buffer.BlockCopy(frameTexture, 0, LLFrameData, 0, LLFrameData.Length);
                }

                if (showRealtimeFeed)
                {
                    LLMediaMaterial.mainTexture = LLMediaTexture;   
                    LLMediaTexture.LoadRawTextureData(LLFrameData);
                    LLMediaTexture.Apply();   
                }
                else
                {
                    LLMediaMaterial.mainTexture = null;
                }
            }
        }
        // update RR camera texture
        if (startRealtimePreview && RRPreviewPlane != null && researchMode.RRImageUpdated())
        {
            long ts;
            byte[] frameTexture = researchMode.GetRRCameraBuffer(out ts);
            if (frameTexture.Length > 0)
            {
                if (RRFrameData == null)
                {
                    RRFrameData = frameTexture;
                }
                else
                {
                    System.Buffer.BlockCopy(frameTexture, 0, RRFrameData, 0, RRFrameData.Length);
                }

                if (showRealtimeFeed)
                {
                    RRMediaMaterial.mainTexture = RRMediaTexture;   
                    RRMediaTexture.LoadRawTextureData(RRFrameData);
                    RRMediaTexture.Apply();   
                }
                else
                {
                    RRMediaMaterial.mainTexture = null;
                }
            }
        }
#endif
    }

    #region Button Event Functions
    public void ToggleFeedEvent()
    {
        showRealtimeFeed = !showRealtimeFeed;
    }

    bool renderPointCloud = true;

    public void StopSensorsEvent()
    {
#if ENABLE_WINMD_SUPPORT
        researchMode.StopAllSensorDevice();
#endif
        startRealtimePreview = false;
    }

#endregion
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) StopSensorsEvent();
    }

#if WINDOWS_UWP
    private long GetCurrentTimestampUnix()
    {
        // Get the current time, in order to create a PerceptionTimestamp. 
        Windows.Globalization.Calendar c = new Windows.Globalization.Calendar();
        Windows.Perception.PerceptionTimestamp ts = Windows.Perception.PerceptionTimestampHelper.FromHistoricalTargetTime(c.GetDateTime());
        return ts.TargetTime.ToUnixTimeMilliseconds();
        //return ts.SystemRelativeTargetTime.Ticks;
    }
    private Windows.Perception.PerceptionTimestamp GetCurrentTimestamp()
    {
        // Get the current time, in order to create a PerceptionTimestamp. 
        Windows.Globalization.Calendar c = new Windows.Globalization.Calendar();
        return Windows.Perception.PerceptionTimestampHelper.FromHistoricalTargetTime(c.GetDateTime());
    }
#endif
}