using UnityEngine;
using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FacebookHolder : MonoBehaviour
{
	public GameObject DialogLoggedIn;
	public GameObject DialogLoggedOut;
	public Image ProfilePicture;
	public Text UserName;

	void Awake()
	{
		FacebookManager.Instance.InitFacebook();
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
				FacebookManager.Instance.isLoggedIn = true;
				FacebookManager.Instance.GetProfile();
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

			StartCoroutine(WaitForProfileName());
			StartCoroutine(WaitForProfilePicture());
		}
		else
		{
			DialogLoggedIn.SetActive(false);
			DialogLoggedOut.SetActive(true);
		}
	}

	IEnumerator WaitForProfileName()
	{
		while (FacebookManager.Instance.profileName == null)
		{
			yield return null;
		}

		UserName.text = "Hi " + FacebookManager.Instance.profileName;
	}

	IEnumerator WaitForProfilePicture()
	{
		while (FacebookManager.Instance.profilePicture == null)
		{
			yield return null;
		}

		ProfilePicture.sprite = FacebookManager.Instance.profilePicture;
	}

	public void Share()
	{
		FacebookManager.Instance.Share();
	}

	public void Invite()
	{
		FacebookManager.Instance.Invite();
	}

	public void ShareWithUsers()
	{
		FacebookManager.Instance.ShareWithUsers();
	}
}
