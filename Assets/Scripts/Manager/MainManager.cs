using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySqlConnector;

public class MainManager : MonoBehaviourPunCallbacks
{
	private readonly string gameVersion = "1";

	public Button joinLobby;
	public Button howToPlayBtn;

	public GameObject explainPanel;
	public GameObject optionPanel;
	public GameObject loginPanel;

	private bool isConnected;

	[Header("Toggle")]
	public GameObject BGToggle;
	public GameObject EffectToggle;
	private bool optionPanelOnoff;
	bool explainShow;
	float explainY;

	[Header("Login")]
	public InputField idInput;
	public InputField pwInput;
	public Button loginBtn;
	public Button logoutBtn;

	[Header("Alarm")]
	public GameObject alarmPanel;
	public Text alarmText;
	public Button closeBtn;

	[Header("VersionCheck")]
	public GameObject vCheckPanel;
	public Button vCheckButton;
	public VersionSet vSet;
	// Start is called before the first frame update

	private void Awake()
	{
		Screen.SetResolution(960, 600, false);
	}
	private void OnApplicationQuit()
	{
		PhotonNetwork.Disconnect();
		DBManager.LogOut();
	}
	void Start()
	{
		vSet.VersionInit();
		if (!DBManager.GameVersionCheck())
		{
			vCheckPanel.SetActive(true);
		}
		else
		{
			vCheckPanel.SetActive(false);
		}
		alarmPanel.SetActive(false);
		isConnected = false;
		explainShow = false;
		explainY = 900.0f;


		Toggle toggle = BGToggle.GetComponent<Toggle>();
		toggle.onValueChanged.AddListener(BGSoundOnOff);
		if (BGSoundManager.instance != null)
		{
			toggle.isOn = BGSoundManager.instance.isOn;
		}
		toggle = EffectToggle.GetComponent<Toggle>();
		toggle.onValueChanged.AddListener(EffectSoundOnOff);
		if (EffectSoundManager.instance != null)
		{
			toggle.isOn = EffectSoundManager.instance.isOn;
		}

			
		if (DBManager.LoggedIn)
		{
			BGSoundManager.instance.StopBGM();
			BGSoundManager.instance.PlayGunfightMainBGM();
			DBManager.LoadGP();

			loginPanel.SetActive(false);
		}
		else
		{
			loginPanel.SetActive(true);
		}

		optionPanel.SetActive(false);
	}

	public override void OnConnectedToMaster()
	{
		//if (joinLobby != null)
		//{
		//	joinLobby.interactable = true;
		//}
		Debug.Log("연결성공");
		isConnected = true;

		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		//if (joinLobby != null)
		//{
		//	joinLobby.interactable = false;
		//}
		if (!isConnected)
		{
			Debug.Log("연결실패");
			PhotonNetwork.ConnectUsingSettings();
		}
		else
		{
			Debug.Log("연결 종료");
			isConnected = false;
		}
	}

	public void Connect()
	{
		if (DBManager.gp <= 0)
		{
			alarmText.text = "보유 GP가 부족합니다.";
			alarmPanel.SetActive(true);
			return;
		}
		PhotonNetwork.GameVersion = gameVersion;
		PhotonNetwork.SendRate = 60;
		PhotonNetwork.SerializationRate = 60;
		PhotonNetwork.PhotonServerSettings.DevRegion = "kr";
		PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "kr";
		PhotonNetwork.ConnectUsingSettings();

		//if (PhotonNetwork.IsConnected)
		//{
		//	Debug.Log("방참가");
		//	PhotonNetwork.JoinRandomRoom();
		//	joinLobby.interactable = false;
		//}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("방생성");
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "MasterPanel", 1 } }  });
	}

	public override void OnJoinedLobby()
	{
	}
	public override void OnJoinedRoom()
	{
		Debug.Log("방참가성공");
		PhotonNetwork.IsMessageQueueRunning = false;
		PhotonNetwork.LoadLevel("GunfightLobby");
	}
	// Update is called once per frame
	void Update()
    {
		if (explainShow)
		{
			explainY -= Time.deltaTime * 400.0f;
			if (explainY < 300.0f)
			{
				explainY = 300.0f;
			}

			explainPanel.transform.localPosition = new Vector3(0, explainY, 0);
		}
		else
		{
			explainY += Time.deltaTime * 400.0f;
			if (explainY > 900.0f)
			{
				explainY = 900.0f;
			}

			explainPanel.transform.localPosition = new Vector3(0, explainY, 0);
		}
	}
	public void ShowOptionView()
	{
		optionPanelOnoff = !optionPanelOnoff;
		optionPanel.SetActive(optionPanelOnoff);
	}
	
	public void BGSoundOnOff(bool state)
	{
		BGSoundManager.instance.isOn = state;
		if (!BGSoundManager.instance.isOn)
		{
			BGSoundManager.instance.StopBGM();
		}
		else
		{
			BGSoundManager.instance.Play();
		}
	}

	public void EffectSoundOnOff(bool state)
	{
		EffectSoundManager.instance.isOn = state;
	}	
	public void Login()
	{
		string alarmMesseage = "";
		if (DBManager.Login(idInput.text, pwInput.text, out alarmMesseage))
		{
			loginPanel.SetActive(false);

			idInput.text = "";
			pwInput.text = "";
		}
		else
		{
			alarmText.text = alarmMesseage;
			alarmPanel.SetActive(true);
			Debug.Log("Login Failed");
		}
	}
	public void LogOut()
	{
		if (DBManager.LogOut())
		{
			//PhotonNetwork.Disconnect();
			Debug.Log("연결해제");
			BGSoundManager.instance.StopBGM();
			idInput.text = "";
			pwInput.text = "";
			loginPanel.SetActive(true);
		}
	}
	public void AlarmPanelOff()
	{
		if (alarmPanel.activeSelf)
		{
			alarmPanel.SetActive(false);
		}
	}
	public void ShowExplainPanel()
	{
		explainShow = true;
		joinLobby.interactable = false;
		howToPlayBtn.interactable = false;
	}

	public void ExitExplainPanel()
	{
		explainShow = false;
		joinLobby.interactable = true;
		howToPlayBtn.interactable = true;
	}

	public void GameQuit()
	{
		Application.Quit();
	}
}
