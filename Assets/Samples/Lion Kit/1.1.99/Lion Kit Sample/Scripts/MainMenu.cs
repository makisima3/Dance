using UnityEngine;
using LionStudios;
using System.Collections.Generic;
using LionStudios.Debugging;

public class MainMenu : MonoBehaviour
{
    EventTrackingMenu _EventTracking;
    RegularAds _RegularAds;
    DebuggingMenu _Debugger;

	void Start()
    {
        _EventTracking = GetComponentInChildren<EventTrackingMenu>(true);
        _RegularAds = GetComponentInChildren<RegularAds>(true);
        _Debugger = GetComponentInChildren<DebuggingMenu>(true);

        //LionKit.Initialize();
        Analytics.OnAttachDefaultEventParams += UpdateEventParams;
    }

    void UpdateEventParams(Dictionary<string, object> eventParams)
    {
        if (eventParams == null)
            return;

        //eventParams[Analytics.Key.Param.level] = Random.Range(1, 100);
        //eventParams[Analytics.Key.Param.score] = Random.Range(1f, 1000f);
    }

	public void ShowConsentDialog()
	{
#if UNITY_EDITOR && LION_KIT_DEV
        GDPR.ShowPromptDbg(0);
#else
        GDPR.Show();
#endif
    }

	public void EventTracking()
	{
        _EventTracking.gameObject.SetActive(true);
    }

	public void RegularAds()
	{
        _RegularAds.gameObject.SetActive(true);
	}

	public void CrossPromoAds()
	{
        if (CrossPromo.Active)
        {
            CrossPromo.Hide();
        }
        else
        {
            CrossPromo.Show();
        }
    }

    public void Debugging()
    {
        LionDebugger.Show(ignoreInstallMode: true);
    }
}
