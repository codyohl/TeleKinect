using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityEngine;

namespace WebApplication1.Controllers
{

    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {

            switch (Request.QueryString["method"])
            {
                case "NuiInitialize":
                    NuiInitialize();
                    break;
                case "NuiShutdown":
                    NuiShutdown();
                    break;
                case "NuiCameraElevationSetAngle":
                    NuiCameraElevationSetAngle();
                    break;
                case "NuiCameraElevationGetAngle":
                    NuiCameraElevationGetAngle();
                    break;
                case "NuiImageGetColorPixelCoordinatesFromDepthPixelAtResolution":
                    NuiImageGetColorPixelCoordinatesFromDepthPixelAtResolution();
                    break;
                case "NuiGetSensorCountNuiGetSensorCount":
                    NuiGetSensorCount();
                    break;
                case "NuiSkeletonTrackingEnable":
                    NuiSkeletonTrackingEnable();
                    break;
                case "NuiSkeletonGetNextFrame":
                    NuiSkeletonGetNextFrame();
                    break;
                case "NuiTransformSmooth":
                    NuiTransformSmooth();
                    break;
                case "NuiSkeletonCalculateBoneOrientations":
                    NuiSkeletonCalculateBoneOrientations();
                    break;
                case "NuiImageStreamGetNextFrame":
                    NuiImageStreamGetNextFrame();
                    break;
                case "NuiImageStreamReleaseFrame":
                    NuiImageStreamReleaseFrame();
                    break;
                case "NuiImageStreamSetImageFrameFlags":
                    NuiImageStreamSetImageFrameFlags();
                    break;
                case "NuiImageResolutionToSize":
                    NuiImageResolutionToSize();
                    break;

            }
            

            return null;
        }

        void NuiInitialize()
        {
            try
            {
                int hr = KinectWrapper.NuiInitialize(KinectWrapper.NuiInitializeFlags.UsesSkeleton |
                     KinectWrapper.NuiInitializeFlags.UsesDepthAndPlayerIndex |
                     (false ? KinectWrapper.NuiInitializeFlags.UsesColor : 0));
                if (hr != 0)
                {
                    Response.Write(hr.ToString());
                }
                else
                {
                    Response.Write(hr.ToString());
                }
            }
            catch
            {
                Response.Write("error.....");
            }
        }

        void NuiShutdown()
        {
            
        }

        void NuiCameraElevationSetAngle()
        {
            int angle = int.Parse(Request.QueryString["angle"]);
            int hr = KinectWrapper.NuiCameraElevationSetAngle(angle);

            Response.Write(hr.ToString());
        }

        void NuiCameraElevationGetAngle()
        {

        }

        void NuiImageGetColorPixelCoordinatesFromDepthPixelAtResolution()
        {

        }

        void NuiGetSensorCount()
        {

        }

        void NuiSkeletonTrackingEnable()
        {
            IntPtr hNextFrameEvent = (IntPtr) Int32.Parse(Request.QueryString["hNextFrameEvent"]);
            uint dwFlags = UInt32.Parse(Request.QueryString["dwFlags"]);
            int hr = KinectWrapper.NuiSkeletonTrackingEnable(hNextFrameEvent, dwFlags);

            Response.Write(hr.ToString());
        }

        void NuiSkeletonGetNextFrame()
        {
            KinectWrapper.NuiSkeletonFrame frame = new KinectWrapper.NuiSkeletonFrame();
            
            // retrieves the GET parameters to build up the skeleton frame.
            uint dwMillisecondsToWait = UInt32.Parse(Request.QueryString["dwMillisecondsToWait"]);
            string[] skeletonDataStruct = Request.QueryString["skeletonFrameData"].Split(',');

            //public struct NuiSkeletonFrame
            //{
            //    public Int64 liTimeStamp;
            //    public uint dwFrameNumber;
            //    public uint dwFlags;
            //    public Vector4 vFloorClipPlane;
            //    public Vector4 vNormalToGravity;
            //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
            //    public NuiSkeletonData[] SkeletonData;
            //}
            int i = 0;
            frame.liTimeStamp = Int64.Parse(skeletonDataStruct[i++]);
            frame.dwFrameNumber = uint.Parse(skeletonDataStruct[i++]);
            frame.dwFlags = uint.Parse(skeletonDataStruct[i++]);
            frame.vFloorClipPlane = ParseVector4(skeletonDataStruct[i++], skeletonDataStruct[i++], skeletonDataStruct[i++], skeletonDataStruct[i++]);
            frame.vNormalToGravity = ParseVector4(skeletonDataStruct[i++], skeletonDataStruct[i++], skeletonDataStruct[i++], skeletonDataStruct[i++]);

            // parses each skeleton data
            //     public struct NuiSkeletonData
            //{
            //    public NuiSkeletonTrackingState eTrackingState;
            //    public uint dwTrackingID;
            //    public uint dwEnrollmentIndex_NotUsed;
            //    public uint dwUserIndex;
            //    public Vector4 Position;
            //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
            //    public Vector4[] SkeletonPositions;
            //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
            //    public NuiSkeletonPositionTrackingState[] eSkeletonPositionTrackingState;
            //    public uint dwQualityFlags;
            //}
            for (int j = 0; j < 6; ++j)
            {
                KinectWrapper.NuiSkeletonData data = new KinectWrapper.NuiSkeletonData();
                data.eTrackingState = (KinectWrapper.NuiSkeletonTrackingState)int.Parse(skeletonDataStruct[i++]);
                data.dwTrackingID = uint.Parse(skeletonDataStruct[i++]);
            }
            
            int hr = KinectWrapper.NuiSkeletonGetNextFrame(dwMillisecondsToWait, ref frame);

            Response.Write(hr.ToString() + "," + frame.ToString());
        }

        Vector4 ParseVector4(string x, string y, string z, string w)
        {
            float vector_x = float.Parse(x);
            float vector_y = float.Parse(y);
            float vector_z = float.Parse(z);
            float vector_w = float.Parse(w);

            return new Vector4(vector_x, vector_y, vector_z, vector_w);
        }

        void NuiTransformSmooth()
        {

        }

        void NuiSkeletonCalculateBoneOrientations()
        {

        }

        void NuiImageStreamGetNextFrame()
        {

        }

        void NuiImageStreamReleaseFrame()
        {

        }

        void NuiImageStreamSetImageFrameFlags()
        {

        }

        void NuiImageResolutionToSize()
        {

        }

        public ActionResult About()
        {
            return null;
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}