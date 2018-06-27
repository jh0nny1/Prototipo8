using UnityEngine;
using System.Collections.Generic;
using Gamelogic;
using UnityCallbacks;
using UnityEngine.UI;

namespace Memoria
{
	public class LoadingScene : GLMonoBehaviour, IOnEnable, IUpdate
	{
		public Text porcentageText;
		public List<GameObject> activateAtEndObjects;
		public List<GameObject> deactivateAtEndObjects;

		[HideInInspector]
		public bool loading;
        bool initialized = false;

		private float _porcentage;
		private DIOManager _dioManager;

		public void OnEnable()
		{
			loading = true;
			_porcentage = 0.00f;
		}

		public void Initialize(DIOManager dioManager)
		{
			_dioManager = dioManager;

			if (dioManager.useLeapMotion)
				dioManager.leapMotionRig.leapSpace.SetActive(false);

            initialized = true;
		}

        //DELETE THIS

        public void Initialize()
        {
            string Scope = ProfileManager.Instance.currentEvaluationScope;
            //this needs the LeapMotion manager
            /*
			if (GLPlayerPrefs.GetBool (Scope, "useLeapMotion"))
				Debug.Log ("Entre al initialize del loadingScene");
                InterfaceManager.Instance.leapMotionManager.leapMotionRig.leapSpace.SetActive(false);
            */
            initialized = true;
        }
        

        public void Update()
		{
			if (!loading)
				return;

            if (!initialized)
                return;

            //_porcentage = _dioManager.loadImageController.ImagesLoaded / (float)_dioManager.loadImageController.images;
            _porcentage = InformationObjectManager.Instance.planeImages.loadImageController.ImagesLoaded / (float)InformationObjectManager.Instance.planeImages.loadImageController.images;
			porcentageText.text = _porcentage.ToString("000%");

			if (!(_porcentage >= 1.0f))
				return;

			loading = false;

			foreach (var activateAtEndObject in activateAtEndObjects)
			{
				activateAtEndObject.SetActive(true);
			}

			foreach (var deactivateAtEndObject in deactivateAtEndObjects)
			{
				deactivateAtEndObject.SetActive(false);
			}

            //DELETE THIS needs the leap motion manager
            /*
			if (_dioManager.useLeapMotion)
				_dioManager.leapMotionRig.leapSpace.SetActive(true);
            */
		}
	}
}