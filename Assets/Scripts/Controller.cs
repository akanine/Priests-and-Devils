using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.game;

public class Controller : MonoBehaviour
{

    GameObject[] boat = new GameObject[2];
    GameObject boat_obj;
    GameSceneController my;
    Stack<GameObject> p_start = new Stack<GameObject>();
    Stack<GameObject> p_end = new Stack<GameObject>();
    Stack<GameObject> d_start = new Stack<GameObject>();
    Stack<GameObject> d_end = new Stack<GameObject>();
    Vector3 bankStart = new Vector3(0, 0, -12);
    Vector3 priestStart = new Vector3(0, 2.8f, -11f);
    Vector3 devilStart = new Vector3(0, 2.8f, -16f);
    Vector3 priestEnd = new Vector3(0, 2.8f, 8f);
    Vector3 devilEnd = new Vector3(0, 2.8f, 13f);
    Vector3 boatStart = new Vector3(0, 0, -4);
    Vector3 bankEnd = new Vector3(0, 0, 12);
    Vector3 boatEnd = new Vector3(0, 0, 4);
    Vector3 River = new Vector3(0, -1.6f, 0f);
    public float speed = 50f;

    void Start()
    {
        my = GameSceneController.GetInstance();
        my.setGenGameObject(this);
        Screen();
    }

    //实例化游戏对象
    void Screen()
    {
        Instantiate(Resources.Load("Prefabs/Bank"), bankStart, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/Bank"), bankEnd, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/River"), River, Quaternion.identity);
        boat_obj = Instantiate(Resources.Load("Prefabs/Boat"), boatStart, Quaternion.identity) as GameObject;

        for (int i = 0; i < 3; ++i)
        {
            p_start.Push(Instantiate(Resources.Load("Prefabs/Priest")) as GameObject);
            d_start.Push(Instantiate(Resources.Load("Prefabs/Devil")) as GameObject);
        }
    }

    //检查游戏状态
    void check()
    {
        int p_on_Bank = 0, d_on_Bank = 0;
        int p_s = 0, d_s = 0, p_e = 0, d_e = 0;

        if (p_end.Count == 3 && d_end.Count == 3)
        {
            my.state = State.WIN;
            return;
        }

        for (int i = 0; i < 2; ++i)
        {
            if (boat[i] != null && boat[i].tag == "Priest")
                p_on_Bank++; //给牧师和恶魔添加Tag，以区分
            else if (boat[i] != null && boat[i].tag == "Devil")
                d_on_Bank++;
        }
        if (my.state == State.START)
        {
            p_s = p_start.Count + p_on_Bank;
            d_s = d_start.Count + d_on_Bank;
            p_e = p_end.Count;
            d_e = d_end.Count;
        }
        else if (my.state == State.END)
        {
            p_e = p_on_Bank + p_end.Count;
            d_e = d_on_Bank + d_end.Count;
            p_s = p_start.Count;
            d_s = d_start.Count;
        }
        if ((p_e < d_e && p_e != 0) || (p_s < d_s && p_s != 0))
        {
            my.state = State.LOSE;
        }
    }

        int boatCapacity()
    {
        int capacity = 0;
        for (int i = 0; i < 2; ++i)
        {
            if (boat[i] == null) capacity++;
        }
        return capacity;
    }
    //动作上船
    void getOnBoat(GameObject obj)
    {
        if (boatCapacity() != 0)
        {
            obj.transform.parent = boat_obj.transform; //把船设置为游戏角色的子对象
            if (boat[0] == null)
            {
                boat[0] = obj;
                obj.transform.localPosition = new Vector3(0, -0.9f, 0);
            }
            else
            {
                boat[1] = obj;
                obj.transform.localPosition = new Vector3(0, -0.9f, 0.3f);
            }
        }
    }
    //动作开船
    public void moveBoat()
    {
        if (boatCapacity() != 2)
        {
            if (my.state == State.START)
            {
                my.state = State.STOE;
            }
            else if (my.state == State.END)
            {
                my.state = State.ETOS;
            }
        }
    }

    //动作下船
    public void getOffTheBoat(int side)
    {
        if (boat[side] != null)
        {
            boat[side].transform.parent = null; //取消船和角色的父子关系
            if (my.state == State.END)
            {
                if (boat[side].tag == "Priest")
                {
                 p_end.Push(boat[side]);
                }
                else if (boat[side].tag == "Devil")
                {
                    d_end.Push(boat[side]);
                }
            }
            else if (my.state == State.START)
            {
                if (boat[side].tag == "Priest")
                {
                    p_start.Push(boat[side]);
                }
                else if (boat[side].tag == "Devil")
                {
                    d_start.Push(boat[side]);
                }
            }
            boat[side] = null;
        }
    }

    void set_Position(Stack<GameObject> stack, Vector3 pos)
    {
        GameObject[] array = stack.ToArray();
        for (int i = 0; i < stack.Count; ++i)
        {
            array[i].transform.position = new Vector3(pos.x, pos.y, pos.z + 1.5f * i);
        }
    }

    public void priestStartOnBoat()
    {
        if (my.state == State.START && p_start.Count != 0 && boatCapacity() != 0)
            getOnBoat(p_start.Pop());
    }

    public void devilStartOnBoat()
    {
        if (my.state == State.START && d_start.Count != 0 && boatCapacity() != 0)
            getOnBoat(d_start.Pop());
    }

    public void priestEndOnBoat()
    {
        if (my.state == State.END && p_end.Count != 0 && boatCapacity() != 0)
            getOnBoat(p_end.Pop());
    }

    public void devilEndOnBoat()
    {
        if (my.state == State.END && d_end.Count != 0 && boatCapacity() != 0)
            getOnBoat(d_end.Pop());
    }

    void Update()
    {
        set_Position(p_start, priestStart);
        set_Position(p_end, priestEnd);
        set_Position(d_start, devilStart);
        set_Position(d_end, devilEnd);

        if (my.state == State.STOE)
        {
            boat_obj.transform.position = Vector3.MoveTowards(boat_obj.transform.position, boatEnd, speed * Time.deltaTime);
            if (boat_obj.transform.position == boatEnd)
            {
                my.state = State.END;
            }
        }
        else if (my.state == State.ETOS)
        {
            boat_obj.transform.position = Vector3.MoveTowards(boat_obj.transform.position, boatStart, speed * Time.deltaTime);
            if (boat_obj.transform.position == boatStart)
            {
                my.state = State.START;
            }
        }
        else check();
    }
}
