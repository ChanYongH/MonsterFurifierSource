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
    public void GameStart(bool enter) // ���ӽ��� ��ư Ŭ��
    {
        if (enter)
            gameStartCamera.targetDisplay = 0;
        else
            gameStartCamera.targetDisplay = 1;
        mainMenu.SetActive(!enter);
        gameStartName.SetActive(enter);

    }
    public void SettingName() // ���ӽ���(�̸��� �Է��ϰ�) ��ư Ŭ��
    {
        notification.SetActive(true);
        UIManager.setPlayerName = inputField.text; // InputField�� �Է��� ���� ���� �̸����� �ٲ��ش�.
        startGameForPlayer.text = "������ �̸��� " + inputField.text + "�� �����Ͻðڽ��ϱ�?";
    }
    public void SettingName2(bool yes) 
    {
        if (yes) // �� �����ϰڽ��ϴ�.
            SceneManager.LoadScene("PlayerTest");
        else // �ƴϿ� �ٽ� ���ڽ��ϴ�.
            notification.SetActive(false);
    }
    public void Setting(bool enter) // ���� ��ư
    {
        mainMenu.SetActive(!enter);
        setting.SetActive(enter);
    }
    public void GameExit() // ������ ��ư
    {
        Application.Quit();
    }
}
