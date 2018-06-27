using System;
using System.Collections;
using Gamelogic;
using Memoria.Core;
using UnityCallbacks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

namespace Memoria
{
    public class LookPointerBGIIES : LookPointer, IAwake, IUpdate
    {
        public bool zoomActive = false;

        public List<DIOController> listaImagenes = null;

        public List<PitchGrabObject> listaCat1 = null;
        public List<PitchGrabObject> listaCat2 = null;
        public List<PitchGrabObject> listaCat3 = null;
        public List<PitchGrabObject> listaCat4 = null;

        public List<PitchGrabObject> actualListaCat = null;
        public List<Vector3> listaPos = null;

        public int indexPhoto = 0;

        float closeRange = 18f;

        bool eliminarFromCategoria = false;
        public void Awake()
        {

            actualPitchGrabObject = null;
            posibleActualPitchGrabObject = null;

            zoomingOut = false;
            zoomingIn = false;

            listaCat1 = new List<PitchGrabObject>();
            listaCat2 = new List<PitchGrabObject>();
            listaCat3 = new List<PitchGrabObject>();
            listaCat4 = new List<PitchGrabObject>();

            listaPos = new List<Vector3>();

        }
        public void Update()
        {
            /* Code used only to zoom out of image, which should not exist given the ActionManager architecture
            //this update triggers the zoom out if the right click is pressed or if the kinect gesture zoom out has been triggered.
            if ((Input.GetMouseButtonDown(1) || dioManager.kinectGestures.kinectGestureZoomOut()) && actualPitchGrabObject != null && !dioManager.movingPlane)
            {
                if (!zoomingOut && !zoomingIn)
                {
                    if (zoomActive)
                    {
                        StartCoroutine(ZoomingOut(null));
                        //dioManager.panelBgiies.noInteractableButtons();
                        zoomActive = false;
                    }
                }

            }
            */
        }

        public override void LookPointerStay(PitchGrabObject pitchGrabObject)
        {
            posibleActualPitchGrabObject = pitchGrabObject;
            /* Code inside the lookPointer loop [in LookPointerBGIIES], triggered by the OnDetect function in pitchGrabObject, which is triggered by the lookPointerRaycasting if a new object is detected.
             * 
            if ((Input.GetMouseButtonDown(0) || dioManager.kinectGestures.kinectGestureZoomIn()) && actualPitchGrabObject == null && !dioManager.movingPlane)
            {
                if (!zoomingIn && !zoomingOut)
                {
                    if (!zoomActive)
                    {
                        StartCoroutine(ZoomingIn(pitchGrabObject, null));
                        dioManager.panelBgiies.interactableButtons(posibleActualPitchGrabObject);
                        zoomActive = true;
                    }
                }
            }
            */
        }

        public override void SetZoomInInitialStatus(PitchGrabObject pitchGrabObject)
        {
            actualPitchGrabObject = pitchGrabObject;
            actualPitchGrabObject.dioController.inVisualizationPosition = false;
            _actualPitchObjectOriginalPosition = pitchGrabObject.transform.position;
            _actualPitchObjectOriginalRotation = pitchGrabObject.transform.rotation;
            _actualPitchObjectOriginalScale = pitchGrabObject.transform.localScale;
        }
        public override void DirectZoomInCall(Action finalAction)
        {
            if (!zoomingIn && !zoomingOut && actualPitchGrabObject == null && !VisualizationManager.Instance.planeVisualization.movingPlane)
                StartCoroutine(ZoomingIn(posibleActualPitchGrabObject, finalAction));
        }

        public override void DirectZoomInCall(PitchGrabObject pitchGrabObject, Action finalAction)
        {
            if (!zoomingIn && !zoomingOut && actualPitchGrabObject == null && !VisualizationManager.Instance.planeVisualization.movingPlane)
            {
                StartCoroutine(ZoomingIn(pitchGrabObject, finalAction));
            }
        }

        public override IEnumerator ZoomingIn(PitchGrabObject pitchGrabObject, Action finalAction)
        {
                zoomingIn = true;
                SetZoomInInitialStatus(pitchGrabObject);
                MOTIONSManager.Instance.AddLines("Zooming In", pitchGrabObject.idName);

                var counter = 0;
                while (true)
                {
                    pitchGrabObject.transform.position = Vector3.MoveTowards(pitchGrabObject.transform.position, Camera.main.transform.position, 0.01f);

                    if (counter >= closeRange)
                    {
                        break;
                    }

                    counter++;
                    yield return new WaitForFixedUpdate();
                }

                if (finalAction != null)
                    finalAction();
                zoomingIn = false;
            
        }
        public override void DirectZoomOutCall(Action finalAction)
        {
            if (!zoomingOut && !zoomingIn && actualPitchGrabObject != null && !VisualizationManager.Instance.planeVisualization.movingPlane)
            {
                StartCoroutine(ZoomingOut(finalAction));
            }
        }

        public override IEnumerator ZoomingOut(Action finalAction)
        {
            MOTIONSManager.Instance.AddLines("Zooming Out", actualPitchGrabObject.idName);
                zoomingOut = true;

                var positionTargetReached = false;
                var scaleTargetReaced = false;

                while (true)
                {
                    //Position
                    actualPitchGrabObject.transform.position =
                        Vector3.MoveTowards(actualPitchGrabObject.transform.position,
                            _actualPitchObjectOriginalPosition, _positionSteps);

                    if (actualPitchGrabObject.transform.position.EqualOrMayorCompareVector(_actualPitchObjectOriginalPosition, -0.0001f) && !positionTargetReached)
                    {
                        positionTargetReached = true;
                        actualPitchGrabObject.transform.position = _actualPitchObjectOriginalPosition;
                    }

                    //Scale
                    actualPitchGrabObject.transform.localScale =
                        Vector3.MoveTowards(actualPitchGrabObject.transform.localScale,
                            _actualPitchObjectOriginalScale, _scaleSteps);

                    if (actualPitchGrabObject.transform.localScale.EqualOrMinorCompareVector(_actualPitchObjectOriginalScale, 0.001f) && !scaleTargetReaced)
                    {
                        scaleTargetReaced = true;
                        actualPitchGrabObject.transform.localScale = _actualPitchObjectOriginalScale;
                    }

                    if (positionTargetReached && scaleTargetReaced)
                        break;

                    yield return new WaitForFixedUpdate();

                }

                actualPitchGrabObject.OnUnDetect();
                actualPitchGrabObject.dioController.inVisualizationPosition = true;
                actualPitchGrabObject = null;
                zoomingOut = false;
                if (finalAction != null)
                    finalAction();
            

        }
    
        public void SelectCat1()
        {
            bool unPitchedAccept = false;

            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;

                unPitchedAccept = true;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }

            actualPitchGrabObject.isSelectedCat1 = !actualPitchGrabObject.isSelectedCat1;

            if (actualPitchGrabObject.isSelectedCat1)
            {
                createClone(actualPitchGrabObject, listaCat1);
                
                createMarcador(VisualizationManager.Instance.planeVisualization.panelBgiies.aceptBt1, VisualizationManager.Instance.planeVisualization.panelBgiies.bt1);
                VisualizationManager.Instance.planeVisualization.panelBgiies.PositiveCatButton(VisualizationManager.Instance.planeVisualization.panelBgiies.bt1);
            }
            else
            {
                destroyClone(actualPitchGrabObject, listaCat1);

                deleteMarcador(VisualizationManager.Instance.planeVisualization.panelBgiies.aceptBt1, VisualizationManager.Instance.planeVisualization.panelBgiies.bt1);
                VisualizationManager.Instance.planeVisualization.panelBgiies.NegativeCatButton(VisualizationManager.Instance.planeVisualization.panelBgiies.bt1);
            }

            //dioManager.panelBgiies.interactableButtons(actualPitchGrabObject);

            var action = actualPitchGrabObject.isSelectedCat1 ? "Select " : "Deselect ";
            action = action + VisualizationManager.Instance.planeVisualization.panelBgiies.bt1.tag.ToString();
            MOTIONSManager.Instance.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }

        public void SelectCat2()
        {
            bool unPitchedAccept = false;

            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;

                unPitchedAccept = true;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }

            actualPitchGrabObject.isSelectedCat2 = !actualPitchGrabObject.isSelectedCat2;

            if (actualPitchGrabObject.isSelectedCat2)
            {
                createClone(actualPitchGrabObject, listaCat2);

                createMarcador(VisualizationManager.Instance.planeVisualization.panelBgiies.aceptBt2, VisualizationManager.Instance.planeVisualization.panelBgiies.bt2);
                VisualizationManager.Instance.planeVisualization.panelBgiies.PositiveCatButton(VisualizationManager.Instance.planeVisualization.panelBgiies.bt2);
            }
            else
            {
                destroyClone(actualPitchGrabObject, listaCat2);

                deleteMarcador(VisualizationManager.Instance.planeVisualization.panelBgiies.aceptBt2, VisualizationManager.Instance.planeVisualization.panelBgiies.bt2);
                VisualizationManager.Instance.planeVisualization.panelBgiies.NegativeCatButton(VisualizationManager.Instance.planeVisualization.panelBgiies.bt2);
            }

            //dioManager.panelBgiies.interactableButtons(actualPitchGrabObject);

            var action = actualPitchGrabObject.isSelectedCat2 ? "Select " : "Deselect ";
            action = action + VisualizationManager.Instance.planeVisualization.panelBgiies.bt2.tag.ToString();
            MOTIONSManager.Instance.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }
        public void SelectCat3()
        {
            bool unPitchedAccept = false;

            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;

                unPitchedAccept = true;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }
            
            actualPitchGrabObject.isSelectedCat3 = !actualPitchGrabObject.isSelectedCat3;

            if (actualPitchGrabObject.isSelectedCat3)
            {
                createClone(actualPitchGrabObject, listaCat3);

                createMarcador(VisualizationManager.Instance.planeVisualization.panelBgiies.aceptBt3, VisualizationManager.Instance.planeVisualization.panelBgiies.bt3);
                VisualizationManager.Instance.planeVisualization.panelBgiies.PositiveCatButton(VisualizationManager.Instance.planeVisualization.panelBgiies.bt3);
            }
            else
            {
                destroyClone(actualPitchGrabObject, listaCat3);

                deleteMarcador(VisualizationManager.Instance.planeVisualization.panelBgiies.aceptBt3, VisualizationManager.Instance.planeVisualization.panelBgiies.bt3);
                VisualizationManager.Instance.planeVisualization.panelBgiies.NegativeCatButton(VisualizationManager.Instance.planeVisualization.panelBgiies.bt3);
            }

            //dioManager.panelBgiies.interactableButtons(actualPitchGrabObject);

            var action = actualPitchGrabObject.isSelectedCat3 ? "Select " : "Deselect ";
            action = action + VisualizationManager.Instance.planeVisualization.panelBgiies.bt3.tag.ToString();
            MOTIONSManager.Instance.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }
        public void SelectCat4()
        {
            bool unPitchedAccept = false;

            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;

                unPitchedAccept = true;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }

            actualPitchGrabObject.isSelectedCat4 = !actualPitchGrabObject.isSelectedCat4;

            if (actualPitchGrabObject.isSelectedCat4)
            {
                createClone(actualPitchGrabObject, listaCat4);

                createMarcador(VisualizationManager.Instance.planeVisualization.panelBgiies.aceptBt4, VisualizationManager.Instance.planeVisualization.panelBgiies.bt4);
                VisualizationManager.Instance.planeVisualization.panelBgiies.PositiveCatButton(VisualizationManager.Instance.planeVisualization.panelBgiies.bt4);
            }
            else
            {
                destroyClone(actualPitchGrabObject, listaCat4);

                deleteMarcador(VisualizationManager.Instance.planeVisualization.panelBgiies.aceptBt4, VisualizationManager.Instance.planeVisualization.panelBgiies.bt4);
                VisualizationManager.Instance.planeVisualization.panelBgiies.NegativeCatButton(VisualizationManager.Instance.planeVisualization.panelBgiies.bt4);
            }

            //dioManager.panelBgiies.interactableButtons(actualPitchGrabObject);

            var action = actualPitchGrabObject.isSelectedCat4 ? "Select " : "Deselect ";
            action = action + VisualizationManager.Instance.planeVisualization.panelBgiies.bt4.tag.ToString();
            MOTIONSManager.Instance.AddLines(action, actualPitchGrabObject.idName);

            if (unPitchedAccept)
                actualPitchGrabObject = null;
        }

        public void createMarcador(Color color, Button boton)
        {
            GameObject obj = Instantiate(VisualizationManager.Instance.planeVisualization.childPrefab) as GameObject;
            obj.transform.parent = actualPitchGrabObject.transform;

            int i = actualPitchGrabObject.transform.childCount;

            actualPitchGrabObject.transform.GetChild(i - 1).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + ((i - 1) * 0.095f), actualPitchGrabObject.transform.position.y - 0.12f, actualPitchGrabObject.transform.position.z - 0.001f);
            obj.transform.GetComponent<Renderer>().material.color = color;

            string[] textBt = boton.GetComponentInChildren<Text>().text.ToString().Split(':');
            int contador = Int32.Parse(textBt[1].Trim()) + 1;
            boton.GetComponentInChildren<Text>().text = textBt[0] + ": " + contador.ToString();
        }

        public void createClone(PitchGrabObject imagen, List<PitchGrabObject> lista)
        {
            PitchGrabObject obj = Instantiate(actualPitchGrabObject);

            obj.transform.parent = actualPitchGrabObject.transform.parent;

            obj.transform.DestroyChildrenImmediate();
            

            obj.idName = imagen.idName;

            obj.transform.localPosition = _actualPitchObjectOriginalPosition;
            obj.transform.localScale = _actualPitchObjectOriginalScale;

            obj.transform.localScale =
                Vector3.MoveTowards(actualPitchGrabObject.transform.localScale,
                    _actualPitchObjectOriginalScale, _scaleSteps);


            if (obj.transform.localScale.EqualOrMinorCompareVector(_actualPitchObjectOriginalScale, 0.001f))
            {
                obj.transform.localScale = _actualPitchObjectOriginalScale;
            }

            obj.gameObject.SetActive(false);

            bool contenedor = false;
            foreach(var photo in lista)
            {
                if(photo.idName == imagen.idName)
                {
                    contenedor = true;
                }
            }

            if (!contenedor)
            {
                lista.Add(obj);
            }
            RevisarLista(lista);
        }
    
        public void destroyClone(PitchGrabObject imagen, List<PitchGrabObject> lista)
        {
            Debug.Log(imagen);
            PitchGrabObject obj = null;
            List<PitchGrabObject> listaAux = lista;
            int i = 0;
            foreach (PitchGrabObject photo in listaAux)
            {
                Debug.Log(photo);
                if (photo.idName == imagen.idName)
                {
                    obj = photo;                 
                    break;
                }
                i++;
            }
            lista.RemoveAt(i);
            if (obj != null)
                Destroy(obj.gameObject);

            RevisarLista(lista);
        }
        public void deleteMarcador(Color color, Button boton)
        {
            int child = actualPitchGrabObject.transform.childCount;
            int index = 0;
            for (int i = 0; i < child; i++)
            {
                if (actualPitchGrabObject.transform.GetChild(i).transform.GetComponent<Renderer>().material.color == color)
                {
                    Destroy(actualPitchGrabObject.transform.GetChild(i).gameObject);
                }
                else
                {
                    actualPitchGrabObject.transform.GetChild(i).transform.position = new Vector3(actualPitchGrabObject.transform.position.x - 0.142f + (index * 0.095f), actualPitchGrabObject.transform.position.y - 0.12f, actualPitchGrabObject.transform.position.z - 0.001f);
                    index += 1;
                }
            }

            string[] textBt = boton.GetComponentInChildren<Text>().text.ToString().Split(':');
            int contador = Int32.Parse(textBt[1].Trim()) - 1;
            boton.GetComponentInChildren<Text>().text = textBt[0] + ": " + contador.ToString();
        }

        public void MostrarCategoria(List<PitchGrabObject> lista , int index)
        {
            indexPhoto = index;
            actualListaCat = lista;

            zoomActive = false;
            InteractionManager.Instance.raycastingSpherePlane.ResetActualPitchGrabObject();
            
            if (lista.Count / (indexPhoto + 12f) > 1f)
                VisualizationManager.Instance.planeVisualization.panelBgiies.EnableButton(VisualizationManager.Instance.planeVisualization.panelBgiies.moveCameraInside3DButton, VisualizationManager.Instance.planeVisualization.panelBgiies.moveCameraInsideEventTrigger);
            else
                VisualizationManager.Instance.planeVisualization.panelBgiies.DisableButton(VisualizationManager.Instance.planeVisualization.panelBgiies.moveCameraInside3DButton, VisualizationManager.Instance.planeVisualization.panelBgiies.moveCameraInsideEventTrigger);

            if ((indexPhoto-12) >= 0)
                VisualizationManager.Instance.planeVisualization.panelBgiies.EnableButton(VisualizationManager.Instance.planeVisualization.panelBgiies.moveCameraOutside3DButton, VisualizationManager.Instance.planeVisualization.panelBgiies.moveCameraOutsideEventTrigger);
            else
                VisualizationManager.Instance.planeVisualization.panelBgiies.DisableButton(VisualizationManager.Instance.planeVisualization.panelBgiies.moveCameraOutside3DButton, VisualizationManager.Instance.planeVisualization.panelBgiies.moveCameraOutsideEventTrigger);

            //listaImagenes = dioManager.sphereControllers.Count > dioManager.planeControllers.Count ? dioManager.sphereControllers.SelectMany(sc => sc.dioControllerList).ToList() : dioManager.planeControllers.SelectMany(sc => sc.dioControllerList).ToList();
            listaImagenes = VisualizationManager.Instance.planeVisualization.planeControllers.SelectMany(sc => sc.dioControllerList).ToList();
            int head;
            if (VisualizationManager.Instance.planeVisualization.InLastVisualization)
            {
                head = (VisualizationManager.Instance.planeVisualization.actualVisualization - 1) * 12;
                VisualizationManager.Instance.planeVisualization.MovePlaneLastOutside(VisualizationManager.Instance.planeVisualization.panelBgiies.initialPlaneAction, VisualizationManager.Instance.planeVisualization.panelBgiies.finalPlaneAction);
            }
            else
            {
                head = VisualizationManager.Instance.planeVisualization.actualVisualization * 12;
            }
            int y = head;
            Debug.Log("indice de y " + y);
            for (int i = 0; i < lista.Count; i++)
            {
                if (y % 12 == 0)
                {
                    y = head;
                }
                Debug.Log("indice de y " + y + " indice de i" + i);
                if (lista[i] != null)
                {
                    lista[i].transform.position = listaImagenes[y].transform.position;

                    var grabableObjectMeshRender = lista[i].GetComponent<MeshRenderer>();
                    var grabableObjectColor = grabableObjectMeshRender.material.color;
                    grabableObjectColor.a = 0.66f;
                    grabableObjectMeshRender.material.color = grabableObjectColor;

                    if (i >= indexPhoto && i < indexPhoto + 12)
                    {
                        lista[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        lista[i].gameObject.SetActive(false);
                    }
                    y++;
                }
                Debug.Log("foto " + lista[i].idName + " estado " + lista[i].gameObject.activeSelf + " posicion " + lista[i].transform.position);
            }

            OcultarImagenes(VisualizationManager.Instance.planeVisualization.planeControllers.SelectMany(sc => sc.dioControllerList).ToList());
            if (!eliminarFromCategoria)
            {
                var action = "Ingresa a categoria ";
                MOTIONSManager.Instance.AddLines(action, VisualizationManager.Instance.planeVisualization.panelBgiies.nombreCategoria);
            }
            eliminarFromCategoria = false;
        }
        public void OcultarImagenes(List<DIOController> imageList)
        {
            //listaImagenes = dioManager.sphereControllers.Count > dioManager.planeControllers.Count ? dioManager.sphereControllers.SelectMany(sc => sc.dioControllerList).ToList() : dioManager.planeControllers.SelectMany(sc => sc.dioControllerList).ToList();
            listaImagenes = imageList;
            foreach (var photo in listaImagenes)
            {
                photo.pitchGrabObject.gameObject.SetActive(false);
            }
        }
        public void MostrarImagenes(List<PitchGrabObject> listaOculta, List<DIOController> imageList)
        {
            //listaImagenes = dioManager.sphereControllers.Count > dioManager.planeControllers.Count ? dioManager.sphereControllers.SelectMany(sc => sc.dioControllerList).ToList() : dioManager.planeControllers.SelectMany(sc => sc.dioControllerList).ToList();
            listaImagenes = imageList;
            foreach (var photo in listaImagenes)
            {
                photo.pitchGrabObject.gameObject.SetActive(true);
            }
            Debug.Log("Mostrar Imagenes");
            RevisarLista(listaOculta);

            foreach(var clone in listaOculta)
            {
                try
                {
                    clone.gameObject.SetActive(false);
                }
                catch { }
            }

            var action = "Sale de categoria ";
            MOTIONSManager.Instance.AddLines(action, dioManager.panelBgiies.nombreCategoria);
        }

        public void InsideCategoria(List<PitchGrabObject> lista)
        {
            indexPhoto = indexPhoto + 12;
            if (lista.Count / indexPhoto >= 1)
                VisualizationManager.Instance.planeVisualization.panelBgiies.EnableMoveCameraInside();
            else
                VisualizationManager.Instance.planeVisualization.panelBgiies.DisableMoveCameraInside();

            MostrarCategoria(lista, indexPhoto);
        }

        public void OutsideCategoria(List<PitchGrabObject> lista)
        {
            VisualizationManager.Instance.planeVisualization.panelBgiies.EnableMoveCameraOutside();
            indexPhoto = indexPhoto - 12;
            MostrarCategoria(lista, indexPhoto);
        }
        public void RevisarLista(List<PitchGrabObject> lista)
        {
            Debug.Log("Lista revisada" + lista.ToString());
            foreach(var e in lista)
            {
                Debug.Log(e.idName);
            }
        }

        public void DeseleccionarFromCategoria(List<PitchGrabObject> lista, int categoriaActual, Button boton, Color color, List<DIOController> imageList)
        {
            //listaImagenes = dioManager.sphereControllers.Count > dioManager.planeControllers.Count ? dioManager.sphereControllers.SelectMany(sc => sc.dioControllerList).ToList() : dioManager.planeControllers.SelectMany(sc => sc.dioControllerList).ToList();
            listaImagenes = imageList;
            if (actualPitchGrabObject == null)
            {
                if (posibleActualPitchGrabObject == null)
                    return;
                actualPitchGrabObject = posibleActualPitchGrabObject;
            }

            PitchGrabObject imagen = null;
            foreach(var photo in listaImagenes)
            {
                if(photo.pitchGrabObject.idName == actualPitchGrabObject.idName)
                {
                    imagen = photo.pitchGrabObject;
                    break;
                }
            }

            if ((int)Categorias.Categoria1 == categoriaActual)
                imagen.isSelectedCat1 = !imagen.isSelectedCat1;
            else if ((int)Categorias.Categoria2 == categoriaActual)
                imagen.isSelectedCat2 = !imagen.isSelectedCat2;
            else if ((int)Categorias.Categoria3 == categoriaActual)
                imagen.isSelectedCat3 = !imagen.isSelectedCat3;
            else if ((int)Categorias.Categoria4 == categoriaActual)
                imagen.isSelectedCat4 = !imagen.isSelectedCat4;
            else
                Debug.Log("Error al eliminar desde una categoria, categoria ingresada desde ButtonPanelBgiies inválida");

            var action = "Deselect " + VisualizationManager.Instance.planeVisualization.panelBgiies.nombreCategoria;
            MOTIONSManager.Instance.AddLines(action, imagen.idName);

            zoomActive = false;
            destroyClone(imagen, lista);
            actualPitchGrabObject = imagen;
            deleteMarcador(color, boton);
            actualPitchGrabObject = null;
            eliminarFromCategoria = true;
            MostrarCategoria(lista, 0);
        }

    }
}
