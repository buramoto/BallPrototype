using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;

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
    private float _timeForCheckpoint1;
    private float _timeForCheckpoint2;
    private float _timeForCheckpoint3;
    private float _timeForGoalCheckpoint;
    public GameObject[] plank;
    public GameObject[] spring;
    public GameObject[] heater;

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
    // Create a static reference to this object so that it can be accessed from other scripts
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
        _timeForCheckpoint1 = DungeonMaster.timeArray[0];
        _timeForCheckpoint2 = DungeonMaster.timeArray[1];
        _timeForCheckpoint3 = DungeonMaster.timeArray[2];
        _timeForGoalCheckpoint = DungeonMaster.timeArray[3];
        //coordinate for props
        plank = GameObject.FindGameObjectsWithTag("Plank");
        foreach (GameObject p in plank)
        {
            Plank script = p.GetComponent<Plank>();
            if(script!=null && script.editable){
                Debug.Log(p.transform.position);
            }   
        }
        spring = GameObject.FindGameObjectsWithTag("Spring");
        foreach (GameObject s in spring)
        {
            Spring script = s.GetComponent<Spring>();
            if(script!=null && script.editable){
                Debug.Log(s.transform.position);
            }   
        }
        heater = GameObject.FindGameObjectsWithTag("TempChange");
        foreach (GameObject h in heater)
        {
            ChangeTemperature script = h.GetComponent<ChangeTemperature>();
            if(script!=null && script.editable){
                Debug.Log(h.transform.position);
            }   
        }
        StartCoroutine(Post(_sessionID.ToString(), _userID.ToString(), _numberOfAttempts.ToString(), _numberOfPlanks.ToString(), _numberOfSprings.ToString(), _numberOfHeaters.ToString(),_timeForCheckpoint1.ToString(),_timeForCheckpoint2.ToString(),_timeForCheckpoint3.ToString(),_timeForGoalCheckpoint.ToString()));
    }

    // Send data to Google Form
    private IEnumerator Post(string sessionID, string userID, string numberOfAttempts, string numberOfPlanks, string numberOfSprings, string numberOfHeaters, string timeForCheckpoint1,string timeForCheckpoint2,string timeForCheckpoint3,string timeForGoalCheckpoint)
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
