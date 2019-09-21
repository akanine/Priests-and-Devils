using UnityEngine;
using System.Collections;
using Com.game;

public class View : MonoBehaviour
{
    IUserActions act;
    GameSceneController my;
    float width, height;

    void Start()
    {
        my = GameSceneController.GetInstance();
        act = GameSceneController.GetInstance() as IUserActions;
    }

    float stw(float scale)
    {
        return (Screen.width - width) / scale;
    }

    float sth(float scale)
    {
        return (Screen.height - height) / scale;
    }

    void OnGUI()
    {
        width = Screen.width / 15;
        height = Screen.height / 15;
        print(my.state);
        if (my.state == State.WIN)
        {
            if (GUI.Button(new Rect(stw(2f), sth(6f), width, height), "Your Win!"))
            {
                act.restart();
            }
        }
        else if (my.state == State.LOSE)
        {
            if (GUI.Button(new Rect(stw(2f), sth(6f), width, height), "Your Lose!"))
            {
                act.restart();
            }
        }
        else
        {
            //通过接口实现动作
                if (my.state == State.START || my.state == State.END)
            {
                if (GUI.Button(new Rect(stw(2f), sth(6f), width, height), "GO"))
                {
                    act.moveBoat();
                }
                if (GUI.Button(new Rect(stw(10.5f), sth(4.5f), width, height), "ON"))
                {
                    act.devil_SOnB();
                }
                if (GUI.Button(new Rect(stw(4.3f), sth(4.5f), width, height), "ON"))
                {
                    act.priest_SOnB();
                }
                if (GUI.Button(new Rect(stw(1.1f), sth(4.5f), width, height), "ON"))
                {
                    act.devil_EOnB();
                }
                if (GUI.Button(new Rect(stw(1.3f), sth(4.5f), width, height), "ON"))
                {
                    act.priest_EOnB();
                }
                if (GUI.Button(new Rect(stw(2.5f), sth(1.2f), width, height), "OFF"))
                {
                    act.offBoatS();
                }
                if (GUI.Button(new Rect(stw(1.7f), sth(1.2f), width, height), "OFF"))
                {
                    act.offBoatE();
                }
            }
        }
    }
}