using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject setting;
    public TMP_InputField inputField;
    public TextMeshProUGUI startGameForPlayer;
    public Camera gameStartCamera;
    public GameObject gameStartName;
    public GameObject notification;
    public void GameStart(bool enter) // 게임시작 버튼 클릭
    {
        if (enter)
            gameStartCamera.targetDisplay = 0;
        else
            gameStartCamera.targetDisplay = 1;
        mainMenu.SetActive(!enter);
        gameStartName.SetActive(enter);

    }
    public void SettingName() // 게임시작(이름을 입력하고) 버튼 클릭
    {
        notification.SetActive(true);
        UIManager.setPlayerName = inputField.text; // InputField에 입력한 값을 몬스터 이름으로 바꿔준다.
        startGameForPlayer.text = "몬스터의 이름을 " + inputField.text + "로 설정하시겠습니까?";
    }
    public void SettingName2(bool yes) 
    {
        if (yes) // 예 시작하겠습니다.
            SceneManager.LoadScene("PlayerTest");
        else // 아니요 다시 짓겠습니다.
            notification.SetActive(false);
    }
    public void Setting(bool enter) // 설정 버튼
    {
        mainMenu.SetActive(!enter);
        setting.SetActive(enter);
    }
    public void GameExit() // 나가기 버튼
    {
        Application.Quit();
    }
}
