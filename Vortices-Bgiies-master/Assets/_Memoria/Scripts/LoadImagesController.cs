using System;
using System.Collections;
using System.Linq;
using Gamelogic;
using Memoria.Core;
using UnityCallbacks;
using UnityEngine;
using System.Collections.Generic;

namespace Memoria
{
	public enum FileType
	{
		Jpg,
		Png
	}

	public class LoadImagesController : GLMonoBehaviour, IOnValidate
	{
		public int images = 1;

		private LoadImageBehaviour _loadImageBehaviour;
        //DELETE THIS
		private DIOManager _dioManager;

		public LoadImageBehaviour LoadImageBehaviour { get { return _loadImageBehaviour; } }

		public int ImagesLoaded { get { return _imagesLoaded; } }
		private int _imagesLoaded;

        //DELETE THIS old method tied to DIOManager
		public void Initialize(DIOManager dioManager)
		{
			_dioManager = dioManager;

			_loadImageBehaviour = GetComponent<LoadImageBehaviour>();
			_loadImageBehaviour.Initialize(this);
		}

        public void Initialize()
        {
            _loadImageBehaviour = GetComponent<LoadImageBehaviour>();
            _loadImageBehaviour.Initialize(this);
        }

        public IEnumerator LoadFolderImages(List<DIOController> listOfDio)
		{
            var tuple = _loadImageBehaviour.TextureFormatGetter();

			string fileSuffix = tuple.First;
			TextureFormat textureFormat = tuple.Second;

			string indexSuffix;
			string fullFilename;
			WWW www;
			Texture2D texTmp;
			PitchGrabObject pitchGrabObject;

			while (true)
			{
				if (_imagesLoaded == images)
					break;
                //DELETE THIS
                if(!GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "BGIIESMode"))
				    indexSuffix = _loadImageBehaviour.FormattedIndex();
                else
                {
                    if(_imagesLoaded.ToString().Length == 1)
                        indexSuffix = "00" + _imagesLoaded.ToString();
                    else if(_imagesLoaded.ToString().Length == 2)
                        indexSuffix = "0" + _imagesLoaded.ToString();
                    else
                        indexSuffix = _imagesLoaded.ToString();
                }
				if (!_loadImageBehaviour.ValidationOfIndex(indexSuffix))
					continue;

				fullFilename = _loadImageBehaviour.pathPrefix +
					_loadImageBehaviour.pathImageAssets +
					_loadImageBehaviour.filename + indexSuffix + fileSuffix;

				www = new WWW(fullFilename);
				texTmp = new Texture2D(1024, 1024, textureFormat, false);
				www.LoadImageIntoTexture(texTmp);

				pitchGrabObject = listOfDio[_imagesLoaded].pitchGrabObject;
				pitchGrabObject.InitializeMaterial(texTmp);
				pitchGrabObject.SetId(_loadImageBehaviour.filename
					+ indexSuffix + fileSuffix);

				_imagesLoaded++;

				yield return new WaitForEndOfFrame();
			}

			_loadImageBehaviour.EndAction();
		}

		public void OnValidate()
		{
			images = Mathf.Max(1, images);
		}
	}

	public abstract class LoadImageBehaviour : GLMonoBehaviour
	{
		public string pathPrefix = @"file://";
		public string pathImageAssets = @"D:\";
		public string pathSmall = @"";
		public string filename = @"photo";

		//protected DIOManager dioManager;
		protected LoadImagesController loadImagesController;

		public abstract string FormattedIndex();
		public abstract bool ValidationOfIndex(string index);
		public abstract Tuple<string, TextureFormat> TextureFormatGetter();
		public abstract void EndAction();

        //DELETE THIS and find references to this instance of the DIOManager
        public virtual void Initialize( LoadImagesController loadImagesController)
        {
            //dioManager = fatherDioManager;
            this.loadImagesController = loadImagesController;
        }
    }
}