using UnityEngine;
using System.Collections;
using Com.game;

public class Model : MonoBehaviour
{
    void Start()
    {
        GameSceneController my = GameSceneController.GetInstance();
        my.setBaseCode(this);
    }
}

namespace Com.game
{
    //实现接口
    public interface IUserActions
    {
        void priest_SOnB();
        void priest_EOnB();
        void devil_SOnB();
        void devil_EOnB();
        void moveBoat();
        void offBoatS();
        void offBoatE();
        void restart();
    }

    public enum State { START, STOE, ETOS, END, WIN, LOSE }; //游戏的六种当前状态

    public class GameSceneController : System.Object, IUserActions
    {

        private static GameSceneController instance;
        private Model base_code;
        private Controller game_obj;
        public State state = State.START;

        public Model getBaseCode()
        {
            return base_code;
        }

        internal void setBaseCode(Model c)
        {
            if (null == base_code)
            {
                base_code = c;
            }
        }

        public Controller getGenGameObject()
        {
            return game_obj;
        }

        internal void setGenGameObject(Controller ggo)
        {
            if (null == game_obj)
            {
                game_obj = ggo;
            }
        }

        public void priest_SOnB()
        {
            game_obj.priestStartOnBoat();
        }

        public void priest_EOnB()
        {
            game_obj.priestEndOnBoat();
        }

        public void devil_SOnB()
        {
            game_obj.devilStartOnBoat();
        }

        public void offBoatS()
        {
            game_obj.getOffTheBoat(1);
        }

        public void offBoatE()
        {
            game_obj.getOffTheBoat(0);
        }

        public void devil_EOnB()
        {
            game_obj.devilEndOnBoat();
        }

        public void moveBoat()
        {
            game_obj.moveBoat();
        }

        public static GameSceneController GetInstance()
        {
            if (null == instance)
            {
                instance = new GameSceneController();
            }
            return instance;
        }

        public void restart()
        {
            Application.LoadLevel(Application.loadedLevelName);
            state = State.START;
        }
    }
}
