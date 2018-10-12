using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

[Serializable]
public struct PlayerSettings
{   //start at line 11 to make it easier to count, just subtract 10 from final line
    public Point colliderOffset;
    public Point colliderSize;
    public float terminalVelocity;
    public float maxHealth;
    public float maxShield;
    public float maxEnergy;
    public float invulnerableMaxTime;
    public float energyUsableMargin;
    public float minSqrDetectDistance;
    public Rectangle boundingBox;
    public bool cameraLerpSetting;
    public float cameraLerpXSpeed;
    public float cameraLerpYSpeed;
    public bool smartCamera;
    public float shieldRechargeRate;
    public float energyRechargeRate;
    public float healthRechargeRate;
    public float potionRechargeRate;
    public float potionRechargeTime;
    public int maxNumberOfPotions;
    public float shieldHealingPercentage;
    public float shockwaveEffectiveDistance;
    public float shockwaveStunTime;
    public Vector2 shockwaveKnockback;
    public float speed;
    public float jumpTimeMax;
    public float idleTimeMax;
    public float[] animationSpeed;
    public int[] animationFrames;
    public int[] characterWidth;
    public float analogCutoff;
    public float shieldAnimationSpeed;
    public float superShockwaveHoldtime;
    public float walljumpHoldtime;
    public Vector2 attackDamage;
    public float critChance;
    public Vector2 critMultiplier;
    public string timerAccuracy;
    public bool cameraShakeSetting;
    public bool cameraSwingSetting;
    public float swingDuration;
    public float swingMagnitude;
    public Point attackColliderSize;
    public int attackFrame;
    public bool fullscreen;
    public float screenScale;
    public Point resolution;
    public bool vSync;
    public bool easyWalljumpMode;
    public float masterAudioLevel;
    public float musicLevel;
    public float soundEffectsLevel;
    public Keys attackKey;
    public Keys shockwaveKey;
    public Keys shieldKey;
    public Keys jumpKey;
    public Keys upKey;
    public Keys downKey;
    public Keys leftKey;
    public Keys rightKey;
    public Keys rollKey;
    public Keys potionKey;
    public Keys useKey;
    public Keys altAttackKey;
    public Keys altShockwaveKey;
    public Keys altShieldKey;
    public Keys altJumpKey;
    public Keys altUpKey;
    public Keys altDownKey;
    public Keys altLeftKey;
    public Keys altRightKey;
    public Keys altRollKey;
    public Keys altPotionKey;
    public Keys altUseKey;
    public Buttons GPattackKey;
    public Buttons GPshockwaveKey;
    public Buttons GPshieldKey;
    public Buttons GPjumpKey;
    public Buttons GPupKey;
    public Buttons GPdownKey;
    public Buttons GPleftKey;
    public Buttons GPrightKey;
    public Buttons GProllKey;
    public Buttons GPpotionKey;
    public Buttons GPuseKey;
    public int characterHeight;
    public int debug;
    public bool lighting;
    public Rectangle playerLight;
    public Rectangle shieldLight;

    //start at line 11 to make it easier to count, just subtract 10 from final line
    const int numberOfVariables = 90;


    public PlayerSettings(bool useDefaults)
	{
        if (useDefaults)
        {
            //lines beginning with ; are ignored
            //delete this file and relaunch to return everything to defaults

            //SETTINGS

            //KEY BINDS
            //a list of all available keys can be found here:
            // https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx
            attackKey = Keys.Enter;
            altAttackKey = Keys.Enter;
            shockwaveKey = Keys.E;
            altShockwaveKey = Keys.E;
            shieldKey = Keys.Q;
            altShieldKey = Keys.Q;
            jumpKey = Keys.Space;
            altJumpKey = Keys.Space;
            upKey = Keys.W;
            altUpKey = Keys.Up;
            downKey = Keys.S;
            altDownKey = Keys.Down;
            leftKey = Keys.A;
            altLeftKey = Keys.Left;
            rightKey = Keys.D;
            altRightKey = Keys.Right;
            rollKey = Keys.LeftShift;
            altRollKey = Keys.LeftShift;
            potionKey = Keys.F;
            altPotionKey = Keys.F;
            useKey = Keys.R;
            altUseKey = Keys.R;

            GPattackKey = Buttons.RightTrigger;
            GPshockwaveKey = Buttons.RightShoulder;
            GPshieldKey = Buttons.LeftTrigger;
            GPjumpKey = Buttons.A;
            GPupKey = Buttons.DPadUp;
            GPdownKey = Buttons.DPadDown;
            GPleftKey = Buttons.DPadLeft;
            GPrightKey = Buttons.DPadRight;
            GProllKey = Buttons.X;
            GPpotionKey = Buttons.B;
            GPuseKey = Buttons.Y;


            //CAMERA SETTINGS
            //The camera will move when the player leaves this area on the screen
            //{int X location, int Y location, int Width, int Height}
            //where location refers to the center of the rectangle
            boundingBox = Rectangle.Empty;

            //Do you want the camera to smoothly trail the player?
            cameraLerpSetting = true;

            //Do you want to use the smart camera (as opposed to just a plain bounding box)?
            smartCamera = true;

            //How fast should the camera lerp?
            cameraLerpXSpeed = 2f;
            cameraLerpYSpeed = 5f;

            //Turn off camera "swing" (the motion the camera makes when you attack and miss)
            cameraSwingSetting = false;
            
            //How far should the camera swing?
            swingMagnitude = 10f;

            //How long should that swing take (this is the time it takes to travel the above pixels,
            //the time it takes to return to the normal camera position is double this number) (seconds)
            swingDuration = 0.1f;

            //Turn off camera shake
            cameraShakeSetting = true;


            //VIDEO SETTINGS
            //toggles windowed/fullscreen mode
            fullscreen = false;

            //the scale of each individual pixel in-game
            screenScale = 0;

            //how big is the game window (0,0 will result in a fullscreen borderless window)
            resolution = Point.Zero;

            vSync = false;
            
            // determines if lighting is enabled or not. disabling this can greatly improve performance.
            lighting = true;
            easyWalljumpMode = false;

            //AUDIO SETTINGS
             masterAudioLevel = 100f;
                   musicLevel = 100f;
            soundEffectsLevel = 100f;


            //MISC SETTINGS
            //How far does the thumbstick need to be moved before input is registered?
            analogCutoff = 0.1f;

            //How accurate is the timer (digits after the seconds' decimal)
            timerAccuracy = "00.00";

            //how long before the player is considered idle (seconds)
            idleTimeMax = 30f;

            //This is the time it takes to "charge" the super shockwave (seconds)
            //Basically, how long you have to hold the shockwaveKey to use
            superShockwaveHoldtime = 0.15f;

            //The amount of time the player can hold down the left or right movement key
            //during a wall jump before they drift away from the wall (seconds)
            walljumpHoldtime = 0.1f;

            //minimum distance(squared) for an enemy health bar to appear(bosses override this)
            minSqrDetectDistance = 1000000f;


            //CHEATS
            //self-explanatory
            attackDamage = new Vector2(20, 30);
            critChance = 0.075f;
            critMultiplier = new Vector2(2.5f, 3f);

            //run&jump speed
            speed = 275f;
            //The maximum speed you can travel in any given direction
            terminalVelocity = 800f;
            //how long your jumps will maintain jump velocity (seconds)
            jumpTimeMax = 0.25f;


            //highly unrecommended to not mess with these unless you really know what you're doing
            //collider size and placement relative to the 128x128 player sprite
            colliderOffset = new Point(54, 63);
            colliderSize = new Point(20, 48);
            //location of the lights inside the player texture
            playerLight = new Rectangle(2176, 3584, 256, 256);
            shieldLight = new Rectangle(1920, 3584, 256, 256);

            //this is the size of the rectangle used as the main attack hitbox
            attackColliderSize = new Point(40, 30);

            //on the start of which frame of animation will the attack be triggered
            attackFrame = 1;

            //self-explanatory
            maxHealth = 100f;
            maxShield = 50f;
            maxEnergy = 50f;

            //distance at which shockwave has no power
            shockwaveEffectiveDistance = 100f;
            //shockwave multipliers
            shockwaveStunTime = 0.5f;
            shockwaveKnockback = new Vector2(2f, -1f);

            //this how the player won't take damage after previously taking damage (seconds)
            invulnerableMaxTime = 0.1f;

            //self-explanatory (per second)
            shieldRechargeRate = 5f;        //2f //4f
            energyRechargeRate = 10f;       //5f //10f
            healthRechargeRate = 0.5f;      //0f
            potionRechargeRate = 15f;
            potionRechargeTime = 3f;
            maxNumberOfPotions = 3;

            //this is the percentage of the damage absorbed by the shield that is converted back into health
            //0.25 means 25% of damage absorbed by the shield is converted to health (if 20 damage is absorbed, 5 damage is healed)
            shieldHealingPercentage = 0.25f;

            //how full the energy bar has to be before allowing you to use (95f == 95%)
            energyUsableMargin = 0.95f;

            //ANIMATION SETTINGS
            //the amount of time that is allowed to pass before the animator displays the next frame (seconds)
            //(for each animation listed below)
            animationSpeed = new float[47];
            for (int i = 0; i < animationSpeed.Length; i++)
            { animationSpeed[i] = 0.1f; }
            for (int i = 0; i <= 6; i++)
            { animationSpeed[i] = 0.125f; }
            for (int i = 7; i <= 8; i++)
            { animationSpeed[i] = 0.1f; }
            for (int i = 9; i <= 14; i++)
            { animationSpeed[i] = 0.125f; }
            for (int i = 17; i <= 22; i++)  //attacks
            { animationSpeed[i] = 0.075f; }
            for (int i = 29; i <= 34; i++)  //post-attacks
            { animationSpeed[i] = 0.075f; }
            for (int i = 23; i <= 26; i++)
            { animationSpeed[i] = 0.05f; }
            for (int i = 39; i <= 40; i++)
            { animationSpeed[i] = 0.05f; }
            for (int i = 41; i <= 46; i++)
            { animationSpeed[i] = 0.075f; }

            // 0 is 1 frame, 1 is 2 frames, etc
            //the number of frames in each animation, only edit this if you are remaking the default spritesheet
            animationFrames = new int[47];
            for (int i = 0; i < animationFrames.Length; i++)
            { animationFrames[i] = 0; }
            animationFrames[00] = 3;
            animationFrames[01] = 15;
            animationFrames[02] = 3;
            animationFrames[03] = 15;
            animationFrames[04] = 15;
            animationFrames[05] = 15;
            animationFrames[06] = 15;
            animationFrames[07] = 9;
            animationFrames[08] = 9;
            animationFrames[09] = 5;
            animationFrames[10] = 5;
            animationFrames[11] = 0;
            animationFrames[12] = 0;
            animationFrames[13] = 1;
            animationFrames[14] = 1;
            animationFrames[15] = 0;
            animationFrames[16] = 0;
            animationFrames[17] = 3;
            animationFrames[18] = 3;
            animationFrames[19] = 3;
            animationFrames[20] = 3;
            animationFrames[21] = 3;
            animationFrames[22] = 3;
            animationFrames[23] = 3;
            animationFrames[24] = 3;
            animationFrames[25] = 1;
            animationFrames[26] = 1;
            animationFrames[27] = 1;
            animationFrames[28] = 1;
            animationFrames[29] = 1;
            animationFrames[30] = 1;
            animationFrames[31] = 1;
            animationFrames[32] = 1;
            animationFrames[33] = 1;
            animationFrames[34] = 1;
            animationFrames[35] = 6;
            animationFrames[36] = 6;
            animationFrames[37] = 7;
            animationFrames[38] = 7;
            animationFrames[39] = 4;
            animationFrames[40] = 4;
            animationFrames[41] = 0;
            animationFrames[42] = 0;
            animationFrames[43] = 0;
            animationFrames[44] = 0;
            animationFrames[45] = 5;
            animationFrames[46] = 5;

            //the amount of time that is allowed to pass before the shield animator displays the next frame (seconds)
            //NOTE: there is no variable for the number of frames in the shield animation, as the shield animator
            //uses the width of the shield sprite to determine when to loop.
            shieldAnimationSpeed = 0.05f;

            //FONT SETTINGS
            //the height of each character in the font spritesheet (pixels)
            characterHeight = 12;
            //the width of each character in the font spritesheet (pixels)
            characterWidth = new int[100];
            int[] twosCol = { 18, 44, 62, 82, 83, 86 };
            int[] threesCol = { 47, 84, 85, 87, 90, 91, 93 };
            int[] foursCol = { 1, 45, 55, 70, 71, 80, 81, 94 };
            int[] fivesCol = { 41, 88, 92 };
            int[] sixesCol = { 5, 14, 15, 19, 21, 28, 53, 54, 57, 59, 63, 65, 67, 72, 73, 75, 76, 77 };
            int[] sevensCol = { 0, 2, 3, 4, 6, 7, 8, 9, 11, 12, 13, 16, 17, 20, 23, 24, 25, 26, 27, 30, 35, 36, 37, 38, 39, 40, 42, 43, 46, 49, 50, 51, 52, 56, 60, 61, 74, 78 };
            int[] eightsCol = { 10, 29, 31, 32, 33, 34, 48, 68, 69, 99 };
            int[] ninesCol = { 64, 79 };
            int[] tensCol = { 22, 58, 66, 89, 95 };
            int[] elevenCol = { };
            int[] twelveCol = { 32, 96, 97, 98 };
            foreach (int i in characterWidth)
            { characterWidth[i] = 12; }
            foreach (int i in twosCol)
            { characterWidth[i] = 2; }
            foreach (int i in threesCol)
            { characterWidth[i] = 3; }
            foreach (int i in foursCol)
            { characterWidth[i] = 4; }
            foreach (int i in fivesCol)
            { characterWidth[i] = 5; }
            foreach (int i in sixesCol)
            { characterWidth[i] = 6; }
            foreach (int i in sevensCol)
            { characterWidth[i] = 7; }
            foreach (int i in eightsCol)
            { characterWidth[i] = 8; }
            foreach (int i in ninesCol)
            { characterWidth[i] = 9; }
            foreach (int i in tensCol)
            { characterWidth[i] = 10; }
            foreach (int i in elevenCol)
            { characterWidth[i] = 11; }
            foreach (int i in twelveCol)
            { characterWidth[i] = 12; }


            //DEBUG MODE
            debug = 0;
        }
        else
        {
            //lines beginning with ; are ignored
            //delete this file and relaunch to return everything to defaults

            //KEY BINDS
            //a list of all available keys can be found here:
            // https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx
            attackKey = Keys.Enter;
            altAttackKey = Keys.Enter;
            shockwaveKey = Keys.E;
            altShockwaveKey = Keys.E;
            shieldKey = Keys.Q;
            altShieldKey = Keys.Q;
            jumpKey = Keys.Space;
            altJumpKey = Keys.Space;
            upKey = Keys.W;
            altUpKey = Keys.Up;
            downKey = Keys.S;
            altDownKey = Keys.Down;
            leftKey = Keys.A;
            altLeftKey = Keys.Left;
            rightKey = Keys.D;
            altRightKey = Keys.Right;
            rollKey = Keys.LeftShift;
            altRollKey = Keys.LeftShift;
            potionKey = Keys.F;
            altPotionKey = Keys.F;
            useKey = Keys.R;
            altUseKey = Keys.R;

            GPattackKey = Buttons.RightTrigger;
            GPshockwaveKey = Buttons.RightShoulder;
            GPshieldKey = Buttons.LeftTrigger;
            GPjumpKey = Buttons.A;
            GPupKey = Buttons.DPadUp;
            GPdownKey = Buttons.DPadDown;
            GPleftKey = Buttons.DPadLeft;
            GPrightKey = Buttons.DPadRight;
            GProllKey = Buttons.X;
            GPpotionKey = Buttons.B;
            GPuseKey = Buttons.Y;


            //CAMERA SETTINGS
            //The camera will move when the player leaves this area on the screen
            //{int X location, int Y location, int Width, int Height}
            //where location refers to the center of the rectangle
            boundingBox = Rectangle.Empty;

            //Do you want the camera to smoothly trail the player?
            cameraLerpSetting = false;

            //Do you want to use the smart camera (as opposed to just a plain bounding box)?
            smartCamera = false;

            //How fast should the camera lerp?
            cameraLerpXSpeed = 0f;
            cameraLerpYSpeed = 0f;

            //Turn off camera "swing" (the motion the camera makes when you attack and miss)
            cameraSwingSetting = false;

            //How far should the camera swing?
            swingMagnitude = 0f;

            //How long should that swing take (this is the time it takes to travel the above pixels,
            //the time it takes to return to the normal camera position is double this number) (seconds)
            swingDuration = 0f;

            //Turn off camera shake
            cameraShakeSetting = false;


            //VIDEO SETTINGS
            //toggles windowed/fullscreen mode
            fullscreen = false;

            //the scale of each individual pixel in-game
            screenScale = 0;

            //how big is the game window (0,0 will result in a fullscreen borderless window)
            resolution = Point.Zero;

            vSync = false;
            
            // determines if lighting is enabled or not. disabling this can greatly improve performance.
            lighting = false;

            easyWalljumpMode = false;


            //AUDIO SETTINGS
            masterAudioLevel = 0f;
                   musicLevel = 0f;
            soundEffectsLevel = 0f;


            //MISC SETTINGS
            //How far does the thumbstick need to be moved before input is registered?
            analogCutoff = 0f;

            //How accurate is the timer (digits after the seconds' decimal)
            timerAccuracy = "00.";

            //how long before the player is considered idle (seconds)
            idleTimeMax = 0f;

            //This is the time it takes to "charge" the super shockwave (seconds)
            //Basically, how long you have to hold the shockwaveKey to use
            superShockwaveHoldtime = 0f;

            //The amount of time the player can hold down the left or right movement key
            //during a wall jump before they drift away from the wall (seconds)
            walljumpHoldtime = 0f;

            //minimum distance(squared) for an enemy health bar to appear(bosses override this)
            minSqrDetectDistance = 0f;


            //CHEATS
            //self-explanatory
            attackDamage = new Vector2(20, 30);
            critChance = 0f;
            critMultiplier = Vector2.Zero;

            //run&jump speed
            speed = 0f;
            //The maximum speed you can travel in any given direction
            terminalVelocity = 0f;
            //how long your jumps will maintain jump velocity (seconds)
            jumpTimeMax = 0f;


            //highly unrecommended to not mess with these unless you really know what you're doing
            //collider size and placement relative to the 128x128 player sprite
            colliderSize = colliderOffset = Point.Zero;
            //location of the lights inside the player texture
            playerLight = shieldLight = Rectangle.Empty;

            //this is the size of the rectangle used as the main attack hitbox
            attackColliderSize = Point.Zero;

            //on the start of which frame of animation will the attack be triggered
            attackFrame = 0;

            //self-explanatory
            maxHealth = 0f;
            maxShield = 0f;
            maxEnergy = 0f;

            //distance at which shockwave has no power
            shockwaveEffectiveDistance = 0f;
            //shockwave multipliers
            shockwaveStunTime = 0f;
            shockwaveKnockback = new Vector2(0f, 0f);

            //this how the player won't take damage after previously taking damage (seconds)
            invulnerableMaxTime = 0f;

            //self-explanatory (per second)
            shieldRechargeRate = 0f;        //2f //4f
            energyRechargeRate = 0f;       //5f //10f
            healthRechargeRate = 0f;      //0f
            potionRechargeRate = 0f;
            potionRechargeTime = 0f;
            maxNumberOfPotions = 0;

            //this is the percentage of the damage absorbed by the shield that is converted back into health
            //0.25 means 25% of damage absorbed by the shield is converted to health (if 20 damage is absorbed, 5 damage is healed)
            shieldHealingPercentage = 0f;

            //how full the energy bar has to be before allowing you to use (95f == 95%)
            energyUsableMargin = 0f;





            //ANIMATION SETTINGS
            //the amount of time that is allowed to pass before the animator displays the next frame (seconds)
            //(for each animation listed below)
            animationSpeed = new float[0];
            for (int i = 0; i < animationSpeed.Length; i++)
            { animationSpeed[i] = 0f; }

            // 0 is 1 frame, 1 is 2 frames, etc
            //the number of frames in each animation, only edit this if you are remaking the default spritesheet
            animationFrames = new int[0];
            for (int i = 0; i < animationFrames.Length; i++)
            { animationFrames[i] = 0; }

            //the amount of time that is allowed to pass before the shield animator displays the next frame (seconds)
            //NOTE: there is no variable for the number of frames in the shield animation, as the shield animator
            //uses the width of the shield sprite to determine when to loop.
            shieldAnimationSpeed = 0.05f;

            //FONT SETTINGS
            //the height of each character in the font spritesheet (pixels)
            characterHeight = 12;
            //the width of each character in the font spritesheet (pixels)
            characterWidth = new int[0];
            for (int i = 0; i < characterWidth.Length; i++)
            { characterWidth[i] = 0; }

            //DEBUG MODE
            debug = 1;
        }
    }

    public PlayerSettings(PlayerSettings settings)
    { this = settings; }

    public PlayerSettings(string filename)
    {
        this = new PlayerSettings(true);
        this.Load(filename);
    }

    public void Save(string filename)
    {
        Irbis.Irbis.WriteLine("saving " + filename + "...");
        StreamWriter writer = new StreamWriter(filename);

        //do writing here
        //;lines beginning with ; are ignored
        writer.WriteLine(";lines beginning with \";\" and empty lines are ignored");
        writer.WriteLine(";delete this file and relaunch to return everything to defaults");
        writer.WriteLine("");
        writer.WriteLine("");


        writer.WriteLine(";SETTINGS");
        writer.WriteLine(";KEY BINDS");
        writer.WriteLine(";a list of all available keys can be found here:");
        writer.WriteLine("; https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx");
        writer.WriteLine("attackKey=" + attackKey);
        writer.WriteLine("altAttackKey=" + altAttackKey);
        writer.WriteLine("");
        writer.WriteLine("shockwaveKey=" + shockwaveKey);
        writer.WriteLine("altShockwaveKey=" + altShockwaveKey);
        writer.WriteLine("");
        writer.WriteLine("shieldKey=" + shieldKey);
        writer.WriteLine("altShieldKey=" + altShieldKey);
        writer.WriteLine("");
        writer.WriteLine("jumpKey=" + jumpKey);
        writer.WriteLine("altJumpKey=" + altJumpKey);
        writer.WriteLine("");
        writer.WriteLine("upKey=" + upKey);
        writer.WriteLine("altUpKey=" + altUpKey);
        writer.WriteLine("");
        writer.WriteLine("downKey=" + downKey);
        writer.WriteLine("altDownKey=" + altDownKey);
        writer.WriteLine("");
        writer.WriteLine("leftKey=" + leftKey);
        writer.WriteLine("altLeftKey=" + altLeftKey);
        writer.WriteLine("");
        writer.WriteLine("rightKey=" + rightKey);
        writer.WriteLine("altRightKey=" + altRightKey);
        writer.WriteLine("");
        writer.WriteLine("rollKey=" + rollKey);
        writer.WriteLine("altRollKey=" + altRollKey);
        writer.WriteLine("");
        writer.WriteLine("potionKey=" + potionKey);
        writer.WriteLine("altPotionKey=" + altPotionKey);
        writer.WriteLine("");
        writer.WriteLine("useKey=" + useKey);
        writer.WriteLine("altUseKey=" + altUseKey);
        writer.WriteLine("");
        writer.WriteLine(";a list of all available gamepad (controller) buttons can be found here:");
        writer.WriteLine("; https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.buttons.aspx");
        writer.WriteLine("GPattackKey=" + GPattackKey);
        writer.WriteLine("GPshockwaveKey=" + GPshockwaveKey);
        writer.WriteLine("GPshieldKey=" + GPshieldKey);
        writer.WriteLine("GPjumpKey=" + GPjumpKey);
        writer.WriteLine("GPupKey=" + GPupKey);
        writer.WriteLine("GPdownKey=" + GPdownKey);
        writer.WriteLine("GPleftKey=" + GPleftKey);
        writer.WriteLine("GPrightKey=" + GPrightKey);
        writer.WriteLine("GProllKey=" + GProllKey);
        writer.WriteLine("GPpotionKey=" + GPpotionKey);
        writer.WriteLine("GPuseKey=" + GPuseKey);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";CAMERA SETTINGS");
        writer.WriteLine(";The camera will move when the player leaves this area on the screen");
        writer.WriteLine(";{int X location, int Y location, int Width, int Height})");
        writer.WriteLine(";where location refers to the center of the rectangle");
        writer.WriteLine("boundingBox={X:" + boundingBox.Center.X + " Y:" + boundingBox.Center.Y + " Width:" + boundingBox.Width + " Height:" + boundingBox.Height + "}");
        writer.WriteLine("");
        writer.WriteLine(";Do you want the camera to smoothly trail the player?");
        writer.WriteLine("cameraLerp=" + cameraLerpSetting);
        writer.WriteLine("");
        writer.WriteLine(";Do you want to use the smart camera (as opposed to just a plain bounding box)?");
        writer.WriteLine("smartCamera=" + smartCamera);
        writer.WriteLine("");
        writer.WriteLine(";How fast should the camera lerp?");
        writer.WriteLine("cameraLerpXSpeed=" + cameraLerpXSpeed);
        writer.WriteLine("cameraLerpYSpeed=" + cameraLerpYSpeed);
        writer.WriteLine("");
        writer.WriteLine(";Turn off camera \"swing\" (the motion the camera makes when you attack and miss)");
        writer.WriteLine("cameraSwingSetting=" + cameraSwingSetting);
        writer.WriteLine("");
        writer.WriteLine(";How far should the camera swing? (pixels)");
        writer.WriteLine("swingMagnitude=" + swingMagnitude);
        writer.WriteLine("");
        writer.WriteLine(";How long should that swing take (this is the time it takes to travel the above pixels,");
        writer.WriteLine(";the time it takes to return to the normal camera position is double this number) (seconds)");
        writer.WriteLine("swingDuration=" + swingDuration);
        writer.WriteLine("");
        writer.WriteLine(";Turn off camera shake");
        writer.WriteLine("cameraShakeSetting=" + cameraShakeSetting);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";VIDEO SETTINGS");
        writer.WriteLine(";toggles windowed/fullscreen mode");
        writer.WriteLine("fullscreen=" + fullscreen);
        writer.WriteLine("");
        writer.WriteLine(";the scale of each individual pixel in-game");
        writer.WriteLine("screenScale=" + screenScale);
        writer.WriteLine("");
        writer.WriteLine(";how big is the game window (0,0 will result in a fullscreen borderless window)");
        writer.WriteLine("resolution=" + resolution);
        writer.WriteLine("");
        writer.WriteLine("vSync=" + vSync);
        writer.WriteLine("");
        writer.WriteLine(";determines if lighting is enabled or not. disabling this can greatly improve performance");
        writer.WriteLine("lighting=" + lighting);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";AUDIO SETTINGS");
        writer.WriteLine("masterAudioLevel=" + masterAudioLevel);
        writer.WriteLine("musicLevel=" + musicLevel);
        writer.WriteLine("soundEffectsLevel=" + soundEffectsLevel);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";MISC SETTINGS");
        writer.WriteLine(";How far does the thumbstick need to be moved before input is registered?");
        writer.WriteLine("analogCutoff=" + analogCutoff);
        writer.WriteLine("");
        writer.WriteLine(";How accurate is the timer (digits after the seconds' decimal)");
        writer.WriteLine("timerAccuracy=" + (timerAccuracy.Length - 3));
        writer.WriteLine("");
        writer.WriteLine(";How long before the player is considered idle (seconds)");
        writer.WriteLine("idleTimeMax=" + idleTimeMax);
        writer.WriteLine("");
        writer.WriteLine(";This is the time it takes to \"charge\" the super shockwave (seconds)");
        writer.WriteLine(";Basically, how long you have to hold the shockwaveKey to use");
        writer.WriteLine("superShockwaveHoldtime=" + superShockwaveHoldtime);
        writer.WriteLine("");
        writer.WriteLine(";The amount of time the player can hold down the left or right movement key");
        writer.WriteLine(";during a wall jump before they drift away from the wall (seconds)");
        writer.WriteLine("walljumpHoldtime=" + walljumpHoldtime);
        writer.WriteLine("");
        writer.WriteLine("");
        writer.WriteLine(";A mode that makes wall jumping easier");
        writer.WriteLine("easyWalljumpMode=" + easyWalljumpMode);
        writer.WriteLine("");
        writer.WriteLine(";Minimum distance(squared) for an enemy health bar to appear(bosses override this)");
        writer.WriteLine("minSqrDetectDistance=" + minSqrDetectDistance);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";CHEATS");
        writer.WriteLine(";self-explanatory");
        writer.WriteLine("attackDamage=" + attackDamage);
        writer.WriteLine("critChance=" + critChance);
        writer.WriteLine("critMultiplier=" + critMultiplier);
        writer.WriteLine("");

        writer.WriteLine(";run&jump speed");
        writer.WriteLine("speed=" + speed);
        writer.WriteLine(";The maximum speed you can travel in any given direction");
        writer.WriteLine("terminalVelocity=" + terminalVelocity);
        writer.WriteLine(";how long your jumps will maintain jump velocity (seconds)");
        writer.WriteLine("jumpTimeMax=" + jumpTimeMax);
        writer.WriteLine("");

        writer.WriteLine(";highly unrecommended to not mess with these unless you really know what you're doing");
        writer.WriteLine(";collider size and placement relative to the 128x128 player sprite");
        writer.WriteLine("colliderOffset=" + colliderOffset);
        writer.WriteLine("colliderSize=" + colliderSize);
        writer.WriteLine(";location of the lights inside the player texture");
        writer.WriteLine("playerLight=" + playerLight);
        writer.WriteLine("shieldLight=" + shieldLight);
        writer.WriteLine("");

        writer.WriteLine(";this is the size of the rectangle used as the main attack hitbox");
        writer.WriteLine("attackColliderSize=" + attackColliderSize);
        writer.WriteLine("");

        writer.WriteLine(";on the start of which frame of animation will the attack be triggered");
        writer.WriteLine("attackFrame=" + attackFrame);
        writer.WriteLine("");

        writer.WriteLine(";self-explanatory");
        writer.WriteLine("maxHealth=" + maxHealth);
        writer.WriteLine("maxShield=" + maxShield);
        writer.WriteLine("maxEnergy=" + maxEnergy);
        writer.WriteLine("");



        writer.WriteLine(";distance at which shockwave has no power");
        writer.WriteLine("shockwaveEffectiveDistance=" + shockwaveEffectiveDistance);
        writer.WriteLine(";shockwave multipliers");
        writer.WriteLine("shockwaveStunTime=" + shockwaveStunTime);
        writer.WriteLine("shockwaveKnockback=" + shockwaveKnockback);
        writer.WriteLine("");

        writer.WriteLine(";this how the player won't take damage after previously taking damage (seconds)");
        writer.WriteLine("invulnerableMaxTime=" + invulnerableMaxTime);
        writer.WriteLine("");

        writer.WriteLine(";these are only the base. they may be modified in-game by potions and other ways (per second)");
        writer.WriteLine("shieldRechargeRate=" + shieldRechargeRate);
        writer.WriteLine("energyRechargeRate=" + energyRechargeRate);
        writer.WriteLine("healthRechargeRate=" + healthRechargeRate);
        writer.WriteLine("potionRechargeRate=" + potionRechargeRate);
        writer.WriteLine("potionRechargeTime=" + potionRechargeTime);
        writer.WriteLine("maxNumberOfPotions=" + maxNumberOfPotions);
        writer.WriteLine("");

        writer.WriteLine(";this is the percentage of the damage absorbed by the shield that is converted back into health");
        writer.WriteLine(";0.25 means 25% of damage absorbed by the shield is converted to health (if 20 damage is absorbed, 5 damage is healed)");
        writer.WriteLine("shieldHealingPercentage=" + shieldHealingPercentage);
        writer.WriteLine("");

        writer.WriteLine(";how full the energy bar has to be before allowing you to use (95 == 95%)");
        writer.WriteLine("energyUsableMargin=" + energyUsableMargin);
        writer.WriteLine("");
        //writer.WriteLine("");

        writer.WriteLine(";ANIMATION SETTINGS");
        writer.WriteLine(";the amount of time that is allowed to pass before the animator displays the next frame (seconds)");
        writer.WriteLine(";(for each animation listed below)");
        for (int i = 00; i < animationSpeed.Length; i++)
        { writer.WriteLine("animationSpeed[" + i + "]=" + animationSpeed[i]); }
        writer.WriteLine("");


        writer.WriteLine(";0 is 1 frame, 1 is 2 frames, etc");
        writer.WriteLine(";the number of frames in each animation, only edit this if you are remaking the default spritesheet");
        for (int i = 00; i < animationFrames.Length; i++)
        { writer.WriteLine("animationFrames[" + i + "]=" + animationFrames[i]); }
        writer.WriteLine("");


        writer.WriteLine(";the amount of time that is allowed to pass before the shield animator displays the next frame (seconds)");
        writer.WriteLine(";NOTE: there is no variable for the number of frames in the shield animation, as the shield animator");
        writer.WriteLine(";uses the width of the shield sprite to determine when to loop.");
        writer.WriteLine("shieldAnimationSpeed=" + shieldAnimationSpeed);


        writer.WriteLine(";FONT SETTINGS");
        writer.WriteLine(";the height of each character in the font spritesheet (pixels)");
        writer.WriteLine("characterHeight=" + characterHeight);
        writer.WriteLine(";the width of each character in the font spritesheet (pixels)");
        for (int i = 00; i < 100; i++)
        { writer.WriteLine("characterWidth[" + i + "]=" + characterWidth[i]); }
        writer.WriteLine(";characterWidth[99] is used as SPACE, so it should remain blank on the spritesheet (it is the final character)");
        writer.WriteLine("");
        writer.WriteLine("");


        writer.WriteLine(";DEBUG MODE");
        writer.WriteLine("debug=" + debug);
        writer.WriteLine("");
        writer.WriteLine(";//END");




        writer.Close();
        Irbis.Irbis.WriteLine("save successful.");
    }

    public static void Save(Irbis.Irbis game, string filename)
    {
        PlayerSettings settings = new PlayerSettings(true);
        settings.attackKey = Irbis.Irbis.attackKey;
        settings.altAttackKey = Irbis.Irbis.altAttackKey;
        settings.shockwaveKey = Irbis.Irbis.shockwaveKey;
        settings.altShockwaveKey = Irbis.Irbis.altShockwaveKey;
        settings.shieldKey = Irbis.Irbis.shieldKey;
        settings.altShieldKey = Irbis.Irbis.altShieldKey;
        settings.jumpKey = Irbis.Irbis.jumpKey;
        settings.altJumpKey = Irbis.Irbis.altJumpKey;
        settings.upKey = Irbis.Irbis.upKey;
        settings.altUpKey = Irbis.Irbis.altUpKey;
        settings.downKey = Irbis.Irbis.downKey;
        settings.altDownKey = Irbis.Irbis.altDownKey;
        settings.leftKey = Irbis.Irbis.leftKey;
        settings.altLeftKey = Irbis.Irbis.altLeftKey;
        settings.rightKey = Irbis.Irbis.rightKey;
        settings.altRightKey = Irbis.Irbis.altRightKey;
        settings.rollKey = Irbis.Irbis.rollKey;
        settings.altRollKey = Irbis.Irbis.altRollKey;
        settings.potionKey = Irbis.Irbis.potionKey;
        settings.altPotionKey = Irbis.Irbis.altPotionKey;
        settings.useKey = Irbis.Irbis.useKey;
        settings.altUseKey = Irbis.Irbis.altUseKey;
        settings.GPattackKey = Irbis.Irbis.GPattackKey;
        settings.GPshockwaveKey = Irbis.Irbis.GPshockwaveKey;
        settings.GPshieldKey = Irbis.Irbis.GPshieldKey;
        settings.GPjumpKey = Irbis.Irbis.GPjumpKey;
        settings.GPupKey = Irbis.Irbis.GPupKey;
        settings.GPdownKey = Irbis.Irbis.GPdownKey;
        settings.GPleftKey = Irbis.Irbis.GPleftKey;
        settings.GPrightKey = Irbis.Irbis.GPrightKey;
        settings.GProllKey = Irbis.Irbis.GProllKey;
        settings.GPpotionKey = Irbis.Irbis.GPpotionKey;
        settings.GPuseKey = Irbis.Irbis.GPuseKey;
        if (Irbis.Irbis.boundingBox == Irbis.Irbis.DefaultBoundingBox)
        { settings.boundingBox = Rectangle.Empty; }
        else
        { settings.boundingBox = Irbis.Irbis.boundingBox; }
        settings.cameraLerpSetting = Irbis.Irbis.cameraLerpSetting;
        settings.smartCamera = Irbis.Irbis.smartCamera;
        settings.cameraLerpXSpeed = Irbis.Irbis.cameraLerpXSpeed;
        settings.cameraLerpYSpeed = Irbis.Irbis.cameraLerpYSpeed;
        settings.cameraSwingSetting = Irbis.Irbis.cameraSwingSetting;
        settings.swingMagnitude = Irbis.Irbis.swingMagnitude;
        settings.swingDuration = Irbis.Irbis.swingDuration;
        settings.cameraShakeSetting = Irbis.Irbis.cameraShakeSetting;
        settings.fullscreen = Irbis.Irbis.graphics.IsFullScreen;
        if (Irbis.Irbis.screenScale == Irbis.Irbis.DefaultScreenScale)
        { settings.screenScale = 0; }
        else
        { settings.screenScale = Irbis.Irbis.screenScale; }
        settings.resolution = Irbis.Irbis.tempResolution;
        settings.vSync = game.IsFixedTimeStep;
        settings.lighting = Irbis.Irbis.lightingEnabled;
        settings.easyWalljumpMode = Irbis.Irbis.easyWalljumpMode;
        settings.masterAudioLevel = Irbis.Irbis.masterAudioLevel;
        settings.musicLevel = Irbis.Irbis.musicLevel;
        settings.soundEffectsLevel = Irbis.Irbis.soundEffectsLevel;
        settings.timerAccuracy = Irbis.Irbis.timerAccuracy;
        settings.idleTimeMax = Irbis.Irbis.jamie.idleTimeMax;
        settings.superShockwaveHoldtime = Irbis.Irbis.jamie.superShockwaveHoldtime;
        settings.walljumpHoldtime = Irbis.Irbis.jamie.walljumpHoldtime;
        settings.minSqrDetectDistance = Irbis.Irbis.minSqrDetectDistance;
        settings.attackDamage = Irbis.Irbis.jamie.attackDamage;
        settings.critChance = Irbis.Irbis.jamie.critChance;
        settings.critMultiplier = Irbis.Irbis.jamie.critMultiplier;
        settings.speed = Irbis.Irbis.jamie.speed;
        settings.terminalVelocity = Irbis.Irbis.jamie.terminalVelocity;
        settings.jumpTimeMax = Irbis.Irbis.jamie.jumpTimeMax;

        settings.colliderOffset = (Irbis.Irbis.jamie.origin + Irbis.Irbis.jamie.standardCollider.Location.ToVector2()).ToPoint();
        settings.colliderSize = Irbis.Irbis.jamie.standardCollider.Size;
        settings.shieldLight = Irbis.Irbis.jamie.shieldLight;
        settings.playerLight = Irbis.Irbis.jamie.playerLight;

        settings.attackColliderSize = Irbis.Irbis.jamie.attackColliderSize;

        settings.attackFrame = Irbis.Irbis.jamie.attackFrame;

        settings.maxHealth = Irbis.Irbis.jamie.maxHealth;
        settings.maxShield = Irbis.Irbis.jamie.maxShield;
        settings.maxEnergy = Irbis.Irbis.jamie.maxEnergy;

        settings.shockwaveEffectiveDistance = Irbis.Irbis.jamie.shockwaveEffectiveDistance;
        settings.shockwaveStunTime = Irbis.Irbis.jamie.shockwaveStunTime;
        settings.shockwaveKnockback = Irbis.Irbis.jamie.shockwaveKnockback;
        settings.invulnerableMaxTime = Irbis.Irbis.jamie.invulnerableMaxTime;

        settings.analogCutoff = Irbis.Irbis.analogCutoff;
        settings.shieldRechargeRate = Irbis.Irbis.jamie.shieldRechargeRate;
        settings.energyRechargeRate = Irbis.Irbis.jamie.energyRechargeRate;
        settings.healthRechargeRate = Irbis.Irbis.jamie.baseHealing;
        settings.potionRechargeRate = Irbis.Irbis.jamie.potionRechargeRate;
        settings.potionRechargeTime = Irbis.Irbis.jamie.potionRechargeTime;
        settings.maxNumberOfPotions = Irbis.Irbis.jamie.maxNumberOfPotions;
        settings.shieldHealingPercentage = Irbis.Irbis.jamie.shieldHealingPercentage;
        settings.energyUsableMargin = Irbis.Irbis.jamie.energyUsableMargin;

        settings.animationSpeed = Irbis.Irbis.jamie.animationSpeed;
        settings.animationFrames = Irbis.Irbis.jamie.animationFrames;
        settings.shieldAnimationSpeed = Irbis.Irbis.jamie.shieldAnimationSpeed;
        settings.characterHeight = Irbis.Irbis.font.charHeight;
        settings.characterWidth = Irbis.Irbis.font.charWidth;

        settings.debug = Irbis.Irbis.debug;

        settings.Save(filename);    
    }

    public void Load(string filename)
    {
        Irbis.Irbis.WriteLine("loading " + filename + "...");

        int checker = 0;
        //filename = ".\\Content\\" + filename;

        Stream stream = TitleContainer.OpenStream(filename);
        StreamReader reader = new StreamReader(stream);

        string line = string.Empty;
        string errorVars = string.Empty;
        List<int> animationFrames = new List<int>();
        List<float> animationSpeed = new List<float>();
        List<int> characterWidth = new List<int>();

        while ((line = reader.ReadLine()) != null)
        {
            line.Trim();
            //line = line.ToLower();
            //if (debug0) { Console.WriteLine(" trimline: " + line); }
            if (line.Length >= 1 && !line[0].Equals('\u003b'))
            {
                string variable = string.Empty;             //'\u003d'://=
                string extra = string.Empty;
                string value = string.Empty;                //'\u003b'://;
                string statement = string.Empty;

                foreach (char c in line)
                {
                    if (!char.IsWhiteSpace(c))
                    { statement += c; }
                }

                int stage = 0;
                //stage 0  = START
                //stage 1  = encountered [ pulling contents of brackets
                //stage 2  = encountered ] dump info until reaching = (should always be next character)
                //stage 3  = encountered = pulling value
                //stage -1 = encountered ; FULL STOP
                //the below IF statements should be in reverse stage order (highest stage first)

                int intResult;
                float floatResult;
                Keys keyResult;
                Buttons buttonResult;
                bool boolResult;
                int extraResult;        //for the index of arrays

                foreach (char c in statement)               //'\u005b':[ //'\u005d':]
                {
                    if (stage == 3)
                    {
                        if (c.Equals('\u003b'))
                        { stage = -1; }
                        if (stage > 0)
                        { value += c; }
                    }
                    if (stage == 2)
                    {
                        if (c.Equals('\u003d'))
                        { stage = 3; }
                        else
                        { /*do nothing*/ }
                    }
                    if (stage == 1)
                    {
                        if (c.Equals('\u005d'))
                        { stage = 2; }
                        else
                        { extra += c; }
                    }
                    if (stage == 0)
                    {
                        if (c.Equals('\u003d'))
                        { stage = 3; }
                        else
                        {
                            if (c.Equals('\u005b'))
                            { stage = 1; }
                            else
                            { variable += c; }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(variable) && !string.IsNullOrWhiteSpace(value))
                {
                    switch (variable.ToLower())
                    {
                        case "critchance":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.critChance = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "speed":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.speed = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "jumptimemax":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.jumpTimeMax = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "idletimemax":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.idleTimeMax = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "maxhealth":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.maxHealth = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "maxshield":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.maxShield = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "maxenergy":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.maxEnergy = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "supershockwaveholdtime":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.superShockwaveHoldtime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "shockwaveeffectivedistance":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.shockwaveEffectiveDistance = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "shockwavestuntime":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.shockwaveStunTime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "invulnerablemaxtime":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.invulnerableMaxTime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "shieldrechargerate":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.shieldRechargeRate = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "energyrechargerate":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.energyRechargeRate = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "healthrechargerate":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.healthRechargeRate = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "energyusablemargin":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.energyUsableMargin = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "minsqrdetectdistance":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.minSqrDetectDistance = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "animationspeed":
                            if (float.TryParse(value, out floatResult))
                            {
                                if (int.TryParse(extra, out extraResult))
                                {
                                    if (extraResult < animationSpeed.Count)
                                    {
                                        animationSpeed[extraResult] = floatResult;
                                    }
                                    else
                                    {
                                        while (extraResult > animationSpeed.Count)
                                        {
                                            animationSpeed.Add(-1);
                                        }
                                        animationSpeed.Add(floatResult);
                                    }
                                }
                                else
                                {
                                    Irbis.Irbis.WriteLine("error: variable index \"" + variable + "[" + extra + "]\" could not be parsed");
                                    errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                                }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "[" + extra + "]\" could not be parsed");
                                errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                            }
                            break;
                        case "shieldanimationspeed":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.shieldAnimationSpeed = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "shieldhealingpercentage":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.shieldHealingPercentage = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "cameralerpxspeed":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.cameraLerpXSpeed = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "cameralerpyspeed":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.cameraLerpYSpeed = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "terminalvelocity":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.terminalVelocity = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "masteraudiolevel":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.masterAudioLevel = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "musiclevel":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.musicLevel = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "soundeffectslevel":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.soundEffectsLevel = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "potionrechargerate":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.potionRechargeRate = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "potionrechargetime":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.potionRechargeTime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "walljumpholdtime":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.walljumpHoldtime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "swingmagnitude":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.swingMagnitude = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "swingduration":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.swingDuration = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "analogcutoff":
                            if (float.TryParse(value, out floatResult))
                            {
                                this.analogCutoff = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;






                        case "collideroffset":                                                         //place new floats above
                            this.colliderOffset = PointParser(value);
                            if (this.colliderOffset == new Point(-0112, -0112))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "collidersize":
                            this.colliderSize = PointParser(value);
                            if (this.colliderSize == new Point(-0112, -0112))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "attackframe":
                            if (int.TryParse(value, out intResult))
                            {
                                this.attackFrame = intResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "animationframes":
                            if (int.TryParse(value, out intResult))
                            {
                                if (int.TryParse(extra, out extraResult))
                                {
                                    if (extraResult < animationFrames.Count)
                                    { animationFrames[extraResult] = intResult; }
                                    else
                                    {
                                        while (extraResult > animationFrames.Count)
                                        { animationFrames.Add(-1); }
                                        animationFrames.Add(intResult);
                                    }
                                }
                                else
                                {
                                    Irbis.Irbis.WriteLine("error: variable index \"" + variable + "[" + extra + "]\" could not be parsed");
                                    errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                                }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "[" + extra + "]\" could not be parsed");
                                errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                            }
                            break;
                        case "characterheight":
                            if (int.TryParse(value, out intResult))
                            {
                                this.characterHeight = intResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "characterwidth":
                            if (int.TryParse(value, out intResult))
                            {
                                if (int.TryParse(extra, out extraResult))
                                {
                                    if (extraResult < characterWidth.Count)
                                    {
                                        characterWidth[extraResult] = intResult;
                                    }
                                    else
                                    {
                                        while (extraResult > characterWidth.Count)
                                        {
                                            characterWidth.Add(-1);
                                        }
                                        characterWidth.Add(intResult);
                                    }
                                }
                                else
                                {
                                    Irbis.Irbis.WriteLine("error: variable index \"" + variable + "[" + extra + "]\" could not be parsed");
                                    errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                                }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "[" + extra + "]\" could not be parsed");
                                errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                            }

                            break;
                        case "screenscale":
                            if (float.TryParse(value, out floatResult))
                            {
                                if (floatResult > 0)
                                { this.screenScale = floatResult; }
                                else
                                { this.screenScale = 0; }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "maxnumberofpotions":
                            if (int.TryParse(value, out intResult))
                            {
                                this.maxNumberOfPotions = intResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;

                            



                        case "attackkey":                                                               //place new ints above
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.attackKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altattackkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altAttackKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "shockwavekey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.shockwaveKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altshockwavekey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altShockwaveKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "shieldkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.shieldKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altshieldkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altShieldKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "jumpkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.jumpKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altjumpkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altJumpKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "upkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.upKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altupkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altUpKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "downkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.downKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altdownkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altDownKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "leftkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.leftKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altleftkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altLeftKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "rightkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.rightKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altrightkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altRightKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "rollkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.rollKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altrollkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altRollKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "potionkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.potionKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altpotionkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altPotionKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "usekey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.useKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "altusekey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                this.altUseKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gpattackkey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPattackKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gpshockwavekey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPshockwaveKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gpshieldkey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPshieldKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gpjumpkey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPjumpKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gpupkey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPupKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gpdownkey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPdownKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gpleftkey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPleftKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gprightkey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPrightKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gprollkey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GProllKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gppotionkey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPpotionKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "gpusekey":
                            if (Enum.TryParse(value, out buttonResult))
                            {
                                this.GPuseKey = buttonResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;





                        case "shockwaveknockback":                                                        //place new key binds above
                            this.shockwaveKnockback = Vector2Parser(value);
                            if (this.shockwaveKnockback == new Vector2(-0.112f, -0.112f))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "boundingbox":
                            this.boundingBox = RectangleCenterParser(value);
                            if (this.boundingBox == new Rectangle(-0, -1, -1, -2))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "timeraccuracy":
                            if (int.TryParse(value, out intResult))
                            {
                                this.timerAccuracy = "00.";
                                if (intResult > 8)
                                {
                                    for (int i = 8; i > 0; i--)
                                    { this.timerAccuracy += "0"; }
                                }
                                else
                                {
                                    for (int i = intResult; i > 0; i--)
                                    { this.timerAccuracy += "0"; }
                                }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            
                            checker++; break;
                        case "resolution":
                            this.resolution = PointParser(value);
                            if (this.resolution == new Point(-0112, -0112))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "attackcollidersize":
                            this.attackColliderSize = PointParser(value);
                            if (this.attackColliderSize == new Point(-0112, -0112))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "attackdamage":
                            this.attackDamage = Vector2Parser(value);
                            if (this.attackDamage == new Vector2(-0.112f, -0.112f))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "critmultiplier":
                            this.critMultiplier = Vector2Parser(value);
                            if (this.critMultiplier == new Vector2(-0.112f, -0.112f))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "playerlight":
                            this.playerLight = RectangleParser(value);
                            if (this.playerLight == new Rectangle(-0, -1, -1, -2))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "shieldlight":
                            this.shieldLight = RectangleParser(value);
                            if (this.shieldLight == new Rectangle(-0, -1, -1, -2))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;





                        case "cameralerp":                                                                //place new "etc variable" (like vectors and rectangles) above
                            if (bool.TryParse(value, out boolResult))
                            {
                                this.cameraLerpSetting = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "camerashakesetting":
                            if (bool.TryParse(value, out boolResult))
                            {
                                this.cameraShakeSetting = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "fullscreen":
                            if (bool.TryParse(value, out boolResult))
                            {
                                this.fullscreen = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "vsync":
                            if (bool.TryParse(value, out boolResult))
                            {
                                this.vSync = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "cameraswingsetting":
                            if (bool.TryParse(value, out boolResult))
                            {
                                this.cameraSwingSetting = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "easywalljumpmode":
                            if (bool.TryParse(value, out boolResult))
                            { this.easyWalljumpMode = boolResult; }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "lighting":
                            if (bool.TryParse(value, out boolResult))
                            { this.lighting = boolResult; }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        case "smartcamera":
                            if (bool.TryParse(value, out boolResult))
                            { this.smartCamera = boolResult; }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;






                        case "debug":                                                                     //place new bools above
                            if (int.TryParse(value, out intResult))
                            {
                                this.debug = intResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker++; break;
                        default:
                            if (string.IsNullOrWhiteSpace(extra))
                            {
                                Irbis.Irbis.WriteLine("error: no variable with name: \"" + variable + "\"");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: no variable with name: \"" + variable + "[" + extra + "]\"");
                                errorVars = errorVars + "\n    " + variable + "[" + extra + "]";
                            }
                            break;
                    }
                }
            }
        }

        if (animationFrames.Count > 0)
        { checker++; }
        if (animationSpeed.Count > 0)
        { checker++; }
        if (characterWidth.Count > 0)
        { checker++; }

        this.animationFrames = animationFrames.ToArray();
        this.animationSpeed = animationSpeed.ToArray();
        this.characterWidth = characterWidth.ToArray();

        bool variablesHaveEncounteredErrors = !string.IsNullOrWhiteSpace(errorVars);
        int variablesThatHaveBeenAssigned = checker;

        //checker.count should always be == to number of variable, currently 66 variables
        if (variablesHaveEncounteredErrors || variablesThatHaveBeenAssigned < numberOfVariables)
        {
            if (!string.IsNullOrWhiteSpace(errorVars))
            {
                Irbis.Irbis.WriteLine("could not load:" + errorVars + "\nthese variables.");
                Console.WriteLine("could not load:" + errorVars + "\nthese variables.");
            }

            if (variablesThatHaveBeenAssigned < numberOfVariables)
            {
                Irbis.Irbis.WriteLine("Loaded " + variablesThatHaveBeenAssigned + " variables successfully.");
                Console.WriteLine("Loaded " + variablesThatHaveBeenAssigned + " variables successfully.");
                Irbis.Irbis.WriteLine("Not all variables were assigned values. Is something misspelled?");
                Console.WriteLine("Not all variables were assigned values. Is something misspelled?");
            }
        }
        else
        { Irbis.Irbis.WriteLine("load successful."); }
        reader.Close();
    }

    private static Vector2 Vector2Parser(string value)
    {
        string Xval = string.Empty;
        string Yval = string.Empty;
        //value = {X:000Y:000}
        value = value.Substring(1);
        //value = X:000Y:000}
        while (value.Length > 0 && !value[0].Equals('\u007d'))   //'\u007d'://}
        {
            if (value[0].Equals('\u0058') || value[0].Equals('\u0078'))  //'\u0058'://X '\u0078'://x
            {
                // X + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d') || value[j].Equals('\u002e'))) //'\u002d'://- '\u002e'://.
                {
                    Xval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0059') || value[0].Equals('\u0079')) //'\u0059'://Y '\u0079'://y
            {
                // Y + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d') || value[j].Equals('\u002e'))) //'\u002d'://- '\u002e'://.
                {
                    Yval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
        }

        if (float.TryParse(Xval, out float Xresult))
        {
            if (float.TryParse(Yval, out float Yresult))
            {
                return new Vector2(Xresult, Yresult);
            }
            else
            {
                Irbis.Irbis.WriteLine("error: Vector2 could not be parsed");
            }
        }
        else
        {
            Irbis.Irbis.WriteLine("error: Vector2 could not be parsed");
        }
        return new Vector2(-0.112f, -0.112f);
    }

    private static Point PointParser(string value)
    {
        string Xval = string.Empty;
        string Yval = string.Empty;
        //value = {X:000Y:000}
        if (!(value[0].Equals('\u0058') || value[0].Equals('\u0078') || value[0].Equals('\u0059') || value[0].Equals('\u0079')))
        {
            value = value.Substring(1);
        }
        //value = X:000Y:000}
        while (value.Length > 0 && !value[0].Equals('\u007d'))   //'\u007d'://}
        {
            if (value[0].Equals('\u0058') || value[0].Equals('\u0078'))  //'\u0058'://X '\u0078'://x
            {
                // X + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://- //This is the same as Vector2Variable(string), with the caviet that ints will never have a decimal
                {
                    Xval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0059') || value[0].Equals('\u0079')) //'\u0059'://Y '\u0079'://y
            {
                // Y + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d') || value[j].Equals('\u002e'))) //'\u002d'://- '\u002e'://.
                {
                    Yval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
        }
        if (int.TryParse(Xval, out int Xresult))
        {
            if (int.TryParse(Yval, out int Yresult))
            {
                return new Point(Xresult, Yresult);
            }
            else
            {
                Irbis.Irbis.WriteLine("error: Point could not be parsed");
            }
        }
        else
        {
            Irbis.Irbis.WriteLine("error: Point could not be parsed");
        }
        return new Point(-0112, -0112);
    }

    private static Rectangle RectangleParser(string value)
    {
        string Xval = string.Empty;
        string Yval = string.Empty;
        string widthVal = string.Empty;
        string heightVal = string.Empty;
        //value = {X:000Y:000Width:000Height:000}
        value = value.Substring(1);
        //value = X:000Y:000Width:000Height:000}
        while (value.Length > 0 && !value[0].Equals('\u007d'))   //'\u007d'://}
        {
            if (value[0].Equals('\u0058') || value[0].Equals('\u0078'))  //'\u0058'://X '\u0078'://x
            {
                // X + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://- This is the same as Vector2Variable(string), with the caviet that ints will never have a decimal
                {
                    Xval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0059') || value[0].Equals('\u0079')) //'\u0059'://Y '\u0079'://y
            {
                // Y + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://-
                {
                    Yval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0057') || value[0].Equals('\u0077')) //'\u0057'://W '\u0077'://w
            {
                // Width + : = 6 characters before data starts
                int j = 6;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://-
                {
                    widthVal += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0048') || value[0].Equals('\u0068')) //'\u0048'://H '\u0068'://h
            {
                // Height + : = 7 characters before data starts
                int j = 7;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://-
                {
                    heightVal += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
        }
        //boundingBox.X = int.Parse(buttonList[menuSelection].buttonStatement) - (boundingBox.Width / 2);
        if (int.TryParse(Xval, out int Xresult))
        {
            if (int.TryParse(Yval, out int Yresult))
            {
                if (int.TryParse(widthVal, out int widthResult))
                {
                    if (int.TryParse(heightVal, out int HeightResult))
                    { return new Rectangle(Xresult, Yresult, widthResult, HeightResult); }
                    else
                    { Irbis.Irbis.WriteLine("error: Rectangle could not be parsed"); }
                }
                else
                { Irbis.Irbis.WriteLine("error: Rectangle could not be parsed"); }
            }
            else
            { Irbis.Irbis.WriteLine("error: Rectangle could not be parsed"); }
        }
        else
        { Irbis.Irbis.WriteLine("error: Rectangle could not be parsed"); }
        return new Rectangle(-0, -1, -1, -2);
    }

    private static Rectangle RectangleCenterParser(string value)
    {
        string Xval = string.Empty;
        string Yval = string.Empty;
        string widthVal = string.Empty;
        string heightVal = string.Empty;
        //value = {X:000Y:000Width:000Height:000}
        value = value.Substring(1);
        //value = X:000Y:000Width:000Height:000}
        while (value.Length > 0 && !value[0].Equals('\u007d'))   //'\u007d'://}
        {
            if (value[0].Equals('\u0058') || value[0].Equals('\u0078'))  //'\u0058'://X '\u0078'://x
            {
                // X + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://- This is the same as Vector2Variable(string), with the caviet that ints will never have a decimal
                {
                    Xval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0059') || value[0].Equals('\u0079')) //'\u0059'://Y '\u0079'://y
            {
                // Y + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://-
                {
                    Yval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0057') || value[0].Equals('\u0077')) //'\u0057'://W '\u0077'://w
            {
                // Width + : = 6 characters before data starts
                int j = 6;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://-
                {
                    widthVal += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0048') || value[0].Equals('\u0068')) //'\u0048'://H '\u0068'://h
            {
                // Height + : = 7 characters before data starts
                int j = 7;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://-
                {
                    heightVal += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
        }
        //boundingBox.X = int.Parse(buttonList[menuSelection].buttonStatement) - (boundingBox.Width / 2);
        if (int.TryParse(Xval, out int Xresult))
        {
            if (int.TryParse(Yval, out int Yresult))
            {
                if (int.TryParse(widthVal, out int widthResult))
                {
                    if (int.TryParse(heightVal, out int HeightResult))
                    { return new Rectangle(Xresult - (widthResult / 2), Yresult - (HeightResult / 2), widthResult, HeightResult); }
                    else
                    { Irbis.Irbis.WriteLine("error: Rectangle could not be parsed"); }
                }
                else
                { Irbis.Irbis.WriteLine("error: Rectangle could not be parsed"); }
            }
            else
            { Irbis.Irbis.WriteLine("error: Rectangle could not be parsed"); }
        }
        else
        { Irbis.Irbis.WriteLine("error: Rectangle could not be parsed"); }
        return new Rectangle(-0, -1, -1, -2);
    }
}
