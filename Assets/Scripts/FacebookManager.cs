using UnityEngine;
using Facebook.Unity;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour
{
	public GameObject DialogLoggedIn;
	public GameObject DialogLoggedOut;
	public Image ProfilePicture;
	public Text UserName;

	void Awake()
	{
		FB.Init(SetInit, OnHideUnity);
	}

	void SetInit()
	{
		if (FB.IsLoggedIn)
		{
			Debug.Log("FB logged in");
		}
		else
		{
			Debug.Log("FB is not logged in");
		}
		DealWithFBMenus();
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	public void LogIn()
	{
		List<string> permissions = new List<string>();
		permissions.Add("public_profile");
		FB.LogInWithReadPermissions(permissions, AuthCallBack);
	}

	void AuthCallBack(IResult result)
	{
		if (result.Error != null)
		{
			Debug.Log(result.Error);
		}
		else
		{
			if (FB.IsLoggedIn)
			{
				Debug.Log("FB logged in");
			}
			else
			{
				Debug.Log("FB is not logged in");
			}
			DealWithFBMenus();
		}
	}

	public void DealWithFBMenus()
	{
		if (FB.IsLoggedIn)
		{
			DialogLoggedIn.SetActive(true);
			DialogLoggedOut.SetActive(false);

			FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUserName);
			FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePicture);
		}
		else
		{
			DialogLoggedIn.SetActive(false);
			DialogLoggedOut.SetActive(true);
		}
	}

	void DisplayUserName(IResult result)
	{
		if (result.Error == null)
		{
			UserName.text = "Hi there, " + result.ResultDictionary["first_name"];
		}
		else
		{
			Debug.Log(result.Error);
		}
	}

	void DisplayProfilePicture(IGraphResult result)
	{
		if (result.Error != null || result.Texture == null)
		{
			Debug.Log(result.Error);
		}
		else
		{
			ProfilePicture.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), Vector2.zero);
		}
	}
}
