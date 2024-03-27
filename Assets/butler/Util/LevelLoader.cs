using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
	private class LoadingMonoBehaviour : MonoBehaviour { }

	public static event Action<int> LevelLoaded;

	private static GameObject loaderGO;
	private static LoadingCanvas lc;

	private static int currentLevel;
	private static readonly float minLoadTime = 2f;

	public static void LoadLevel(int level)
	{
		if (loaderGO == null)
		{
			loaderGO = new("Scene Loader");
			var loadingCanvas = Resources.Load<GameObject>("LoadingCanvas");
			lc = GameObject.Instantiate(loadingCanvas, loaderGO.transform).GetComponent<LoadingCanvas>();
			UnityEngine.Object.DontDestroyOnLoad(loaderGO);
			//Debug.Break();
		}

		loaderGO.GetOrAddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadScene(level));

		currentLevel = level;
	}

	public static void LoadNextLevel()
	{
		if (SceneManager.sceneCountInBuildSettings >= currentLevel + 2)
			LoadLevel(currentLevel + 1);
		else
			LoadMainMenu();
	}

	public static void ReloadLevel()
	{
		LoadLevel(currentLevel);
	}

	public static void LoadMainMenu()
	{
		LoadLevel(0);
	}

	private static IEnumerator LoadRoutine(int levelIndex)
	{
		lc.SetVisible(true, 0.5f);
		lc.SetFill(0f);

		yield return new WaitForSeconds(0.5f);

		var asyncOperation = SceneManager.LoadSceneAsync(levelIndex);
		asyncOperation.allowSceneActivation = false;

		var elapsed = 0f;
		while (asyncOperation.progress < 0.9f)
		{
			elapsed += Time.deltaTime;
			lc.SetFill(asyncOperation.progress / 0.9f);
			yield return null;
		}

		if (elapsed < minLoadTime)
			yield return new WaitForSeconds(minLoadTime - elapsed);

		asyncOperation.allowSceneActivation = true;
		while (!asyncOperation.isDone)
			yield return null;

		lc.SetFill(1f);
		lc.SetVisible(false, 0.5f);

		yield return new WaitForSeconds(0.5f);

		LevelLoaded?.Invoke(levelIndex);
	}

	private static IEnumerator LoadScene(int levelIndex)
	{
		lc.SetVisible(true, 0.5f);
		lc.SetFill(0f);

		yield return new WaitForSeconds(0.5f);

		//Begin to load the Scene you specify
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelIndex);

		//Don't let the Scene activate until you allow it to
		asyncOperation.allowSceneActivation = false;
		Debug.Log("Progress :" + asyncOperation.progress);

		//When the load is still in progress, output the Text and progress bar
		while (!asyncOperation.isDone)
		{
			//Output the current progress
			lc.SetInfoText("Loading progress: " + (asyncOperation.progress * 100) + "%");
			lc.SetFill(asyncOperation.progress);

			// Check if the load has finished
			if (asyncOperation.progress >= 0.9f)
			{
				//Change the Text to show the Scene is ready

				lc.SetInfoText("Press the space bar to continue");

				//Wait to you press the space key to activate the Scene
				if (Input.GetKeyDown(KeyCode.Space))
					//Activate the Scene
					asyncOperation.allowSceneActivation = true;
			}

			yield return null;
		}

		lc.SetFill(1f);
		lc.SetVisible(false, 0.5f);

		yield return new WaitForSeconds(0.5f);
		LevelLoaded?.Invoke(levelIndex);
	}
}