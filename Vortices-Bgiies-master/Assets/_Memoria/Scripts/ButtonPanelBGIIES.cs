using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using OpenGlove_API_C_Sharp_HL;
using OpenGlove_API_C_Sharp_HL.ServiceReference1;
using Gamelogic;
using System.Linq;

namespace Memoria
{
    public enum Categorias
    {
        Categoria1,
        Categoria2,
        Categoria3,
        Categoria4
    };
    public class ButtonPanelBGIIES : ButtonPanel
    {
        public Text txtTime;
        private float time;
        private string text;
        float min;
        float seg;
		int seconds;

        public Button bt1;
        public EventTrigger bt1ClicAction;

        public Button bt2;
        public EventTrigger bt2ClicAction;

        public Button bt3;
        public EventTrigger bt3ClicAction;

        public Button bt4;
        public EventTrigger bt4ClicAction;

        public Color aceptBt1;
        public Color aceptBt2;
        public Color aceptBt3;
        public Color aceptBt4;

        public Vector3 posInicialMouse;
        public bool primerMovimiento;

        public bool mostrarCategoria = false;
        public string nombreCategoria;

        //plane visualization combination
        [HideInInspector]
        public Action initialPlaneAction;
        [HideInInspector]
        public Action finalPlaneAction;

        //public Categorias categorias;

        public override void Initialize(DIOManager dioManager)
        {
            base.dioManager = dioManager;
            EnableMoveCameraInside();
            EnableMoveCameraOutside();

            bt1.name = "floraYfauna";
            bt2.name = "superficies";
            bt3.name = "mitigaciones";
            bt4.name = "estructuras";

            NegativeAllButtons();

            mostrarCategoria = false;
            primerMovimiento = false;

            if (GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope,"useMouse"))
                posInicialMouse = Input.mousePosition;

            initialPlaneAction = () =>
            {
                DisableMoveCameraInside();
                DisableMoveCameraOutside();
            };

            finalPlaneAction = () =>
            {
                EnableMoveCameraInside();
                EnableMoveCameraOutside();
            };
        }
        
        //initialization untied from dio manager
        public void Initialize()
        {
            EnableMoveCameraInside();
            EnableMoveCameraOutside();


            bt1.name = "floraYfauna";
            bt2.name = "superficies";
            bt3.name = "mitigaciones";
            bt4.name = "estructuras";

            NegativeAllButtons();

            mostrarCategoria = false;
            primerMovimiento = false;

            if (GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "useMouse"))
                posInicialMouse = Input.mousePosition;

            initialPlaneAction = () =>
            {
                DisableMoveCameraInside();
                DisableMoveCameraOutside();
            };

            finalPlaneAction = () =>
            {
                EnableMoveCameraInside();
                EnableMoveCameraOutside();
            };

        }

		void OnEnable(){
			
			InitExperiment ();
		}

		public void InitExperiment()
		{
			Debug.Log ("Entre al initExperimento del BGIIES");
			string Scope = ProfileManager.Instance.currentEvaluationScope;
			seconds = GLPlayerPrefs.GetInt (Scope, "EvaluationTime");
			seconds = seconds * 60;
			StartCoroutine(CalculaTiempo(seconds));
		}

		void Update(){
			TimeSpan span = TimeSpan.FromSeconds (seconds);
			txtTime.text = "Tiempo: " + span.ToString ();
			//timer.text = span.ToString ();
		}

		IEnumerator CalculaTiempo(int value)
		{
			seconds = value;
			while (seconds > 0) {
				seconds--;
				yield return new WaitForSeconds (1f);

			}

			EndExperiment ();
		}

		/* older timer
        public void InitExperiment()
        {
            StartCoroutine(CalculaTiempo());
        }

        IEnumerator CalculaTiempo()
        {
            min = 0;
            seg = 0;
            while (min != 1)
            {
                if (seg == 60)
                {
                    min++;
                    seg = 0;
                }
                if (min.ToString().Length == 1)
                    text = "Tiempo: 0" + min.ToString();
                else
                    text = "Tiempo: " + min.ToString();

                if (seg.ToString().Length == 1)
                    text = text + ":0" + seg.ToString();
                else
                    text = text + ":" + seg.ToString();
                txtTime.text = text;
                seg++;
                yield return new WaitForSeconds(1f);
            }
            EndExperiment();
        }
        */

        public override void Inside()
        {
            Debug.Log("llega a Inside");
            if (!mostrarCategoria)
                VisualizationManager.Instance.planeVisualization.MovePlaneInside(1, initialPlaneAction, finalPlaneAction);
            else
                InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.InsideCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.actualListaCat);
        }

        public override void Outside()
        {
            Debug.Log("llega a Outside");
            if (!mostrarCategoria)
                VisualizationManager.Instance.planeVisualization.MovePlaneOutside(1, initialPlaneAction, finalPlaneAction);
            else
                InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.OutsideCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.actualListaCat);
        }

        public void ZoomIn()
        {
            InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.DirectZoomInCall(null);
        }

        public void ZoomOut()
        {
            InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.DirectZoomOutCall(null);
        }

        public void SelectBt1()
        {
            if (!mostrarCategoria)
            {
                if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomActive)
                    InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.SelectCat1();
                else
                    IngresarACategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat1, bt1, new Button[] { bt2, bt3, bt4 }, 0);
            }
            else
            {
                if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomActive)
                    DeseleccionarFromCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat1, (int)Categorias.Categoria1, bt1, aceptBt1);
                else
                    SalirDeCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat1, new Button[] { bt2, bt3, bt4 });
            }
        }

        public void CategoryBt1()
        {
            if (!mostrarCategoria)
            {
                IngresarACategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat1, bt1, new Button[] { bt2, bt3, bt4 }, 0);
            }
            else
            {
                SalirDeCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat1, new Button[] { bt2, bt3, bt4 });
            }
        }

        public void SelectBt2()
        {
            if (!mostrarCategoria)
            {
                if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomActive)
                    InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.SelectCat2();
                else
                    IngresarACategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat2, bt2, new Button[] { bt1, bt3, bt4 }, 0);
            }
            else
            {
                if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomActive)
                    DeseleccionarFromCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat2, (int)Categorias.Categoria2, bt2, aceptBt2);
                else
                    SalirDeCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat2, new Button[] { bt1, bt3, bt4 });
            }
        }

        public void CategoryBt2()
        {
            if (!mostrarCategoria)
            {
                IngresarACategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat2, bt2, new Button[] { bt1, bt3, bt4 }, 0);
            }
            else
            {
                SalirDeCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat2, new Button[] { bt1, bt3, bt4 });
            }
        }

        public void SelectBt3()
        {
            if (!mostrarCategoria)
            {
                if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomActive)
                    InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.SelectCat3();
                else
                    IngresarACategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat3, bt3, new Button[] { bt1, bt2, bt4 }, 0);
            }
            else
            {
                if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomActive)
                    DeseleccionarFromCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat3, (int)Categorias.Categoria3, bt3, aceptBt3);
                else
                    SalirDeCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat3, new Button[] { bt1, bt2, bt4 });
            }
        }

        public void CategoryBt3()
        {
            if (!mostrarCategoria)
            {
                IngresarACategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat3, bt3, new Button[] { bt1, bt2, bt4 }, 0);
            }
            else
            {
                SalirDeCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat3, new Button[] { bt1, bt2, bt4 });
            }
        }

        public void SelectBt4()
        {
            if (!mostrarCategoria)
            {
                if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomActive)
                    InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.SelectCat4();
                else
                    IngresarACategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat4, bt4, new Button[] { bt1, bt2, bt3 }, 0);
            }
            else
            {
                if (InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.zoomActive)
                    DeseleccionarFromCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat4, (int)Categorias.Categoria4, bt4, aceptBt4);
                else
                    SalirDeCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat4, new Button[] { bt1, bt2, bt3 });
            }
        }

        public void CategoryBt4()
        {
            if (!mostrarCategoria)
            {
                IngresarACategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat4, bt4, new Button[] { bt1, bt2, bt3 }, 0);
            }
            else
            {
                SalirDeCategoria(InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.listaCat4, new Button[] { bt1, bt2, bt3 });
            }
        }

        public void IngresarACategoria(List<PitchGrabObject> lista, Button botonCategoria, Button[] botonesDesactive, int indexPhotos)
        {
            NegativeAllButtons();
            mostrarCategoria = true;
            nombreCategoria = botonCategoria.tag;
            InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.MostrarCategoria(lista, indexPhotos);
            PositiveCatButton(botonCategoria);
            ActiveDesactiveButtons(botonesDesactive, false);
        }

        public void SalirDeCategoria(List<PitchGrabObject> categoriaActual, Button[] botonesActive)
        {
            InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.MostrarImagenes(categoriaActual, VisualizationManager.Instance.planeVisualization.planeControllers.SelectMany(sc => sc.dioControllerList).ToList());
            mostrarCategoria = false;
            ActiveDesactiveButtons(botonesActive, true);
            EnableMoveCameraInside();
            EnableMoveCameraOutside();
        }

        public void DeseleccionarFromCategoria(List<PitchGrabObject> listaActual, int nombreCategoria, Button botonCategoria, Color colorBotonEncendido)
        {
            InformationObjectManager.Instance.planeImages.lookPointerInstanceBGIIES.DeseleccionarFromCategoria(listaActual, nombreCategoria, botonCategoria, colorBotonEncendido, VisualizationManager.Instance.planeVisualization.planeControllers.SelectMany(sc => sc.dioControllerList).ToList());
        }
        public void changeColor(GameObject obj, Color color)
        {
            Renderer rend = obj.GetComponent<MeshRenderer>();
            rend.material.color = color;
        }
        public void noInteractableButtons()
        {
            bt1.interactable = false;
            bt2.interactable = false;
            bt3.interactable = false;
            bt4.interactable = false;
        }


        public void interactableButtons(PitchGrabObject pitchGrabObject)
        {

            bt1.interactable = true;
            bt2.interactable = true;
            bt3.interactable = true;
            bt4.interactable = true;

            if (pitchGrabObject == null)
            {
                if (pitchGrabObject.isSelectedCat1)
                    PositiveCatButton(bt1);
                else
                    NegativeCatButton(bt1);
                if (pitchGrabObject.isSelectedCat2)
                    PositiveCatButton(bt2);
                else
                    NegativeCatButton(bt2);
                if (pitchGrabObject.isSelectedCat3)
                    PositiveCatButton(bt3);
                else
                    NegativeCatButton(bt3);
                if (pitchGrabObject.isSelectedCat4)
                    PositiveCatButton(bt4);
                else
                    NegativeCatButton(bt4);
            }
            else
            {
                if (pitchGrabObject.isSelectedCat1)
                    PositiveCatButton(bt1);
                else
                    NegativeCatButton(bt1);
                if (pitchGrabObject.isSelectedCat2)
                    PositiveCatButton(bt2);
                else
                    NegativeCatButton(bt2);
                if (pitchGrabObject.isSelectedCat3)
                    PositiveCatButton(bt3);
                else
                    NegativeCatButton(bt3);
                if (pitchGrabObject.isSelectedCat4)
                    PositiveCatButton(bt4);
                else
                    NegativeCatButton(bt4);
            }
        }

        public void EndExperiment()
        {
            SceneManager.LoadScene("ConfigPrototype");
			           
        }

        public void PositiveCatButton(Button boton)
        {
            ColorBlock cb = boton.colors;
            cb.normalColor = cb.highlightedColor;
            boton.colors = cb;
        }

        public void NegativeCatButton(Button boton)
        {
            ColorBlock cb = boton.colors;
            if (boton.name == "floraYfauna")
                cb.normalColor = aceptBt1;
            else if (boton.name == "superficies")
                cb.normalColor = aceptBt2;
            else if (boton.name == "mitigaciones")
                cb.normalColor = aceptBt3;
            else if (boton.name == "estructuras")
                cb.normalColor = aceptBt4;
            else
                Debug.Log("error botones Panel BGIIES");
            boton.colors = cb;
            boton.enabled = false;
            boton.enabled = true;
        }

        public void NegativeAllButtons()
        {
            NegativeCatButton(bt1);
            NegativeCatButton(bt2);
            NegativeCatButton(bt3);
            NegativeCatButton(bt4);
        }

        public void ActiveDesactiveButtons(Button[] buttons, bool isActive)
        {
            foreach (var button in buttons)
                button.gameObject.SetActive(isActive);
        }

    }
}