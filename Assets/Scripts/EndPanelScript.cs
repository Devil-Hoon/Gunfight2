using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class EndPanelScript : MonoBehaviourPunCallbacks
{
    public GameObject point;
    public GameObject time;
    public PlayManager playManager;
    private float flowTime;
    // Start is called before the first frame update
    void Start()
    {
        flowTime = 15.0f;
    }

    // Update is called once per frame
    void Update()
    {
        flowTime -= Time.deltaTime;

		if (flowTime <= 0)
		{
            flowTime = 15.0f;
            playManager.BackToMain();
		}
        SetTime(flowTime);
    }
    public void SetTime(float time)
	{
        this.time.GetComponent<Text>().text = ((int)time).ToString();
	}
    public void SetPoint(int point)
	{
        this.point.GetComponent<Text>().text = point <= 0 ? "0" : string.Format("{0:#,###}",point.ToString());
	}
}
