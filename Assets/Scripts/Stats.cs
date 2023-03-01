using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Stats : MonoBehaviour {
    public Image background;
    public RectTransform back, border;
    public TMP_Text stats;
    
    public void setStats(){
        List<string> gameModesWithLevels = new List<string>();
        List<string> gameModesNoLevels = new List<string>();
        foreach(string mode in GameModeSelect.Instance.gameModes){
            if(GameModeSelect.gameModeLevels[mode] == 0)
                gameModesNoLevels.Add(mode);
            else
                gameModesWithLevels.Add(mode);
        }
        stats.text = "LEVELS COMPLETE\n";
        foreach(string mode in gameModesWithLevels)
            stats.text += mode + " : " + GameModeSelect.gameModeCompletedLevels[mode] + "/" + GameModeSelect.gameModeLevels[mode] + "\n";
        stats.text += "\nHIGHSCORES\n";
        foreach(string mode in gameModesNoLevels)
            stats.text += mode + " : " + GameModeSelect.gameModeCompletedLevels[mode] + "\n";
    }
}
