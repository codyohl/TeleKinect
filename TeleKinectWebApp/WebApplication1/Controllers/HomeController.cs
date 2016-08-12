using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using UnityEngine;

namespace WebApplication1.Controllers
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }

    [NoCache]
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
            
            // retrieves the GET parameters for arguments.
            uint dwMillisecondsToWait = UInt32.Parse(Request.QueryString["dwMillisecondsToWait"]);

            XmlSerializer x = new XmlSerializer(frame.GetType());

            StringWriter s = new StringWriter();
            
            
            int hr = KinectWrapper.NuiSkeletonGetNextFrame(dwMillisecondsToWait, ref frame);

            // TODO: don't immediately smooth here, allow the smoothing api to be called separately.
            var smoothParameters = new KinectWrapper.NuiTransformSmoothParameters();
            smoothParameters.fSmoothing = 0.5f;
            smoothParameters.fCorrection = 0.5f;
            smoothParameters.fPrediction = 0.5f;
            smoothParameters.fJitterRadius = 0.05f;
            smoothParameters.fMaxDeviationRadius = 0.04f;

            hr = hr != 0 ? hr : KinectWrapper.NuiTransformSmooth(ref frame, ref smoothParameters);

            if (hr == 0)
            {
                x.Serialize(s, frame);
            }

            Response.Write(hr.ToString() + "," + s.ToString());
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