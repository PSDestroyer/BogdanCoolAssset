//clasele sunt facute pentru extindere viitoare
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private LoadingScene LoadingScreenPrefab
    {
        get
        {
            return Resources.Load<LoadingScene>("Loading/Canvas");
        }
    }

    private LevelData GetData(string id)
    {
        return (from level in Resources.LoadAll<LevelData>("Loading") 
            let levelID = level.ID() 
            where levelID == id 
            select level)
            .FirstOrDefault();
    }

    public void LoadScene(string sceneName)
    {
        LoadScene(GetData(sceneName));
    }

    public void LoadScene(LevelData level)
    {
        GameManager.Instance.Player.Controls(false);
        var instance = Instantiate(LoadingScreenPrefab);
        instance.LoadScene(level.Scene, level.ID());
    }
}
