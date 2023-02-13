using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{   
    public static SendToGoogle sendToGoogle;
    [SerializeField] public string URL;
    private long _sessionID;
    private string _userID;
    private int _numberOfAttempts;
    private int _numberOfPlanks;
    private int _numberOfSprings;
    private int _numberOfHeaters;

    // Create a static reference to this object so that it can be accessed from other scripts
    private void Awake()
    {
        if(sendToGoogle == null)
        {
            DontDestroyOnLoad(gameObject);
            sendToGoogle = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Send()
    {
        _sessionID = System.DateTime.Now.Ticks;
        _userID = SystemInfo.processorCount.ToString() + SystemInfo.deviceUniqueIdentifier;
        // Debug.Log("SendToGoogle.Send() called");
        // Debug.Log(_userID==null);
        _numberOfAttempts = GlobalVariables.attemptCounter;
        _numberOfPlanks = GlobalVariables.plankCounter;
        _numberOfSprings = GlobalVariables.springCounter;
        _numberOfHeaters = GlobalVariables.heaterCounter;
        // Debug.Log(_sessionID+" "+_userID+" "+_numberOfAttempts+" "+_numberOfPlanks+" "+_numberOfSprings+" "+_numberOfHeaters);
        StartCoroutine(Post(_sessionID.ToString(), _userID.ToString(), _numberOfAttempts.ToString(), _numberOfPlanks.ToString(), _numberOfSprings.ToString(), _numberOfHeaters.ToString()));
    }

    // Send data to Google Form
    private IEnumerator Post(string sessionID, string userID, string numberOfAttempts, string numberOfPlanks, string numberOfSprings, string numberOfHeaters)
    {
        // Create the form and enter responses
        // Debug.Log("1");
        WWWForm form = new WWWForm();
        form.AddField("entry.1046962840", sessionID);
        form.AddField("entry.1306942327", userID);
        form.AddField("entry.1054292080", numberOfAttempts);
        form.AddField("entry.141035311", numberOfPlanks);
        form.AddField("entry.163654761", numberOfSprings);
        form.AddField("entry.1061744410", numberOfHeaters);
        // Debug.Log("2");
        // Send responses and verify result
        // url = https://docs.google.com/forms/u/1/d/e/1FAIpQLSd-me1BBMWKkP_7_P9WYRii1JI-7gXpsiKg7F6Vu7s5JPQojg/formResponse

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            // Debug.Log("3");
            yield return www.SendWebRequest();
            // Debug.Log("4");
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
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
