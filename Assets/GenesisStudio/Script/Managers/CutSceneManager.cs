using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using static CutSceneManager;

public class CutSceneManager : Singleton<CutSceneManager>
{
    [SerializeField] private List<CutScene> cutScenes = new List<CutScene>();

    [SerializeField] PlayableDirector Director;
    bool isPlaying = false;

    [Serializable]
    public class CutScene
    {
        public string name;
        public PlayableAsset asset;
        [HideInInspector] public bool played = false;

        public void Play(PlayableDirector director)
        {
            director.playableAsset = asset;
            director.Play();    
        }
    }

    public CutScene GetCutSceneByName(string name) => cutScenes.FirstOrDefault(c => c.name == name);

    private IEnumerator startCutScene(string name, bool disablePlayer = false)
    {
        isPlaying = true;
        if(disablePlayer)
            GameManager.Instance.PlayerEnable(false);
        var cs = GetCutSceneByName(name);
        cs?.Play(Director);
        cs.played = false;
        yield return new WaitForSeconds((float)cs?.asset.duration); // wait for the cutscene to finish  
        if (disablePlayer)
            GameManager.Instance.PlayerEnable(true);
        cs.played = true;
        isPlaying = false;
    }

    public void StartCutScene(string cutScene)
    {
        StartCoroutine(startCutScene(cutScene));
    }
}
