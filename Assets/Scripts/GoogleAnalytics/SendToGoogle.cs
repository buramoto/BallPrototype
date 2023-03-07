using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
//using System.Math;

public class SendToGoogle : MonoBehaviour
{   
    // Create a static reference to this object so that it can be accessed from other scripts
    public static SendToGoogle sendToGoogle;

    // URL of the Google Form
    [SerializeField] private string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSd-me1BBMWKkP_7_P9WYRii1JI-7gXpsiKg7F6Vu7s5JPQojg/formResponse";

    // Data to be sent to Google Form
    private string _sessionID;
    private string _userID;
    private int _numberOfAttempts;
    private int _numberOfPlanks;
    private int _numberOfSprings;
    private int _numberOfHeaters;
    private int _numberOfPlanksUsed;
    private int _numberOfSpringsUsed;
    private int _numberOfHeatersUsed;
    private string _numberOfElementsUsed;
    // private float _timeForCheckpoint1;
    // private float _timeForCheckpoint2;
    // private float _timeForCheckpoint3;
    private float _timeForGoalCheckpoint;
    public GameObject[] plank;
    public GameObject[] spring;
    public GameObject[] heater;
    private string plankPositions;
    private string springPositions;
    private string heaterPositions;
    private int _kbeCounterValue;
    private int _oobCounterValue;
    private int _wgoCounterValue;
    private string resetButtonCounters;
    private int _livesLeft;
    private int _score;
    private int _starsCount;

    private string _level;

    // Get the user ID
    public static string GetUserID()
    {
        string uniqueID = SystemInfo.deviceName + SystemInfo.deviceModel;
        byte[] data = System.Text.Encoding.UTF8.GetBytes(uniqueID);
        byte[] hash = new SHA256Managed().ComputeHash(data);

        return System.Convert.ToBase64String(hash);
    }

    // Generate a unique session ID
    public static string GetSessionID()
    {
        string sessionID = System.Guid.NewGuid().ToString();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(sessionID);
        byte[] hash = new SHA256Managed().ComputeHash(data);

        return System.Convert.ToBase64String(hash);
    }

    public static string getCoordinates(string elementType)
    {
        string elementCoordinate = "";
        Debug.Log("getCoordinates"+ elementType);
        if(elementType == "TempChange"){
            foreach (GameObject obj in GlobalVariables.usedHeaterObjects)
            {
                ChangeTemperature script = obj.GetComponent<ChangeTemperature>();
                if(script!=null && script.editable)
                {
                    // add spring position to string
                    Vector3 position = obj.transform.position;
                    elementCoordinate += System.Math.Round(position.x,3) + "," + System.Math.Round(position.y,3) + ";";
                }  
            }
        }
        else if(elementType == "Plank")
        {
            foreach (GameObject obj in GlobalVariables.usedPlankObjects)
            {
                Plank script = obj.GetComponent<Plank>();
                if(script!=null && script.editable)
                {
                    // add spring position to string
                    Vector3 position = obj.transform.position;
                    elementCoordinate += System.Math.Round(position.x,3) + "," + System.Math.Round(position.y,3) + ";";
                }  
            }
        }
        else if(elementType == "Spring")
        {
            foreach (GameObject obj in GlobalVariables.usedSpringObjects)
            {
                Spring script = obj.GetComponent<Spring>();
                if(script!=null && script.editable)
                {
                    // add spring position to string
                    Vector3 position = obj.transform.position;
                    elementCoordinate += System.Math.Round(position.x,3) + "," + System.Math.Round(position.y,3) + ";";
                }  
            }
        }
        // else
        // {
        //     GameObject[] element = GameObject.FindGameObjectsWithTag(elementType);
        //     Debug.Log("getCoordinates length"+ element.Length);
        //     foreach (GameObject e in element)
        //     {
        //         if(elementType == "Plank")
        //         {
        //             Plank script = e.GetComponent<Plank>();
        //             if(script!=null && script.editable)
        //             {
        //                 // add spring position to plank
        //                 Vector3 position = e.transform.position;
        //                 elementCoordinate += System.Math.Round(position.x,3) + "," + System.Math.Round(position.y,3) + ";";
        //             }   
        //         }
        //         else if(elementType == "Spring")
        //         {
        //             Spring script = e.GetComponent<Spring>();
        //             if(script!=null && script.editable)
        //             {
        //                 // add spring position to string
        //                 Vector3 position = e.transform.position;
        //                 elementCoordinate += System.Math.Round(position.x,3) + "," + System.Math.Round(position.y,3) + ";";
        //             }   
        //         }
        //     }
        // }
        return elementCoordinate;
    }

    public void resetGlobalVariables(string type){
        if(type == "New Level" || type == "Reset Button" || type == "Main Menu")
        {
            if(type == "New Level" || type == "Main Menu")
            {
                GlobalVariables.attemptCounter = 1;
            }
            else{
                GlobalVariables.attemptCounter += 1;
                DungeonMaster.timeTaken = 0;
            }
            GlobalVariables.plankCounter = 0;
            GlobalVariables.springCounter = 0;
            GlobalVariables.heaterCounter = 0; 
            GlobalVariables.kbeCounter = 0;
            GlobalVariables.oobCounter = 0;
            GlobalVariables.wgoCounter = 0; 
        }
        else if(type == "OOB"){
            // GlobalVariables.attemptCounter += 1;
            GlobalVariables.oobCounter += 1; 
        }
        else if(type == "KBE"){
            GlobalVariables.attemptCounter += 1;
            GlobalVariables.kbeCounter += 1;
        }
        else if(type == "WGO"){
            GlobalVariables.attemptCounter += 1;
            GlobalVariables.wgoCounter += 1;
        }
        // else if(type == "Reset Button"){
        //     GlobalVariables.attemptCounter += 1;
        //     GlobalVariables.plankCounter = 0;
        //     GlobalVariables.springCounter = 0;
        //     GlobalVariables.heaterCounter = 0; 
        // }
        
        GameObject[] plank = GameObject.FindGameObjectsWithTag("Plank");
        foreach (GameObject p in plank)
        {
            Plank script = p.GetComponent<Plank>();
            if(script!=null && script.editable){
                script.hasCollided = false;
            }   
        }
        GameObject[] spring = GameObject.FindGameObjectsWithTag("Spring");
        foreach (GameObject s in spring)
        {
            Spring script = s.GetComponent<Spring>();
            if(script!=null && script.editable){
                script.hasCollided = false;
            }   
        }
        GameObject[] heater = GameObject.FindGameObjectsWithTag("TempChange");
        foreach (GameObject h in heater)
        {
            ChangeTemperature script = h.GetComponent<ChangeTemperature>();
            if(script!=null && script.editable){
                script.hasCollided = false;
            }   
        }
        foreach (GameObject obj in GlobalVariables.usedHeaterObjects)
        {
            obj.SetActive(true);
        }
        GlobalVariables.plankUsed = 0;
        GlobalVariables.springUsed = 0;
        GlobalVariables.heaterUsed = 0;
        // GlobalVariables.heaterCoordinates = "";
        GlobalVariables.livesLeft = DungeonMaster.dm.lives; 
        GlobalVariables.usedHeaterObjects.Clear();
        GlobalVariables.usedPlankObjects.Clear();
        GlobalVariables.usedSpringObjects.Clear();
        GlobalVariables.starsCounter = 0;
    }


    private void Awake()
    {
            sendToGoogle = this;
    }


    public void Send()
    {
        _sessionID = GetSessionID();
        _userID = GetUserID();
        _numberOfAttempts = GlobalVariables.attemptCounter;

        _numberOfPlanks = GlobalVariables.plankCounter;
        _numberOfSprings = GlobalVariables.springCounter;
        _numberOfHeaters = GlobalVariables.heaterCounter;
        _numberOfPlanksUsed = GlobalVariables.plankUsed;
        _numberOfSpringsUsed = GlobalVariables.springUsed;
        _numberOfHeatersUsed = GlobalVariables.heaterUsed;
        _numberOfElementsUsed = _numberOfPlanksUsed.ToString() + "," + _numberOfSpringsUsed.ToString() + "," + _numberOfHeatersUsed.ToString();


        // _timeForCheckpoint1 = DungeonMaster.timeArray[0];
        // _timeForCheckpoint2 = DungeonMaster.timeArray[1];
        // _timeForCheckpoint3 =DungeonMaster.timeArray[2];
        _timeForGoalCheckpoint = DungeonMaster.timeTaken;
        _kbeCounterValue= GlobalVariables.kbeCounter;
        _oobCounterValue= GlobalVariables.oobCounter;
        _wgoCounterValue= GlobalVariables.wgoCounter;
        _livesLeft = GlobalVariables.livesLeft;

        _level = GlobalVariables.sceneName;
        _score = GlobalVariables.levelScore;
        _starsCount = GlobalVariables.starsCounter;

        //coordinate for props
        plankPositions = getCoordinates("Plank");
        Debug.Log("Plank Positions: "+plankPositions);
        springPositions = getCoordinates("Spring");
        Debug.Log("Spring Positions: "+springPositions);
        //heaterPositions = getCoordinates("TempChange");
        heaterPositions = getCoordinates("TempChange");
        Debug.Log("Heater Positions: "+ heaterPositions);
        
        //From Analytics kept as is
        Debug.Log("kbe Counter");
        Debug.Log(_kbeCounterValue);
        Debug.Log("oob Counter");
        Debug.Log(_oobCounterValue);
        Debug.Log("wgo Counter");
        Debug.Log(_wgoCounterValue);

        Debug.Log("Scene Name: " + _level);
        
        resetButtonCounters += _kbeCounterValue.ToString() + "," + _oobCounterValue.ToString() + "," + _wgoCounterValue.ToString();
        StartCoroutine(Post(_sessionID.ToString(), 
                            _userID.ToString(),
                            _numberOfAttempts.ToString(), 
                            _numberOfPlanks.ToString(), 
                            _numberOfSprings.ToString(), 
                            _numberOfHeaters.ToString(), 
                            _timeForGoalCheckpoint.ToString(),
                            plankPositions,
                            springPositions,
                            heaterPositions,
                            resetButtonCounters,
                            _livesLeft.ToString(),
                            _numberOfElementsUsed.ToString(),
                            _level.ToString(),
                            _score.ToString(),
                            _starsCount.ToString()
                            ));
    }

    // Send data to Google Form
    private IEnumerator Post(string sessionID, string userID, string numberOfAttempts, string numberOfPlanks, string numberOfSprings, string numberOfHeaters, string timeForGoalCheckpoint, string plankPositions, string springPositions, string heaterPositions, string resetButtonCounters, string livesLeft, string numberOfElementsUsed, string Level, string score, string starsCount)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();

        form.AddField("entry.1046962840", sessionID);
        form.AddField("entry.1306942327", userID);
        form.AddField("entry.963340139", Level);
        form.AddField("entry.1054292080", numberOfAttempts);
        form.AddField("entry.141035311", numberOfPlanks);
        form.AddField("entry.163654761", numberOfSprings);
        form.AddField("entry.1061744410", numberOfHeaters);
        form.AddField("entry.328997273", timeForGoalCheckpoint);
        form.AddField("entry.209397380", plankPositions);
        form.AddField("entry.2035421848", springPositions);
        form.AddField("entry.1072592397", heaterPositions);
        form.AddField("entry.1925814045", resetButtonCounters);
        form.AddField("entry.2022844722", livesLeft);
        form.AddField("entry.1499039969", numberOfElementsUsed);
        form.AddField("entry.2123119901", score);
        form.AddField("entry.203253810", starsCount);
        // Send responses and verify result
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
