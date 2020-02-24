using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;

public class InputManager : MonoBehaviour
{


    public int Xbox_One_Controller = 0;
    public int PS4_Controller = 0;
    public int GamePad_Controller = 0;
    public int Nintendo_ProController = 0;
    public int counterGamePad = 0;
    

    private string[] gamePadDetected;

    public class GamePad
    {
        public int id;
        public string name;

        public enum ButtonCode
        {
            START,
            SELECT,
            A,
            B,
            X,
            Y,
            UP,
            LEFT,
            RIGHT,
            DOWN
        }

        public enum typeGamePad
        {
            CONTROLLER_GAME_PAD,
            CONTROLLER_XBOX,
            CONTROLLER_PS4,
            PROCONTROLLER_NINTENDO
        }

        private typeGamePad type;

        public enum TriggerCode
        {
            RIGHT,
            LEFT
        }

        public enum StickCode
        {
            RIGHT,
            LEFT
        }

        public enum Axe
        {
            HORIZONTAl,
            VERTICAL
        }

        public GamePad(int id, typeGamePad type)
        {
            this.id = id;
            this.type = type;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public float GetAxis(StickCode stick, Axe axe)
        {
            float axis = 0.0f;
            switch (this.id)
            {
                case 0:
                    if (stick == StickCode.LEFT)
                    {
                        if (axe == Axe.HORIZONTAl)
                            axis = Input.GetAxis("StickLeft_Horizontal");
                        if (axe == Axe.VERTICAL)
                            axis = Input.GetAxis("StickLeft_Vertical");
                    }

                    if (stick == StickCode.RIGHT)
                    {
                        if (axe == Axe.HORIZONTAl)
                            axis = Input.GetAxis("StickRight_Horizontal");
                        if (axe == Axe.VERTICAL)
                            axis = Input.GetAxis("StickRight_Vertical");
                    }
                    break;
            }

            return axis;
        }

        public bool GetButtonDown(ButtonCode button)
        {
            bool isKeyDown = false;
            switch (this.id)
            {

                case 0:
                    switch (button)
                    {
                        case ButtonCode.A:
                            isKeyDown = Input.GetButtonDown("Fire1");
                            break;

                        case ButtonCode.B:
                            break;

                        case ButtonCode.X:
                            break;

                        case ButtonCode.Y:
                            break;
                    }
                    break;
            }

            return isKeyDown;

        }

        public bool GetButtonUp(ButtonCode button)
        {
            bool isKeyUp = false;
            switch (this.id)
            {

                case 0:
                    switch (button)
                    {
                        case ButtonCode.A:
                            isKeyUp = Input.GetButtonUp("Fire1");
                            break;

                        case ButtonCode.B:
                            break;

                        case ButtonCode.X:
                            break;

                        case ButtonCode.Y:
                            break;
                    }
                    break;
            }
            return isKeyUp;
        }

    }

    public string[] nameGamePads;
    public List<GamePad> gamePads;

    // Start is called before the first frame update
    void Start()
    {
        gamePads = new List<GamePad>();
        nameGamePads = new string[4];
        gamePadDetected = new string[16];
        Debug.Log(gamePadDetected.Length);
    }

    public GamePad GetGamePadFree()
    {
        int index = 0;
            GamePad gamePadFree;
            foreach (GamePad gamepad in gamePads)
            {
                if (gamepad.id == index)
                {
                    index++;
                }
            }
            return new GamePad(index);
    }

    void Update()
    {
        string[] names = Input.GetJoystickNames();


        bool gamepadIsDeteted = false ;

        for (int x = 0; x < names.Length; x++)
        {
        
            //if(Ga)
            // print(names[x].Length 
            if (names[x].Length > 0 ) 
            {
                gamepadIsDeteted = true;
                print(names[x].Length);
                if (gamePadDetected[x] != names[x])
                {
                    string msg = "InputManager: " + names[x] + " is connected";
                    if (names[x].Length == 19)
                    {
                       
                        PS4_Controller = 1;
                        counterGamePad++;
                        gamePadDetected[x] = names[x];
                    }

                    if (names[x].Length == 14)
                    {

                        Nintendo_ProController = 1;
                        counterGamePad++;
                        gamePadDetected[x] = names[x];
                        GamePad newGamePad = GetGamePadFree();
                        newGamePad.SetName(names[x]);
                        nameGamePads[newGamePad.id] = newGamePad.name;
                        gamePads.Add(newGamePad);
                    }
                    if (names[x].Length == 16)
                    {

                        Nintendo_ProController = 1;
                        counterGamePad++;
                        gamePadDetected[x] = names[x];
                        GamePad newGamePad = GetGamePadFree();
                        newGamePad.SetName(names[x]);
                        nameGamePads[newGamePad.id] = newGamePad.name;
                        gamePads.Add(newGamePad);
                    }

                    if (names[x].Length == 20)
                    {
                        GamePad_Controller = 1;
                        gamePadDetected[x] = names[x];
                        counterGamePad++;
                        GamePad newGamePad = GetGamePadFree();
                        newGamePad.SetName(names[x]);
                        nameGamePads[newGamePad.id] = newGamePad.name;
                        gamePads.Add(newGamePad);
                    }

                    if (names[x].Length == 33)
                    {
                        //set a controller bool to true
                        Xbox_One_Controller = 1;
                        gamePadDetected[x] = names[x];
                        counterGamePad++;
                        GamePad newGamePad = GetGamePadFree();
                        newGamePad.SetName(names[x]);
                        nameGamePads[newGamePad.id] = newGamePad.name;
                        gamePads.Add(newGamePad);
                       
                    }
                    print(msg);
                }


                //SWITCH
            }
            else
            {
               
                if (gamePadDetected[x] != null)
                {
                    print(gamePadDetected[x] + " is disconnected");
                    if (gamePadDetected[x].Length == 19)
                    {
                        PS4_Controller = 0;
                        gamePadDetected[x] = null;
                        counterGamePad--;
                        return;
                    }

                    if (gamePadDetected[x].Length == 20)
                    {
                        GamePad_Controller = 0;
                        gamePadDetected[x] = null;
                        foreach (var gamePad in gamePads)
                        {
                            if (gamePad.id == x)
                            {
                                nameGamePads[gamePad.id] = "FUCK";
                                gamePads.Remove(gamePad);
                                break;
                            }
                        }
                        counterGamePad--;
                        return;
                    }

                    if (gamePadDetected[x].Length == 33)
                    {
                        //set a controller bool to true
                        Xbox_One_Controller = 0;
                        gamePadDetected[x] = null;

                        foreach (var gamePad in gamePads)
                        {
                            if (gamePad.id == x)
                            {
                                
                                nameGamePads[gamePad.id] = "FUCK";
                                gamePads.Remove(gamePad);
                                break;
                            }
                        }

                        counterGamePad--;
                        return;
                    }

                    if (gamePadDetected[x].Length == 14 || gamePadDetected[x].Length == 16)
                    {
                        //set a controller bool to true
                        Nintendo_ProController = 0;
                        gamePadDetected[x] = null;

                        foreach (var gamePad in gamePads)
                        {
                            if (gamePad.id == x)
                            {

                                nameGamePads[gamePad.id] = "FUCK";
                                gamePads.Remove(gamePad);
                                break;
                            }
                        }

                        counterGamePad--;
                        return;
                    }
                }
            }
        }


        if (!gamepadIsDeteted)
        {
            if (gamePads.Count != 0)
            {
                gamePads.Clear();
                nameGamePads[0] = "FUCK";
                nameGamePads[1] = "FUCK";
                nameGamePads[2] = "FUCK";
                nameGamePads[3] = "FUCK";
            }
        }


        if (Xbox_One_Controller == 1)
        {
            //do something
        }
        else if (PS4_Controller == 1)
        {
            //do something
        }
        else
        {
            // there is no controllers
        }
    }




    // Update is called once per frame
    //void Update()
    //{



    //    if (Input.mouseScrollDelta.y != 0)
    //    {
    //        if ((Input.mouseScrollDelta.y < 0 || GameObject.Find("Spatialship").transform.localScale.y >= 0.4f) &&
    //            (Input.mouseScrollDelta.y > 0 || GameObject.Find("Spatialship").transform.localScale.y <= 1.0f))
    //        {
    //            GameObject.Find("Spatialship").transform.localScale -= Vector3.one * (Input.mouseScrollDelta.y / 10);
    //            GameObject.Find("FirePropulseur_big").transform.localScale -= Vector3.one * (Input.mouseScrollDelta.y / 10);
    //            GameObject.Find("FirePropulseur_little").transform.localScale -= Vector3.one * (Input.mouseScrollDelta.y / 60);
    //            GameObject.Find("FirePropulseur_medium").transform.localScale -= Vector3.one * (Input.mouseScrollDelta.y / 40);
    //            GameObject.Find("FirePropulseur_medium2").transform.localScale -= Vector3.one * (Input.mouseScrollDelta.y / 40);
    //            GameObject.Find("FirePropulseur_medium3").transform.localScale -= Vector3.one * (Input.mouseScrollDelta.y / 40);

    //        }
    //    }
    //}
}
