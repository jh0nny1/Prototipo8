using UnityEngine;
using UnityCallbacks;
using Memoria;
using Gamelogic;

public class NeuroSkyData : MonoBehaviour, IAwake
{
    public static NeuroSkyData Instance { set; get; }
    public CsvCreator csvCreator;
    public string dataLogPath;
    public Texture2D[] signalIcons;    
    public GameObject TCGConnectionPrefab;
    private string Scope = "Vortices2Config";

    public bool useGUI = true;

    public bool usePoorSignal = true;
    public bool useAttention = true;
    public bool useMeditation = true;
    public bool useRawData = true;
    public bool useBlink = true;
    public bool useDelta = true;
    public bool useTheta = true;
    public bool useLowAlpha = true;
    public bool useHighAlpha = true;
    public bool useLowBeta = true;
    public bool useHighBeta = true;
    public bool useLowGamma = true;
    public bool useHighGamma = true;

    private int indexSignalIcons = 1;   
	
    TGCConnectionController controller;

    private int poorSignal;
    private int attention;
    private int meditation;
    private int rawData;
    private int blink;
    private int lastBlink;

    private float delta;
    private float theta;
    private float lowAlpha;
    private float highAlpha;

    private float lowBeta;
    private float highBeta;
    private float lowGamma;
    private float highGamma;

    public void Awake()
    {
        Instance = this;
    }

    public void StartNeuroSkyData()
    {
        transform.SetParent(Camera.main.transform);
        Scope = ProfileManager.Instance.currentEvaluationScope;
        GameObject prefab = Instantiate(TCGConnectionPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        controller = prefab.GetComponent<TGCConnectionController>();
		
        if(usePoorSignal)
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;

        if(useAttention)
		controller.UpdateAttentionEvent += OnUpdateAttention;

        if(useMeditation)
		controller.UpdateMeditationEvent += OnUpdateMeditation;

        if(useBlink)
        controller.UpdateBlinkEvent += OnUpdateBlink;


        if(useDelta)
		controller.UpdateDeltaEvent += OnUpdateDelta;

        if(useTheta)
        controller.UpdateThetaEvent += OnUpdateTheta;

        if(useLowAlpha)
        controller.UpdateLowAlphaEvent += OnUpdateLowAlpha;

        if(useHighAlpha)
        controller.UpdateHighAlphaEvent += OnUpdateHighAlpha;


        if(useLowBeta)
        controller.UpdateLowBetaEvent += OnUpdateLowBeta;

        if(useHighBeta)
        controller.UpdateHighBetaEvent += OnUpdateHighBeta;

        if(useLowGamma)
        controller.UpdateLowGammaEvent += OnUpdateLowGamma;

        if(useHighGamma)
        controller.UpdateHighGammaEvent += OnUpdateHighGamma;

        /*
         * Initializes the CvsCreator to store data in a log
         */
        dataLogPath = GLPlayerPrefs.GetString(Scope, "NeuroSkyMindwaveDataPath");
        Debug.Log("neurosky path: " + GLPlayerPrefs.GetString(Scope, "NeuroSkyMindwaveDataPath"));
        if (dataLogPath.Equals(""))
        {
            csvCreator = new CsvCreator("NeuroSkyMindwaveDataLog\\data.csv");
        }
        else
        {
            csvCreator = new CsvCreator(dataLogPath);
        }

    }

    public void ResetBlink()
    {
        if (blink == lastBlink)
            blink = 0;
        else
        {
            lastBlink = blink;
        }
    }
	
	void OnUpdatePoorSignal(int value){
		poorSignal = value;
		if(value < 25){
      		indexSignalIcons = 0;
		}else if(value >= 25 && value < 51){
      		indexSignalIcons = 4;
		}else if(value >= 51 && value < 78){
      		indexSignalIcons = 3;
		}else if(value >= 78 && value < 107){
      		indexSignalIcons = 2;
		}else if(value >= 107){
      		indexSignalIcons = 1;
		}
        csvCreator.AddLines("Signal value: " + value.ToString(), "");
	}
	void OnUpdateAttention(int value){
		attention = value;
        csvCreator.AddLines("Attention value: " + value.ToString(), "");
    }
	void OnUpdateMeditation(int value){
		meditation = value;
        csvCreator.AddLines("Meditation value: " + value.ToString(), "");
    }
    void OnUpdateRawData(int value)
    {
        rawData = value;
        csvCreator.AddLines("Raw data value: " + value.ToString(), "");
    }
    void OnUpdateBlink(int value)
    {
        blink = value;
        csvCreator.AddLines("Blink value: " + value.ToString(), "");
    }

    void OnUpdateDelta(float value)
    {
        delta = value;
        csvCreator.AddLines("Delta value: " + value.ToString(), "");
    }
    void OnUpdateTheta(float value)
    {
        theta = value;
        csvCreator.AddLines("Theta value: " + value.ToString(), "");
    }
    void OnUpdateLowAlpha(float value)
    {
        lowAlpha = value;
        csvCreator.AddLines("Low Alpha value: " + value.ToString(), "");
    }
    void OnUpdateHighAlpha(float value)
    {
        highAlpha = value;
        csvCreator.AddLines("High Alpha value: " + value.ToString(), "");
    }

    void OnUpdateLowBeta(float value)
    {
        lowBeta = value;
        csvCreator.AddLines("Low Beta value: " + value.ToString(), "");
    }
    void OnUpdateHighBeta(float value)
    {
        highBeta = value;
        csvCreator.AddLines("High Beta value: " + value.ToString(), "");
    }
    void OnUpdateLowGamma(float value)
    {
        lowGamma = value;
        csvCreator.AddLines("Low Gamma value: " + value.ToString(), "");
    }
    void OnUpdateHighGamma(float value)
    {
        highGamma = value;
        csvCreator.AddLines("High Gamma value: " + value.ToString(), "");
    }



    public int getAttention()
    {
        return attention;
    }
    public int getMeditation()
    {
        return meditation;
    }
    public int getRawData()
    {
        return rawData;
    }
    public int getBlink()
    {
        return blink;
    }

    public float getDelta()
    {
        return delta;
    }
    public float getTheta()
    {
        return theta;
    }
    public float getLowAlpha()
    {
        return lowAlpha;
    }
    public float getHighAlpha()
    {
        return highAlpha;
    }

    public float getLowBeta()
    {
        return lowBeta;
    }
    public float getHighBeta()
    {
        return highBeta;
    }
    public float getLowGamma()
    {
        return lowGamma;
    }
    public float getHighGamma()
    {
        return highGamma;
    }


    void OnGUI()
    {
        if (!EEGManager.Instance.useNeuroSky || !ActionManager.Instance.initialized)
            return;

        if (useGUI)
        {

            GUILayout.BeginHorizontal();            

            if (GUILayout.Button("Connect"))
            {
                controller.Connect();
            }
            if (GUILayout.Button("DisConnect"))
            {
                controller.Disconnect();
                indexSignalIcons = 1;
            }

            if (usePoorSignal)
            {
                GUILayout.Space(Screen.width - 250);
                GUILayout.Label(signalIcons[indexSignalIcons]);
            }            

            GUILayout.EndHorizontal();

            if(usePoorSignal)
            GUILayout.Label("PoorSignal:" + poorSignal);

            if(useAttention)
            GUILayout.Label("Attention:" + attention);

            if(useMeditation)
            GUILayout.Label("Meditation:" + meditation);            

            if(useBlink)
            GUILayout.Label("Blink:" + blink);

            if (useDelta)
            GUILayout.Label("Delta:" + delta);

            if (useTheta)
            GUILayout.Label("Theta:" + theta);

            if (useLowAlpha)
            GUILayout.Label("Low Alpha:" + lowAlpha);

            if (useHighAlpha)
            GUILayout.Label("High Alpha:" + highAlpha);

            if (useLowBeta)
            GUILayout.Label("Low Beta:" + lowBeta);

            if (useHighBeta)
            GUILayout.Label("High Beta:" + highBeta);

            if (useLowGamma)
            GUILayout.Label("Low Gamma:" + lowGamma);

            if (useHighGamma)
            GUILayout.Label("High Gamma:" + highGamma);

        }	

    }
}
