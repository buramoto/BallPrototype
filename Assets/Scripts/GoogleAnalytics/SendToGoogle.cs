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

        StartCoroutine(Post(_sessionID.ToString(), _userID.ToString(), _numberOfAttempts.ToString(), _numberOfPlanks.ToString(), _numberOfSprings.ToString(), _numberOfHeaters.ToString()));
    }

    // Send data to Google Form
    private IEnumerator Post(string sessionID, string userID, string numberOfAttempts, string numberOfPlanks, string numberOfSprings, string numberOfHeaters)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.1046962840", sessionID);
        form.AddField("entry.1306942327", userID);
        form.AddField("entry.1054292080", numberOfAttempts);
        form.AddField("entry.141035311", numberOfPlanks);
        form.AddField("entry.163654761", numberOfSprings);
        form.AddField("entry.1061744410", numberOfHeaters);

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
