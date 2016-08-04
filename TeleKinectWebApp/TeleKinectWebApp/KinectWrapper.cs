using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using UnityEngine;

namespace WebApplication1
{

    // Wrapper class that holds the various structs and dll imports
    // needed to set up a model with the Kinect.
    public static class KinectWrapper
    {
        public static class Constants
        {
            public const int NuiSkeletonCount = 6;
            public const int NuiSkeletonMaxTracked = 2;
            public const int NuiSkeletonInvalidTrackingID = 0;

            public const float NuiDepthHorizontalFOV = 58.5f;
            public const float NuiDepthVerticalFOV = 45.6f;

            public const int ColorImageWidth = 640;
            public const int ColorImageHeight = 480;
            public const NuiImageResolution ColorImageResolution = NuiImageResolution.resolution640x480;

            public const int DepthImageWidth = 640;
            public const int DepthImageHeight = 480;
            public const NuiImageResolution DepthImageResolution = NuiImageResolution.resolution640x480;

            public const bool IsNearMode = false;

            public const float MinTimeBetweenSameGestures = 0.0f;
            public const float PoseCompleteDuration = 1.0f;
            public const float ClickStayDuration = 2.5f;
        }

        /// <summary>
        ///Structs and constants for interfacing C# with the Kinect.dll 
        /// </summary>

        [Flags]
        public enum NuiInitializeFlags : uint
        {
            UsesAudio = 0x10000000,
            UsesDepthAndPlayerIndex = 0x00000001,
            UsesColor = 0x00000002,
            UsesSkeleton = 0x00000008,
            UsesDepth = 0x00000020,
            UsesHighQualityColor = 0x00000040
        }

        public enum NuiErrorCodes : uint
        {
            FrameNoData = 0x83010001,
            StreamNotEnabled = 0x83010002,
            ImageStreamInUse = 0x83010003,
            FrameLimitExceeded = 0x83010004,
            FeatureNotInitialized = 0x83010005,
            DeviceNotGenuine = 0x83010006,
            InsufficientBandwidth = 0x83010007,
            DeviceNotSupported = 0x83010008,
            DeviceInUse = 0x83010009,

            DatabaseNotFound = 0x8301000D,
            DatabaseVersionMismatch = 0x8301000E,
            HardwareFeatureUnavailable = 0x8301000F,

            DeviceNotConnected = 0x83010014,
            DeviceNotReady = 0x83010015,
            SkeletalEngineBusy = 0x830100AA,
            DeviceNotPowered = 0x8301027F,
        }

        public enum NuiSkeletonPositionIndex : int
        {
            HipCenter = 0,
            Spine = 1,
            ShoulderCenter = 2,
            Head = 3,
            ShoulderLeft = 4,
            ElbowLeft = 5,
            WristLeft = 6,
            HandLeft = 7,
            ShoulderRight = 8,
            ElbowRight = 9,
            WristRight = 10,
            HandRight = 11,
            HipLeft = 12,
            KneeLeft = 13,
            AnkleLeft = 14,
            FootLeft = 15,
            HipRight = 16,
            KneeRight = 17,
            AnkleRight = 18,
            FootRight = 19,
            Count = 20
        }

        public enum NuiSkeletonPositionTrackingState
        {
            NotTracked = 0,
            Inferred,
            Tracked
        }

        public enum NuiSkeletonTrackingState
        {
            NotTracked = 0,
            PositionOnly,
            SkeletonTracked
        }

        public enum NuiImageType
        {
            DepthAndPlayerIndex = 0,    // USHORT
            Color,                      // RGB32 data
            ColorYUV,                   // YUY2 stream from camera h/w, but converted to RGB32 before user getting it.
            ColorRawYUV,                // YUY2 stream from camera h/w.
            Depth                       // USHORT
        }

        public enum NuiImageResolution
        {
            resolutionInvalid = -1,
            resolution80x60 = 0,
            resolution320x240 = 1,
            resolution640x480 = 2,
            resolution1280x960 = 3     // for hires color only
        }

        public enum NuiImageStreamFlags
        {
            None = 0x00000000,
            SupressNoFrameData = 0x0001000,
            EnableNearMode = 0x00020000,
            TooFarIsNonZero = 0x0004000
        }

        [Flags]
        public enum FrameEdges
        {
            None = 0,
            Right = 1,
            Left = 2,
            Top = 4,
            Bottom = 8
        }

        public struct NuiSkeletonData
        {
            public NuiSkeletonTrackingState eTrackingState;
            public uint dwTrackingID;
            public uint dwEnrollmentIndex_NotUsed;
            public uint dwUserIndex;
            public Vector4 Position;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
            public Vector4[] SkeletonPositions;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
            public NuiSkeletonPositionTrackingState[] eSkeletonPositionTrackingState;
            public uint dwQualityFlags;
        }

        public struct NuiSkeletonFrame
        {
            public Int64 liTimeStamp;
            public uint dwFrameNumber;
            public uint dwFlags;
            public Vector4 vFloorClipPlane;
            public Vector4 vNormalToGravity;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
            public NuiSkeletonData[] SkeletonData;
        }

        public struct NuiTransformSmoothParameters
        {
            public float fSmoothing;
            public float fCorrection;
            public float fPrediction;
            public float fJitterRadius;
            public float fMaxDeviationRadius;
        }

        public struct NuiSkeletonBoneRotation
        {
            public Matrix4x4 rotationMatrix;
            public Quaternion rotationQuaternion;
        }

        public struct NuiSkeletonBoneOrientation
        {
            public NuiSkeletonPositionIndex endJoint;
            public NuiSkeletonPositionIndex startJoint;
            public NuiSkeletonBoneRotation hierarchicalRotation;
            public NuiSkeletonBoneRotation absoluteRotation;
        }

        public struct NuiImageViewArea
        {
            public int eDigitalZoom;
            public int lCenterX;
            public int lCenterY;
        }

        public class NuiImageBuffer
        {
            public int m_Width;
            public int m_Height;
            public int m_BytesPerPixel;
            public IntPtr m_pBuffer;
        }

        public struct NuiImageFrame
        {
            public Int64 liTimeStamp;
            public uint dwFrameNumber;
            public NuiImageType eImageType;
            public NuiImageResolution eResolution;
            //[MarshalAsAttribute(UnmanagedType.Interface)]
            public IntPtr pFrameTexture;
            public uint dwFrameFlags_NotUsed;
            public NuiImageViewArea ViewArea_NotUsed;
        }

        public struct NuiLockedRect
        {
            public int pitch;
            public int size;
            //[MarshalAsAttribute(UnmanagedType.U8)] 
            public IntPtr pBits;

        }

        public struct ColorCust
        {
            public byte b;
            public byte g;
            public byte r;
            public byte a;
        }

        public struct ColorBuffer
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 640 * 480, ArraySubType = UnmanagedType.Struct)]
            public ColorCust[] pixels;
        }

        public struct DepthBuffer
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 640 * 480, ArraySubType = UnmanagedType.U2)]
            public ushort[] pixels;
        }

        public struct NuiSurfaceDesc
        {
            uint width;
            uint height;
        }

        [Guid("13ea17f5-ff2e-4670-9ee5-1297a6e880d1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport()]
        public interface INuiFrameTexture
        {
            //[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
            [PreserveSig]
            int BufferLen();
            //[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
            [PreserveSig]
            int Pitch();
            //[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
            [PreserveSig]
            int LockRect(uint Level, ref NuiLockedRect pLockedRect, IntPtr pRect, uint Flags);
            //[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
            [PreserveSig]
            //[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
            int GetLevelDesc(uint Level, ref NuiSurfaceDesc pDesc);
            [PreserveSig]
            int UnlockRect(uint Level);
        }

        /* 
         * kinect NUI (general) functions
         */
#if USE_KINECT_INTERACTION_OR_FACETRACKING || USE_SPEECH_RECOGNITION
	[DllImportAttribute(@"KinectUnityWrapper.dll", EntryPoint = "InitKinectSensor")]
	public static extern int InitKinectSensor(NuiInitializeFlags dwFlags, bool bEnableEvents, int iColorResolution, int iDepthResolution, bool bNearMode);

	[DllImportAttribute(@"KinectUnityWrapper.dll", EntryPoint = "EnableKinectManager")]
	public static extern void EnableKinectManager(bool bEnable);
	
	[DllImportAttribute(@"KinectUnityWrapper.dll", EntryPoint = "UpdateKinectSensor")]
	public static extern int UpdateKinectSensor();
#else
        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiInitialize")]
        public static extern int NuiInitialize(NuiInitializeFlags dwFlags);
#endif

#if USE_KINECT_INTERACTION_OR_FACETRACKING && USE_SPEECH_RECOGNITION
    public static int NuiInitialize(NuiInitializeFlags dwFlags)
	{
		EnableKinectManager(true);
		return InitKinectSensor(dwFlags|NuiInitializeFlags.UsesAudio, true, (int)Constants.ColorImageResolution, (int)Constants.DepthImageResolution, Constants.IsNearMode);
	}
#elif USE_KINECT_INTERACTION_OR_FACETRACKING
    public static int NuiInitialize(NuiInitializeFlags dwFlags)
	{
		EnableKinectManager(true);
		return InitKinectSensor(dwFlags, true, (int)Constants.ColorImageResolution, (int)Constants.DepthImageResolution, Constants.IsNearMode);
	}
#elif USE_SPEECH_RECOGNITION
    public static int NuiInitialize(NuiInitializeFlags dwFlags)
	{
		EnableKinectManager(true);
		return InitKinectSensor(dwFlags|NuiInitializeFlags.UsesAudio, true, (int)Constants.ColorImageResolution, (int)Constants.DepthImageResolution, Constants.IsNearMode);
	}
#endif

#if USE_KINECT_INTERACTION_OR_FACETRACKING || USE_SPEECH_RECOGNITION
	[DllImportAttribute(@"KinectUnityWrapper.dll", EntryPoint = "ShutdownKinectSensor")]
	public static extern void ShutdownKinectSensor();
	
	[DllImportAttribute(@"KinectUnityWrapper.dll", EntryPoint = "SetKinectElevationAngle")]
	public static extern int SetKinectElevationAngle(int sensorAngle);
	
	[DllImportAttribute(@"KinectUnityWrapper.dll", EntryPoint = "GetKinectElevationAngle")]
    public static extern int GetKinectElevationAngle();
	
    public static void NuiShutdown()
	{
		ShutdownKinectSensor();
	}
	
	public static int NuiCameraElevationSetAngle(int angle)
	{
		return SetKinectElevationAngle(angle);
	}
	
    public static int NuiCameraElevationGetAngle(out int plAngleDegrees)
	{
		plAngleDegrees = GetKinectElevationAngle();
		return 0;
	}
#else
        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiShutdown")]
        public static extern void NuiShutdown();

        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiCameraElevationSetAngle")]
        public static extern int NuiCameraElevationSetAngle(int angle);

        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiCameraElevationGetAngle")]
        public static extern int NuiCameraElevationGetAngle(out int plAngleDegrees);
#endif

        [DllImport(@"Kinect10.dll", EntryPoint = "NuiImageGetColorPixelCoordinatesFromDepthPixelAtResolution")]
        public static extern int NuiImageGetColorPixelCoordinatesFromDepthPixelAtResolution(NuiImageResolution eColorResolution, NuiImageResolution eDepthResolution, ref NuiImageViewArea pcViewArea, int lDepthX, int lDepthY, ushort sDepthValue, out int plColorX, out int plColorY);

        [DllImport(@"Kinect10.dll", EntryPoint = "NuiGetSensorCount")]
        public static extern int NuiGetSensorCount(out int pCount);

        /*
         * kinect skeleton functions
         */
#if USE_KINECT_INTERACTION_OR_FACETRACKING || USE_SPEECH_RECOGNITION
    public static int NuiSkeletonTrackingEnable(IntPtr hNextFrameEvent, uint dwFlags)
	{
		// already enabled on init
		return 0;
	}
#else
        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiSkeletonTrackingEnable")]
        public static extern int NuiSkeletonTrackingEnable(IntPtr hNextFrameEvent, uint dwFlags);
#endif

#if USE_KINECT_INTERACTION_OR_FACETRACKING || USE_SPEECH_RECOGNITION
	[DllImport(@"KinectUnityWrapper.dll", EntryPoint = "GetSkeletonFrameLength")]
	public static extern int GetSkeletonFrameLength();

	[DllImport(@"KinectUnityWrapper.dll", EntryPoint = "GetSkeletonFrameData")]
	public static extern bool GetSkeletonFrameData(ref NuiSkeletonFrame pSkeletonData, ref uint iDataBufLen, bool bNewFrame);

	[DllImport(@"KinectUnityWrapper.dll", EntryPoint = "GetNextSkeletonFrame")]
	public static extern int GetNextSkeletonFrame(uint dwWaitMs);
#else
        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiSkeletonGetNextFrame")]
        public static extern int NuiSkeletonGetNextFrame(uint dwMillisecondsToWait, ref NuiSkeletonFrame pSkeletonFrame);
#endif

#if USE_KINECT_INTERACTION_OR_FACETRACKING
    public static int NuiSkeletonGetNextFrame(uint dwMillisecondsToWait, ref NuiSkeletonFrame pSkeletonFrame)
	{
		uint iFrameLength = (uint)GetSkeletonFrameLength();
		bool bSuccess = GetSkeletonFrameData(ref pSkeletonFrame, ref iFrameLength, true);
		return bSuccess ? 0 : -1;
	}
#elif USE_SPEECH_RECOGNITION
    public static int NuiSkeletonGetNextFrame(uint dwMillisecondsToWait, ref NuiSkeletonFrame pSkeletonFrame)
	{
		int hr = GetNextSkeletonFrame(dwMillisecondsToWait);
		if(hr == 0)
		{
			uint iFrameLength = (uint)GetSkeletonFrameLength();
			bool bSuccess = GetSkeletonFrameData(ref pSkeletonFrame, ref iFrameLength, true);
			
			return bSuccess ? 0 : -1;
		}
		
		return hr;
	}
#endif

        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiTransformSmooth")]
        public static extern int NuiTransformSmooth(ref NuiSkeletonFrame pSkeletonFrame, ref NuiTransformSmoothParameters pSmoothingParams);

        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiSkeletonCalculateBoneOrientations")]
        public static extern int NuiSkeletonCalculateBoneOrientations(ref NuiSkeletonData pSkeletonData, NuiSkeletonBoneOrientation[] pBoneOrientations);

        /*
         * kinect video functions
         */
#if USE_KINECT_INTERACTION_OR_FACETRACKING || USE_SPEECH_RECOGNITION
	[DllImport(@"KinectUnityWrapper.dll", EntryPoint = "GetColorStreamHandle")]
	public static extern IntPtr GetColorStreamHandle();

	[DllImport(@"KinectUnityWrapper.dll", EntryPoint = "GetDepthStreamHandle")]
	public static extern IntPtr GetDepthStreamHandle();

	[DllImport(@"KinectUnityWrapper.dll", EntryPoint = "GetColorFrameData")]
	public static extern bool GetColorFrameData(IntPtr btVideoBuf, ref uint iVideoBufLen, bool bGetNewFrame);
	
	[DllImport(@"KinectUnityWrapper.dll", EntryPoint = "GetDepthFrameData")]
	public static extern bool GetDepthFrameData(IntPtr shDepthBuf, ref uint iDepthBufLen, bool bGetNewFrame);
	
    [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageStreamOpen")]
    public static extern int NuiImageStreamOpen(NuiImageType eImageType, NuiImageResolution eResolution, uint dwImageFrameFlags_NotUsed, uint dwFrameLimit, IntPtr hNextFrameEvent, ref IntPtr phStreamHandle);
#endif

        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageStreamGetNextFrame")]
        public static extern int NuiImageStreamGetNextFrame(IntPtr phStreamHandle, uint dwMillisecondsToWait, ref IntPtr ppcImageFrame);

        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageStreamReleaseFrame")]
        public static extern int NuiImageStreamReleaseFrame(IntPtr phStreamHandle, IntPtr ppcImageFrame);

        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageStreamSetImageFrameFlags")]
        public static extern int NuiImageStreamSetImageFrameFlags(IntPtr phStreamHandle, NuiImageStreamFlags dvImageFrameFlags);

        [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageResolutionToSize")]
        public static extern int NuiImageResolutionToSize(NuiImageResolution eResolution, out uint frameWidth, out uint frameHeight);


    }
}