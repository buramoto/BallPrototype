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
    [SerializeField] public string URL;

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
    private float _timeForCheckpoint1;
    private float _timeForCheckpoint2;
    private float _timeForCheckpoint3;
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
        GameObject[] element = GameObject.FindGameObjectsWithTag(elementType);
        foreach (GameObject e in element)
        {
            if(elementType == "Plank")
            {
                Plank script = e.GetComponent<Plank>();
                if(script!=null && script.editable)
                {
                    // add spring position to string
                    Vector3 position = e.transform.position;
                    elementCoordinate += System.Math.Round(position.x,3) + "," + System.Math.Round(position.y,3) + ";";
                }   
            }
            else if(elementType == "Spring")
            {
                Spring script = e.GetComponent<Spring>();
                if(script!=null && script.editable)
                {
                    // add spring position to string
                    Vector3 position = e.transform.position;
                    elementCoordinate += System.Math.Round(position.x,3) + "," + System.Math.Round(position.y,3) + ";";
                }   
            }
            else if(elementType == "TempChange")
            {
                ChangeTemperature script = e.GetComponent<ChangeTemperature>();
                if(script!=null && script.editable)
                {
                    // add heater position to string
                    Vector3 position = e.transform.position;
                    elementCoordinate += System.Math.Round(position.x,3) + "," + System.Math.Round(position.y,3) + ";";
                }   
            }
        }
        return elementCoordinate;
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


        _timeForCheckpoint1 = DungeonMaster.timeArray[0];
        _timeForCheckpoint2 = DungeonMaster.timeArray[1];
        _timeForCheckpoint3 =DungeonMaster.timeArray[2];
        _timeForGoalCheckpoint = DungeonMaster.timeArray[3];
        _kbeCounterValue= GlobalVariables.kbeCounter;
        _oobCounterValue= GlobalVariables.oobCounter;
        _wgoCounterValue= GlobalVariables.wgoCounter;
        _livesLeft = GlobalVariables.livesLeft;

        //coordinate for props
        plankPositions = getCoordinates("Plank");
        Debug.Log("Plank Positions: "+plankPositions);
        springPositions = getCoordinates("Spring");
        Debug.Log("Spring Positions: "+springPositions);
        heaterPositions = getCoordinates("TempChange");
        Debug.Log("Heater Positions: "+heaterPositions);
        
        //From Analytics kept as is
        Debug.Log("kbe Counter");
        Debug.Log(_kbeCounterValue);
        Debug.Log("oob Counter");
        Debug.Log(_oobCounterValue);
        Debug.Log("wgo Counter");
        Debug.Log(_wgoCounterValue);
        resetButtonCounters += _kbeCounterValue.ToString() + "," + _oobCounterValue.ToString() + "," + _wgoCounterValue.ToString();
        StartCoroutine(Post(_sessionID.ToString(), 
                            _userID.ToString(),
                            _numberOfAttempts.ToString(), 
                            _numberOfPlanks.ToString(), 
                            _numberOfSprings.ToString(), 
                            _numberOfHeaters.ToString(), 
                            _timeForCheckpoint1.ToString(),
                            _timeForCheckpoint2.ToString(),
                            _timeForCheckpoint3.ToString(),
                            _timeForGoalCheckpoint.ToString(),
                            plankPositions,
                            springPositions,
                            heaterPositions,
                            resetButtonCounters,
                            _livesLeft.ToString(),
                            _numberOfElementsUsed.ToString()
                            ));
    }

    // Send data to Google Form
    private IEnumerator Post(string sessionID, string userID, string numberOfAttempts, string numberOfPlanks, string numberOfSprings, string numberOfHeaters, string timeForCheckpoint1,string timeForCheckpoint2,string timeForCheckpoint3, string timeForGoalCheckpoint, string plankPositions, string springPositions, string heaterPositions, string resetButtonCounters, string livesLeft, string numberOfElementsUsed)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.1046962840", sessionID);
        form.AddField("entry.1306942327", userID);
        form.AddField("entry.1054292080", numberOfAttempts);
        form.AddField("entry.141035311", numberOfPlanks);
        form.AddField("entry.163654761", numberOfSprings);
        form.AddField("entry.1061744410", numberOfHeaters);
        form.AddField("entry.547195651", timeForCheckpoint1);
        form.AddField("entry.2126259718", timeForCheckpoint2);
        form.AddField("entry.1981682484", timeForCheckpoint3);
        form.AddField("entry.328997273", timeForGoalCheckpoint);
        form.AddField("entry.209397380", plankPositions);
        form.AddField("entry.2035421848", springPositions);
        form.AddField("entry.1072592397", heaterPositions);
        form.AddField("entry.1925814045", resetButtonCounters);
        form.AddField("entry.2022844722", livesLeft);
        form.AddField("entry.1499039969", numberOfElementsUsed);
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
