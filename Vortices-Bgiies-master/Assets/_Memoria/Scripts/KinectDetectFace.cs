using System.Linq;
using Gamelogic;
using UnityCallbacks;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.Face;
using System;

namespace Memoria
{
    public class KinectDetectFace : GLMonoBehaviour, IFixedUpdate
    {
        private KinectSensor kinectSensor;
        private int bodyCount;
        private Body[] bodies;
        private FaceFrameSource[] faceFrameSources;
        private FaceFrameReader[] faceFrameReaders;
        private BodySourceManager _BodyManager;
        private int updateFrame;

        private const double FaceRotationIncrementInDegrees = 1.0f;

        private float deltaX = -5f;
        private float deltaY = 8.5f;

        private float multX = -0.02216f;
        private float multY = 0.0233f;


        public GameObject BodySrcManager;
        private BodySourceManager bodyManager;

        private Vector3 posRay;
        private Vector3 posWorld;

        DIOManager dioManager;

        public Ray ray;
        bool initialize = false;
        public void Initialize(DIOManager dioManager)
        {
            this.dioManager = dioManager;
            this.BodySrcManager = dioManager.bodySrcManager;

            updateFrame = 0;

            // one sensor is currently supported
            kinectSensor = KinectSensor.GetDefault();

            // set the maximum number of bodies that would be tracked by Kinect
            bodyCount = kinectSensor.BodyFrameSource.BodyCount;

            // allocate storage to store body objects
            bodies = new Body[bodyCount];

            if (BodySrcManager == null)
            {
                Debug.Log("Falta asignar Game Object as BodySrcManager");
            }
            else
            {
                bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
            }

            // specify the required face frame results
            FaceFrameFeatures faceFrameFeatures =
                FaceFrameFeatures.BoundingBoxInColorSpace
                    | FaceFrameFeatures.PointsInColorSpace
                    | FaceFrameFeatures.BoundingBoxInInfraredSpace
                    | FaceFrameFeatures.PointsInInfraredSpace
                    | FaceFrameFeatures.RotationOrientation
                    | FaceFrameFeatures.FaceEngagement
                    | FaceFrameFeatures.Glasses
                    | FaceFrameFeatures.Happy
                    | FaceFrameFeatures.LeftEyeClosed
                    | FaceFrameFeatures.RightEyeClosed
                    | FaceFrameFeatures.LookingAway
                    | FaceFrameFeatures.MouthMoved
                    | FaceFrameFeatures.MouthOpen;

            // create a face frame source + reader to track each face in the FOV
            faceFrameSources = new FaceFrameSource[bodyCount];
            faceFrameReaders = new FaceFrameReader[bodyCount];
            for (int i = 0; i < bodyCount; i++)
            {
                // create the face frame source with the required face frame features and an initial tracking Id of 0
                faceFrameSources[i] = FaceFrameSource.Create(kinectSensor, 0, faceFrameFeatures);

                // open the corresponding reader
                faceFrameReaders[i] = faceFrameSources[i].OpenReader();
            }
            initialize = true;
        }
        public void FixedUpdate()
        {
            if (!initialize)
                return;
            else
            {
                if (dioManager.lookPointerInstanceBgiies.zoomActive)
                {
                    return;
                }
            }
            if (updateFrame < 1)
            {
                updateFrame++;
                return;
            }
            updateFrame = 0;
            // get bodies either from BodySourceManager object get them from a BodyReader
            var bodySourceManager = bodyManager.GetComponent<BodySourceManager>();
            bodies = bodySourceManager.GetData();
            if (bodies == null)
            {
                return;
            }


            // iterate through each body and update face source
            for (int i = 0; i < bodyCount; i++)
            {
                // check if a valid face is tracked in this face source				
                if (faceFrameSources[i].IsTrackingIdValid)
                {
                    using (FaceFrame frame = faceFrameReaders[i].AcquireLatestFrame())
                    {
                        if (frame != null)
                        {
                            if (frame.TrackingId == 0)
                            {
                                continue;
                            }

                            // do something with result
                            var result = frame.FaceFrameResult;

                            // extract face rotation in degrees as Euler angles
                            if (result.FaceRotationQuaternion != null)
                            {
                                int pitch, yaw, roll;
                                ExtractFaceRotationInDegrees(result.FaceRotationQuaternion, out pitch, out yaw, out roll);

                                posRay = new Vector3(yaw * multX, pitch * multY , 0.45f);
                                posWorld = Camera.main.WorldToScreenPoint(posRay);
                                ray = Camera.main.ScreenPointToRay(posWorld);                                
                            }
                        }
                    }

                }
                else
                {
                    // check if the corresponding body is tracked 
                    if (bodies[i].IsTracked)
                    {
                        // update the face frame source to track this body
                        faceFrameSources[i].TrackingId = bodies[i].TrackingId;
                    }
                }
            }
        }
        private static void ExtractFaceRotationInDegrees(Windows.Kinect.Vector4 rotQuaternion, out int pitch, out int yaw, out int roll)
        {
            double x = rotQuaternion.X;
            double y = rotQuaternion.Y;
            double z = rotQuaternion.Z;
            double w = rotQuaternion.W;

            // convert face rotation quaternion to Euler angles in degrees
            double yawD, pitchD, rollD;
            pitchD = Math.Atan2(2 * ((y * z) + (w * x)), (w * w) - (x * x) - (y * y) + (z * z)) / Math.PI * 180.0;
            yawD = Math.Asin(2 * ((w * y) - (x * z))) / Math.PI * 180.0;
            rollD = Math.Atan2(2 * ((x * y) + (w * z)), (w * w) + (x * x) - (y * y) - (z * z)) / Math.PI * 180.0;

            // clamp the values to a multiple of the specified increment to control the refresh rate
            double increment = FaceRotationIncrementInDegrees;
            pitch = (int)(Math.Floor((pitchD + ((increment / 2.0) * (pitchD > 0 ? 1.0 : -1.0))) / increment) * increment);
            yaw = (int)(Math.Floor((yawD + ((increment / 2.0) * (yawD > 0 ? 1.0 : -1.0))) / increment) * increment);
            roll = (int)(Math.Floor((rollD + ((increment / 2.0) * (rollD > 0 ? 1.0 : -1.0))) / increment) * increment);
        }

    }
}