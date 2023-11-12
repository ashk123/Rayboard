/*******************************************************************************************
*
*   raylib Rayboard - Simple Typing Game with raylib
*
*   THis is a simple typing game that use Raylib library
*   If you have any good idea about this game let me know.
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   - 2023 Eiko Akiba
*
********************************************************************************************/


/*
 * TOOD:
 *  - select a better game background
 *  - set vintage effect on all texts
 *  - make a main menu with settings and, etc.
 *  - set a background music
 *  - better sentence random generator
 *  - Attach media files into executable file
*/

using Raylib_cs;
using System.Numerics;
using tamrinSnakemy;
using userInputraylib;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace rayboard;

public class Program
{
    public struct Sounds
    {
        public Sound sample_win;
        public Sound sample_lose;
    }
    public const int MaxInputChars = 50;
    public static char[] name = new char[MaxInputChars];
    public static int letterCount;
    public static int points = 0;
    public static string text;
    public static bool win;
    public static bool lose;
    public static CTimer ctimermy = null;
    public static CTimer updatetimer = null;
    public static float round_speed = 10f;
    public static Sounds effects;
    public static int Windows = 1;
    public static bool pause = false;

    public static void LoseGame()
    {
        Array.Clear(name, 0, name.Length);
        letterCount = 0;
        ctimermy = new CTimer(3f);
        text = Utils.GetRandomString("This is such a cool game", "are you really wanna play?", "nice", "yea I love it!");
        //updatetimer = new CTimer(round_speed);
        points = points <= 0 ? 0 : points - 10;
        Console.WriteLine("welcome");
        lose = true;
        PlaySound(effects.sample_lose);
    }
    public static void WinGame(bool start)
    {
        Utils.SavePlayerStatus("player", 10 - (int)updatetimer.RemainTime(), text);
        Array.Clear(name, 0, name.Length);
        letterCount = 0;
        ctimermy = new CTimer(3f);
        text = Utils.GetRandomString("This is such a cool game", "are you really wanna play?", "nice", "yea I love it!");
        updatetimer.StopTimer();
        Console.WriteLine(Utils.RuntimeStatusList[Utils.RuntimeStatusList.Count - 1].play_name);
        Console.WriteLine(Utils.RuntimeStatusList[Utils.RuntimeStatusList.Count - 1].end_time);
        Console.WriteLine(Utils.RuntimeStatusList[Utils.RuntimeStatusList.Count - 1].text);
        //if (start == false) 
        //    updatetimer = new CTimer(round_speed);
        PlaySound(effects.sample_win);

        points += 10;
        win = true;
    }
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------

        const int screenWidth = 1024;
        const int screenHeight = 800;
        bool start = false;
        win = false;
        text = Utils.GetRandomString("This is such a cool game", "are you really wanna play?", "nice", "yea I love it!");
        string title = "How to play: you need to write this text to win";
        //CTimer? updatetimer = null;
        Vector2 fontPosition2 = new Vector2();
        Vector2 fontPosition4 = new Vector2();

        InitWindow(screenWidth, screenHeight, "raylib [text] example - input box");
        InitAudioDevice();
        Utils.SavePlayerStatus("asd", 5f, "asd");
        effects.sample_win = LoadSound(Path.GetFullPath("media\\win.mp3"));
        effects.sample_lose = LoadSound(Path.GetFullPath("media\\ooh.mp3"));
        //effects.sample_lose = LoadSound("C:\\Users\\Rasta724.com\\Desktop\\test\\userInputraylib\\userInputraylib\\ooh.mp3"); // I should use the background sound instead of this sound effect

        Raylib_cs.Image sample_image = LoadImage(Path.GetFullPath("media\\V.png"));
        ImageResize(ref sample_image, screenWidth, screenHeight);
        Texture2D texture = LoadTextureFromImage(sample_image);
        //Font font = LoadFontEx("resources/KAISG.ttf", 96, 0, 0);
        // NOTE: One extra space required for line ending char '\0'
        name = new char[MaxInputChars];
        letterCount = 0;

        Raylib_cs.Rectangle textBox = new(screenWidth / 2 - 655 / 2, screenHeight / 2, 655, 35);
        bool mouseOnText = true;

        int framesCounter = 0;

        SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())
        {
            if (Windows == 1)
            {
                if (pause == false)
                {
                    if (IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT) && IsKeyDown(KeyboardKey.KEY_LEFT))
                    {
                        Windows = 2;
                        pause = true;
                    }
                    if (start == false && lose == false && win == false)
                    {
                        Console.WriteLine("Im doing this");
                        updatetimer = new CTimer(10f);
                        start = true;
                    }
                    if (start && lose == false)
                    {
                        if (updatetimer.TimerDone() == true && win == false)
                        {
                            //start = false;
                            LoseGame();
                        }
                        else
                        {
                            updatetimer.UpdateTimer();
                        }
                    }
                    // Update
                    if (mouseOnText && win == false && lose == false)
                    {
                        // Set the window's cursor to the I-Beam
                        SetMouseCursor(MouseCursor.MOUSE_CURSOR_IBEAM);

                        // Check if more characters have been pressed on the same frame
                        int key = GetCharPressed();
                        //Console.WriteLine(key);

                        while (key > 0)
                        {
                            // NOTE: Only allow keys in range [32..125]
                            if ((key >= 32) && (key <= 125) && (letterCount < MaxInputChars) && (letterCount < text.Length))
                            {
                                name[letterCount] = (char)key;
                                letterCount++;
                            }

                            // Check next character in the queue
                            key = GetCharPressed();
                        }
                        //Console.WriteLine(framesCounter);
                        if (IsKeyDown(KeyboardKey.KEY_BACKSPACE) && ((framesCounter % 3) == 0))
                        {
                            letterCount -= 1;
                            if (letterCount < 0)
                            {
                                letterCount = 0;
                            }
                            name[letterCount] = '\0';
                        }
                    }
                    else
                    {
                        SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
                    }
                    if (mouseOnText)
                    {
                        framesCounter += 1;
                    }
                    else
                    {
                        framesCounter = 0;
                    }

                    if (string.Compare(new string(name), text) == 0)
                    {
                        WinGame(start);
                    }
                    if (win == true || lose == true)
                    {
                        ctimermy.UpdateTimer();
                    }
                    if (points > 30)
                    {
                        round_speed -= 5f;
                    }


                }
            }
            BeginDrawing();
            ClearBackground(Color.RAYWHITE);

            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            // replace if to switch ( even for other ones ) [*]
            if (Windows == 1)
            {
                if (pause == false)
                {
                    DrawTexture(texture, screenWidth / 2 - texture.width / 2, screenHeight / 2 - texture.height / 2, Color.WHITE);
                    if (updatetimer.RemainTime() >= 6f)
                        DrawText("Time: " + ((int)updatetimer.GetCurrentTime()).ToString(), screenWidth / 2 - 225, screenHeight / 4 + 320, 20, Color.MAROON);
                    else
                        DrawText("Time: " + ((int)updatetimer.GetCurrentTime()).ToString(), screenWidth / 2 - 225, screenHeight / 4 + 320, 20, Color.WHITE);

                    DrawText("Points: " + points.ToString(), screenWidth / 2 + 170 + (points.ToString().Length - 5), screenHeight / 4 + 323, 20, Color.WHITE);

                    Vector2 fontPosition3;
                    fontPosition3.X = screenWidth / 2.0f - MeasureTextEx(GetFontDefault(), title, 20, 3).X / 2;
                    fontPosition3.Y = 15;
                    //DrawText(text, screenWidth/2 + (int)w, 50, 20, Color.MAROON);
                    DrawTextEx(GetFontDefault(), title, fontPosition3, 20, 3, Color.GRAY);

                    Vector2 fontPosition1;
                    fontPosition1.X = screenWidth / 2.0f - MeasureTextEx(GetFontDefault(), text, 20, 3).X / 2;
                    fontPosition1.Y = 100;
                    //DrawText(text, screenWidth/2 + (int)w, 50, 20, Color.MAROON);
                    DrawTextEx(GetFontDefault(), text, fontPosition1, 20, 3, Color.BLACK);
                    //DrawRectangleRec(textBox, Color.LIGHTGRAY);
                    if (new string(name).Substring(0, letterCount) == text.Substring(0, letterCount))
                    {
                        DrawText(text.Substring(letterCount, text.Length - letterCount), screenWidth / 2 - 220 + MeasureText(new string(name), 20), screenHeight / 4 + 10, 20, Color.GRAY);
                    }
                    else
                    {
                        DrawText(text.Substring(letterCount, text.Length - letterCount), screenWidth / 2 - 220 + 8 + MeasureText(new string(name), 20), screenHeight / 4 + 10, 20, Color.RED);
                    }
                    DrawText(new string(name), screenWidth / 2 - 220, screenHeight / 4 + 10, 20, Color.BLACK);


                    if (win == true && ctimermy.TimerDone() == false)
                    {
                        string won_string = "you won, next round in ";
                        fontPosition2.X = screenWidth / 2.0f - MeasureTextEx(GetFontDefault(), won_string + new string(((int)ctimermy.GetCurrentTime()).ToString()), 20, 3).X / 2 + 10;
                        fontPosition2.Y = screenHeight / 2;
                        //DrawText(text, screenWidth/2 + (int)w, 50, 20, Color.MAROON);
                        DrawTextEx(GetFontDefault(), won_string + new string(((int)ctimermy.GetCurrentTime()).ToString()), fontPosition2, 20, 3, Color.BEIGE);
                        //DrawRectangleRec(textBox, Color.GREEN);
                        //DrawText("you win !", screenWidth/2 - 65, screenHeight - 50, 40, Color.MAROON);
                    }
                    else if (lose == true && ctimermy.TimerDone() == false)
                    {
                        string won_string2 = "you lose, next round in ";
                        fontPosition4.X = screenWidth / 2.0f - MeasureTextEx(GetFontDefault(), won_string2 + new string(((int)ctimermy.GetCurrentTime()).ToString()), 20, 3).X / 2 + 10;
                        fontPosition4.Y = screenHeight / 2;
                        //DrawText(text, screenWidth/2 + (int)w, 50, 20, Color.MAROON);
                        DrawTextEx(GetFontDefault(), won_string2 + new string(((int)ctimermy.GetCurrentTime()).ToString()), fontPosition4, 20, 3, Color.BEIGE);
                        //DrawRectangleRec(textBox, Color.RED);
                        //DrawText("you win !", screenWidth/2 - 65, screenHeight - 50, 40, Color.MAROON);
                    }
                    else
                    {
                        if (win == true)
                        {
                            win = false;
                            start = false;
                        }
                        else if (lose == true)
                        {
                            lose = false;
                            start = false;
                        }
                    }
                    if (mouseOnText)
                    {
                        if (letterCount < MaxInputChars)
                        {
                            // Draw blinking underscore char
                            if ((framesCounter / 20 % 2) == 0)
                            {
                                DrawText(
                                    "|",
                                    screenWidth / 2 - 220 + MeasureText(new string(name), 20),
                                    screenHeight / 4 + 8,
                                    25,
                                    //Color.MAROON
                                    Color.WHITE
                                );
                            }
                        }
                    }


                }
            }
            else if (Windows == 2)
            {
                for (int a = 0; a < Utils.RuntimeStatusList.Count; a++)
                {
                    Vector2 fontPosition_score;
                    string score_text_sample = Utils.RuntimeStatusList[a].play_name + " - " + Utils.RuntimeStatusList[a].text + " -> " + ((int)Utils.RuntimeStatusList[a].end_time).ToString();
                    fontPosition_score.X = screenWidth / 2.0f - MeasureTextEx(GetFontDefault(), score_text_sample, 20, 3).X / 2;
                    fontPosition_score.Y = (a * 50) + 15;
                    //DrawText(text, screenWidth/2 + (int)w, 50, 20, Color.MAROON);
                    DrawTextEx(GetFontDefault(), score_text_sample, fontPosition_score, 20, 3, Color.GRAY);
                }
                if (IsKeyDown(KeyboardKey.KEY_A))
                {
                    Windows = 1;
                    pause = false;
                }
            }

            EndDrawing();
        }


        // De-Initialization
        //--------------------------------------------------------------------------------------
        CloseWindow();
        //--------------------------------------------------------------------------------------

        return 0;
    }
}