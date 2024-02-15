using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Static class that holds key variables for the game.
/// <br></br>
/// Script will not disappear between scenes
/// </summary>
public static class GameManager
{

    // -------------- Level Variables -----------------

    /// <summary>
    /// Level Phase
    /// </summary>
    public static int s_phase = 1;

    /// <summary>
    /// Name of the level, not including phase number
    /// </summary>
    public static string s_level = "Dino";

    /// <summary>
    /// Name of the scene that is loaded. Often GameManager.Level + GameManager.Phase
    /// </summary>
    public static string s_currentScene = "Dino1";

    /// <summary>
    /// Name of the scene when checkpoint is reached
    /// </summary>
    public static int s_checkpointPhase = 1;

    // -------------- Starting Positions ---------------------

    /// <summary>
    /// Wizard's starting x position
    /// </summary>
    public static float s_wizardResetX = -9.565638f;

    /// <summary>
    /// Wizard's starting y position
    /// </summary>
    public static float s_wizardResetY = 2.258681f;

    /// <summary>
    /// Fairy's starting x position
    /// </summary>
    public static float s_fairyResetX = -4.29f;

    /// <summary>
    /// Fairy's starting y position
    /// </summary>
    public static float s_fairyResetY = 2.56f;

    // -------------- Respawn Point ---------------------

    /// <summary>
    /// Wizard's Respawn x position
    /// </summary>
    public static float s_wizardRespawnX = -9.565638f;

    /// <summary>
    /// Wizard's Respawn y position
    /// </summary>
    public static float s_wizardRespawnY = 2.258681f;

    // --------------- Game Over Variables -------------------
    public static bool s_onMoveableObject = false;


    // --------------- Key Bindings -----------------------

    /// <summary>
    /// Dictionary that maps an enum KeyBind to the corresponding Keycode
    /// <br></br>
    /// Ex: GameManager.KeyBind.WizardJump maps to KeyCode.Space aka space bar = wizard jump for the default value
    /// </summary>
    public static Dictionary<KeyBind, KeyCode> s_keyBinds;

    // Actual key binds are in LoadGame script

    // --------------- Enums -----------------------------
    
    /// <summary>
    /// Enums of keyboard actions. Use when wanting to change keybinding. 
    /// Use when getting/updating values in s_keybinds.
    /// </summary>
    public enum KeyBind { 
        WizardJump, WizardLeft, WizardRight, 
        FairyUp, FairyLeft, FairyDown, FairyRight,
        MoveTimeBack, MoveTimeFor, Preserve, Interact
    };


    // ---------------- Boundaries -------------------------
    public static float s_boundaryLeft = -8.52f;
    public static float s_boundaryRight = 8.52f;
    public static float s_boundaryUp = 4.25f;
    public static float s_boundaryDown = -4.02f;


    // ---------------- Saved Data for Orbs --------------------------------
    public static bool[] s_dinoOrbs = { false, false, false };

    // ---------------- Current level orb data -------------------------
    /// <summary>
    /// Current data about orbs for current level
    /// <br></br>
    /// True if player collected orbs
    /// </summary>
    public static bool[] s_curOrbs = { false, false, false };

    // ------------------ Wizard Interactables ------------------------
    public static Color s_interactColor = Color.cyan;
}
