using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    protected override void Awake()
    {
        base.Awake();
        //���[�J�n�J�����N�����g�U�C��k
        StartCoroutine(AwakeScene());
    }

    private IEnumerator AwakeScene()
    {
        yield return SceneManager.LoadSceneAsync("Home", LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
    }

    public void Transition(string from,string to)
    {
        StartCoroutine(TransitionToScene(from,to));
    }

    private IEnumerator TransitionToScene(string form,string to)
    {
        EventHandler.CallBeforeSceneUnloadEvent();

        yield return SceneManager.UnloadSceneAsync(form);
        yield return SceneManager.LoadSceneAsync(to,LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount-1);
        SceneManager.SetActiveScene(newScene);

        EventHandler.CallAfterSceneLoadedEvent();
    }
}
