using System.Linq;
using Gamelogic;
using Leap.Unity;
using UnityCallbacks;
using UnityEngine;

namespace Memoria
{
	public class PitchGrabObject : GLMonoBehaviour, IOnTriggerStay, IOnMouseOver
	{
		public PinchDetector pinchDetectorLeft;
		public PinchDetector pinchDetectorRight;
		public HapticDetector hapticDetector;

		public bool allowTranslation = true;
		public bool allowRotation = true;
		public bool allowTwoHandScale = true;

		public string[] tagTriggers;

		[HideInInspector]
		public bool isPinched;
		[HideInInspector]
		public MeshRenderer objectMeshRender;
		[HideInInspector]
		public DIOController dioController;
		[HideInInspector]
		public string idName;
		[HideInInspector]
		public bool isSelected;

		public bool isClone;

        //[HideInInspector]
        public bool isSelectedCat1;
        //[HideInInspector]
        public bool isSelectedCat2;
        //[HideInInspector]
        public bool isSelectedCat3;
        //[HideInInspector]
        public bool isSelectedCat4;
        protected DIOManager DioManager
		{
			get { return dioController.DioManager; }
		}

		private Transform _anchor;
		private bool _isLookPointerOn;
		private int _id;

		public void Initialize(DIOController fatherDioController, int id)
		{
			_id = id;

			if (GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "UsePitchGrab") && (pinchDetectorLeft == null || pinchDetectorRight == null))
			{
				Debug.LogWarning(
					"Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
				enabled = false;
				return;
			}

			dioController = fatherDioController;
			enabled = true;
			isSelected = false;
			isClone = false;

            if (GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "BGIIESMode"))
            {
                isSelectedCat1 = isSelectedCat2 = isSelectedCat3 = isSelectedCat4 = false;
            }
            else
            {
                //DELETE THIS untie from DIOManager
                //hapticDetector.Initialize(fatherDioController.DioManager);
            }
			objectMeshRender = GetComponent<MeshRenderer>();

			var pinchControl = new GameObject("DIO Anchor");
			_anchor = pinchControl.transform;
			_anchor.transform.parent = transform.parent;
			transform.parent = _anchor;

			isPinched = false;
			_isLookPointerOn = false;
		}

		public void InitializeMaterial(Texture2D texture2D)
		{
			GetComponent<MeshRenderer>().material.mainTexture = texture2D;
		}

		public void OnDetected()
		{
			//Debug.Log ("Entre al OnDetected");
			if (!_isLookPointerOn)
			{
				//Debug.Log ("El valor de islookPointerOn en OnDetected es:" + _isLookPointerOn);
				_isLookPointerOn = true;


                //DELETE THIS untie bgiiesmode
				if (GLPlayerPrefs.GetBool (ProfileManager.Instance.currentEvaluationScope, "BGIIESMode")) {
					InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.LookPointerEnter (this);
					if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.actualPitchGrabObject == null) {
						//DELETE THIS re-tie button panel
						//DioManager.buttonPanel.EnableZoomIn();
					}

				} else if (GLPlayerPrefs.GetBool (ProfileManager.Instance.currentEvaluationScope, "TIIESMode")) {

					//Debug.Log ("Entre al lookpointerEnter del tiies del OnDetected");
					InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.LookPointerEnter (this);

				
				}
                else
                {
                    InformationObjectManager.Instance.planeImages.lookPointerInstance.LookPointerEnter(this);
                    //DELETE THIS the button panel is still unimplemented. Regardless it only enables/disables the buttons
                    //DioManager.buttonPanel.EnableAccept();
                    /*
                    if (InformationObjectManager.Instance.planeImages.lookPointerInstance.actualPitchGrabObject == null)
                        DioManager.buttonPanel.EnableZoomIn();*/
                }
			}
			else
			{
                //DELETE THIS untie bgiies mode
				if (GLPlayerPrefs.GetBool (ProfileManager.Instance.currentEvaluationScope, "BGIIESMode")) {
					InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.LookPointerStay (this);
				} else if (GLPlayerPrefs.GetBool (ProfileManager.Instance.currentEvaluationScope, "TIIESMode")) {

					//Debug.Log("Entre al lookpointerStay del tiies del OnDetected");
					InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.LookPointerStay (this);

				}
                else
                {
                    InformationObjectManager.Instance.planeImages.lookPointerInstance.LookPointerStay(this);
                }
                
			}
		}

		public void OnUnDetect()
		{
			//Debug.Log ("Entre al OnUnDetect");
			if (_isLookPointerOn)
			{
				//Debug.Log ("El valor de islookPointerOn en OnUnDetected es:" + _isLookPointerOn);
				if (GLPlayerPrefs.GetBool (ProfileManager.Instance.currentEvaluationScope, "BGIIESMode")) {
					
					InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.LookPointerExit (this);
					if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.actualPitchGrabObject == null) {
						_isLookPointerOn = false;
					}
					//DioManager.buttonPanel.DisableZoomIn();
				} else if (GLPlayerPrefs.GetBool (ProfileManager.Instance.currentEvaluationScope, "TIIESMode")) {
				
					//Debug.Log ("Entre al lookPointerExit del OnUnDetect");
					InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.LookPointerExit (this);
					if (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.actualPitchGrabObject == null) {

						//Debug.Log ("Entre al islookPointerOn = false");
						_isLookPointerOn = false;
					}
				}
                else
                {
                    InformationObjectManager.Instance.planeImages.lookPointerInstance.LookPointerExit(this);
                    if (InformationObjectManager.Instance.planeImages.lookPointerInstance.actualPitchGrabObject == null)
                    {
                        
                        _isLookPointerOn = false;
                        //DELETE THIS tie to button panel when reimplemented
                        //DioManager.buttonPanel.DisableAccept();
                    }

                    //DioManager.buttonPanel.DisableZoomIn();
                }
                
			}
		}

		#region UnityCallbacks

		public void OnTriggerStay(Collider other)
		{
			if (!_isLookPointerOn)
				return;

			if (!dioController.DioManager.usePitchGrab)
				return;
            if (!DioManager.bgiiesMode)
            {
                if (DioManager.lookPointerInstance.actualPitchGrabObject != null)
                    if (!DioManager.lookPointerInstance.actualPitchGrabObject.Equals(this))
                        return;
            }
            else
            {
                if (DioManager.lookPointerInstanceBgiies.actualPitchGrabObject != null)
                    if (!DioManager.lookPointerInstanceBgiies.actualPitchGrabObject.Equals(this))
                        return;
            }
			if (!tagTriggers.Contains(other.tag))
				return;

			if (dioController.DioManager.IsAnyDioPitched && !isPinched)
				return;

			if (DioManager.movingSphere || DioManager.lookPointerInstance.zoomingOut)
				return;

			var didUpdate = false;
			didUpdate |= pinchDetectorLeft.DidChangeFromLastFrame;
			didUpdate |= pinchDetectorRight.DidChangeFromLastFrame;

			if (didUpdate)
			{
				transform.SetParent(null, true);
			}

			if (pinchDetectorLeft.IsPinching && pinchDetectorRight.IsPinching)
			{
				TransformDoubleAnchor();
			}
			else if (pinchDetectorLeft.IsPinching)
			{
				TransformSingleAnchor(pinchDetectorLeft);
			}
			else if (pinchDetectorRight.IsPinching)
			{
				TransformSingleAnchor(pinchDetectorRight);
			}
			else
			{
				isPinched = false;
			}

			if (didUpdate)
			{
				transform.SetParent(_anchor, true);
			}
		}

		internal void SetId(string newIdName)
		{
			idName = newIdName;
		}

        //Pre-determined input as to use keyboard and mouse to be able to zoom in and out of the images
        //DELETE THIS no longer necessary with actionmanager
		public void OnMouseOver()
		{
            /*
			if (DioManager.loadingScene.loading)
				return;

			if (DioManager.useKeyboard && DioManager.useMouse && !DioManager.movingSphere)
			{
				if (Input.GetMouseButton(0))
				{
					DioManager.lookPointerInstance.DirectZoomInCall(this, null);
				}
				else if (Input.GetMouseButton(1))
				{
					DioManager.lookPointerInstance.DirectZoomOutCall(null);
				}
			}
            */
		}

		#endregion

		#region Pitch Leap

		private void TransformDoubleAnchor()
		{
			isPinched = true;

			SetInitialState();

			if (allowTranslation)
				_anchor.position = (pinchDetectorLeft.Position + pinchDetectorRight.Position) / 2.0f;

			if (allowRotation)
			{
				Quaternion pp = Quaternion.Lerp(pinchDetectorLeft.Rotation, pinchDetectorRight.Rotation, 0.5f);
				Vector3 u = pp * Vector3.up;
				_anchor.LookAt(pinchDetectorLeft.Position, u);
			}

			if (allowTwoHandScale)
			{
				_anchor.localScale = Vector3.one * Vector3.Distance(pinchDetectorLeft.Position, pinchDetectorRight.Position);
			}
		}

		private void TransformSingleAnchor(PinchDetector singlePinch)
		{
			isPinched = true;

			SetInitialState();

			if (allowTranslation)
			{
				_anchor.position = singlePinch.Position;
			}

			if (allowRotation)
			{
				_anchor.rotation = singlePinch.Rotation;
			}

			_anchor.localScale = Vector3.one;
		}

		private void SetInitialState()
		{
			if (dioController.inVisualizationPosition)
			{
				dioController.inVisualizationPosition = false;
				DioManager.buttonPanel.EnableZoomOut();
				DioManager.buttonPanel.EnableAccept();
				DioManager.buttonPanel.DisableMoveCameraInside();
				DioManager.buttonPanel.DisableMoveCameraOutside();

				DioManager.csvCreator.AddLines("PitchZoomIn", idName);

				DioManager.lookPointerInstance.SetZoomInInitialStatus(this);
			}
		}

		#endregion
		
		public override bool Equals(object o)
		{
			var otherPitchGrabObject = (PitchGrabObject)o;

			if (otherPitchGrabObject == null)
				return false;

			return _id == otherPitchGrabObject._id;
		}

		public override int GetHashCode()
		{
			return _id ^ _id;
		}
	}
}