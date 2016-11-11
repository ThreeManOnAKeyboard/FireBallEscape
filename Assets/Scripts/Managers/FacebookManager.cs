using Facebook.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookManager : MonoBehaviour
{
	private static FacebookManager _instance;
	public static FacebookManager Instance
	{
		get
		{
			if (_instance == null)
			{
				new GameObject("FacebookManager").AddComponent<FacebookManager>();
			}

			return _instance;
		}
	}

	public bool isLoggedIn { get; set; }
	public string profileName { get; set; }
	public Sprite profilePicture { get; set; }
	public string appLinkURL { get; set; }

	private float currentTimeScale;

	void Awake()
	{
		_instance = this;
		isLoggedIn = true;
		appLinkURL = "https://play.google.com/store/apps/details?id=com.games_for_rest.drum_kit_second_free";
		DontDestroyOnLoad(gameObject);
	}

	public void InitFacebook()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(SetInit, OnHideUnity);
		}
		else
		{
			isLoggedIn = FB.IsLoggedIn;
		}
	}

	void SetInit()
	{
		if (FB.IsLoggedIn)
		{
			GetProfile();
			Debug.Log("FB logged in");
		}
		else
		{
			Debug.Log("FB is not logged in");
		}

		isLoggedIn = FB.IsLoggedIn;
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			currentTimeScale = Time.timeScale;
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = currentTimeScale;
		}
	}

	public void GetProfile()
	{
		FB.API("/me?fields=first_name", HttpMethod.GET, GetUserName);
		FB.API("/me/picture?type=square&height=280&width=280", HttpMethod.GET, GetProfilePicture);
	}

	void GetUserName(IResult result)
	{
		if (result.Error == null)
		{
			profileName = "" + result.ResultDictionary["first_name"];
		}
		else
		{
			Debug.Log(result.Error);
		}
	}

	void GetProfilePicture(IGraphResult result)
	{
		if (result.Error != null || result.Texture == null)
		{
			Debug.Log(result.Error);
		}
		else
		{
			profilePicture = Sprite.Create(result.Texture, new Rect(0, 0, 280, 280), Vector2.zero);
		}
	}

	public void Share()
	{
		FB.FeedShare
		(
			string.Empty,
			new Uri(appLinkURL),
			"Test Title",
			"Test Caption",
			"Check this test",
			new Uri("https://i.ytimg.com/vi/yaqe1qesQ8c/maxresdefault.jpg"),
			string.Empty,
			ShareCallback
		);
	}

	void ShareCallback(IResult result)
	{
		if (result.Cancelled)
		{
			Debug.Log("Share Canceled");
		}
		else if (!string.IsNullOrEmpty(result.Error))
		{
			Debug.Log(result.Error);
		}
		else
		{
			Debug.Log("Succes on share");
		}
	}

	public void Invite()
	{
		FB.Mobile.AppInvite
		(
			new Uri(appLinkURL),
			new Uri("https://i.ytimg.com/vi/yaqe1qesQ8c/maxresdefault.jpg"),
			InviteCallback
		);
	}

	void InviteCallback(IResult result)
	{
		if (result.Cancelled)
		{
			Debug.Log("Invite Canceled");
		}
		else if (!string.IsNullOrEmpty(result.Error))
		{
			Debug.Log(result.Error);
		}
		else
		{
			Debug.Log("Succes on Invite");
		}
	}

	public void ShareWithUsers()
	{
		FB.AppRequest
		(
			"Can you beat my score???",
			null,
			new List<object>() { "app_users" },
			null,
			null,
			null,
			null,
			ShareWithUsersCallback
		);
	}

	void ShareWithUsersCallback(IAppRequestResult result)
	{
		if (result.Cancelled)
		{
			Debug.Log("Challange Canceled");
		}
		else if (!string.IsNullOrEmpty(result.Error))
		{
			Debug.Log(result.Error);
		}
		else
		{
			Debug.Log("Succes on Challange");
		}
	}
}
