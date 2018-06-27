using Gamelogic;
using System.Collections;
using UnityCallbacks;
using UnityEngine;

namespace Memoria
{
	public class LookPointerRaycasting : GLMonoBehaviour, IOnValidate
	{
		public float maxDistance;
		public LayerMask ignoredLayerMask;
		public bool debugOutput;

		private DIOManager _dioManager;
		private RaycastHit _raycastHit;
		private Ray _ray;
		private Vector3 _forwardVector;
		private PitchGrabObject _actualPitchGrabObject;
        bool initialized = false;
        //testing

        public void Initialize()
        {
            initialized = true;
        }

		public void Initialize(DIOManager dioManager)
		{
			_dioManager = dioManager;
            initialized = true;
		}

		public void OnValidate()
		{
			maxDistance = Mathf.Max(0.0f, maxDistance);
		}

        public void RegisterRay(PitchGrabObject foto)
        {
            var action = "Move ray vector";
            MOTIONSManager.Instance.AddLines(action, foto.idName);
        }

        public void ResetActualPitchGrabObject()
        {
            _actualPitchGrabObject = null;
        }

        public void CreateRay()
        {
            _forwardVector = transform.TransformDirection(Vector3.forward);
            if (EyetrackerManager.Instance._useEyetribe || EyetrackerManager.Instance._useMouse)
            {
                _ray = EyetrackerManager.Instance.screenPoint;
            }
            else
            {
                _ray = new Ray(transform.position, _forwardVector);
            }
        }

        public void CreateRay(Vector3 vectorOrigin, Vector3 vectorDirection, int currentVisualization)
        {
            Ray ray = new Ray(vectorOrigin, vectorDirection);
            maxDistance = 1f;
            CastRay(ray, currentVisualization);
        }

        public void CreateRay(Ray ray, int currentVisualization)
        {
            maxDistance = 1f;
            CastRay(ray, currentVisualization);
        }

        //called only if categories are being used
        public void CreateRayCategories(Ray ray, int currentVisualization)
        {
			//Debug.Log ("Entre al CreateRayCategories");
			maxDistance = 1f;
            //if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomActive)
              //  return;
            //DELETE THIS tie the function of the timer to the MOTIONSManager
            /*
            if (_dioManager.mouseInput)
            {
                if ((_dioManager.panelBgiies.posInicialMouse != Input.mousePosition) && !_dioManager.panelBgiies.primerMovimiento)
                {
                    _dioManager.panelBgiies.primerMovimiento = true;
                    _dioManager.panelBgiies.InitExperiment();
                }

                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                if (_dioManager.lookPointerInstanceBgiies.zoomActive)
                    return;
                //DELETE THIS remember to add this to the Kinect Manager
                _ray = _dioManager.kinectFace.ray;
                if ((_ray.direction != Vector3.zero) && !_dioManager.panelBgiies.primerMovimiento)
                {
                    _dioManager.panelBgiies.primerMovimiento = true;
                    _dioManager.panelBgiies.InitExperiment();
                }
            }*/
            CastRayCategories(ray, currentVisualization);
        }

        //This creates the ray
        public void CastRay(Ray ray, int currentVisualizationId)
        {
			//Debug.DrawLine(_ray.origin, _ray.direction * maxDistance, Color.red);
            //Debug.Log(ray.direction.ToString() + " " + currentVisualizationId.ToString());
            if (Physics.Raycast(ray, out _raycastHit, maxDistance, ignoredLayerMask))
            {
				//Debug.Log ("Entre al if del raycast del mouse");
                var posiblePitcheGrabObject = _raycastHit.transform.gameObject.GetComponent<PitchGrabObject>();

                if (posiblePitcheGrabObject == null)
                    return;

                if (posiblePitcheGrabObject.dioController.visualizationController.id != currentVisualizationId)
                {
                    if (_actualPitchGrabObject != null)
                    {
                        _actualPitchGrabObject.OnUnDetect();    //funcion para ignorar las imagenes que se encuentren en otras vistas
                    }

                    return;
                }


                if (_actualPitchGrabObject == null)
                {
					
                    _actualPitchGrabObject = posiblePitcheGrabObject;   //en una primera instancia actualPitch es null, la primera vez que toca una foto valida toma el valor de posiblePitch
                    RegisterRay(_actualPitchGrabObject);
                }
                else
                {
                    if (_actualPitchGrabObject.idName != posiblePitcheGrabObject.idName)    //si actualPitch no coincide con posiblePitch se actualiza actualPitch
                    {
                        _actualPitchGrabObject.OnUnDetect();            // actualPitch se hace null
                        _actualPitchGrabObject = posiblePitcheGrabObject;   //se le asigna el valor de posiblePitch
                        RegisterRay(_actualPitchGrabObject);
                    }
                }
                _actualPitchGrabObject.OnDetected();        //activa el MARCAR de buttonPanel y activa LookPointerStay que aplica ZoomIn(iluminar foto)
            }
            else
            {
				if (_actualPitchGrabObject == null)
				{
                    //DELETE THIS tie button panel
                    //_dioManager.buttonPanel.DisableZoomIn();
                    return;
				}
                
                _actualPitchGrabObject.OnUnDetect();        //si actualPitch no era nulo se hace null
            }
        }

        public void CastRayCategories(Ray ray, int currentVisualizationId)
        {
			//Debug.DrawLine(_ray.origin, _ray.direction * maxDistance, Color.red);
            if (Physics.Raycast(ray, out _raycastHit, maxDistance, ignoredLayerMask))
            {
				//Debug.Log ("Entre al if del raycast del leapmotion");
                var posiblePitcheGrabObject = _raycastHit.transform.gameObject.GetComponent<PitchGrabObject>();


				if (posiblePitcheGrabObject == null) {
					
					return;
				}

					
                

				if (GLPlayerPrefs.GetBool (ProfileManager.Instance.currentEvaluationScope, "BGIIESMode")) {
				
					if (!VisualizationManager.Instance.planeVisualization.panelBgiies.mostrarCategoria)
					{

						if (posiblePitcheGrabObject.dioController.visualizationController.id != currentVisualizationId)
						{

							if (_actualPitchGrabObject != null)
							{
								//Debug.Log ("LLegue al antes del _ActualPitchGrabObject.OnUnDetect() del bgieesmode");
								_actualPitchGrabObject.OnUnDetect();    //funcion para ignorar las imagenes que se encuentren en otras vistas
							}

							return;
						}
					}
				}

                

                
                if (_actualPitchGrabObject == null)
                {
					
                    _actualPitchGrabObject = posiblePitcheGrabObject;   //en una primera instancia actualPitch es null, la primera vez que toca una foto valida toma el valor de posiblePitch
                    RegisterRay(_actualPitchGrabObject);
                }
                else
                {
					
                    if (_actualPitchGrabObject.idName != posiblePitcheGrabObject.idName)    //si actualPitch no coincide con posiblePitch se actualiza actualPitch
                    {
						//Debug.Log ("LLegue al antes del _ActualPitchGrabObject.OnUnDetect() antes del _ActualPitchGrab OnDetected");
                        _actualPitchGrabObject.OnUnDetect();            // actualPitch se hace null
                        _actualPitchGrabObject = posiblePitcheGrabObject;   //se le asigna el valor de posiblePitch
                        RegisterRay(_actualPitchGrabObject);
                    }
                }
				//Debug.Log ("LLegue al antes del _ActualPitchGrabObject.OnDetected()");
                _actualPitchGrabObject.OnDetected();        //activa el MARCAR de buttonPanel y activa LookPointerStay que aplica ZoomIn(iluminar foto)
            }
            else
            {
				if (_actualPitchGrabObject == null)
				{
					
					return;
				}
				//Debug.Log ("LLegue al antes del _ActualPitchGrabObject.OnUnDetect() despues del _ActualPitchGrab OnDetected");
                _actualPitchGrabObject.OnUnDetect();        //si actualPitch no era nulo se hace null
            }
        }
	}
}