/*
 *   TextGizmo.cs
 *   August 2009
 *   Carl Emil Carlsen
 *   sixthsensor.dk
 */

using UnityEngine;
using System.Collections.Generic;

public class TextGizmo
{
    #region Singleton

    private class Singleton
    {
        static Singleton()
        {
            if (instance == null)
            {
                instance = new TextGizmo();
            }
        }

        internal static readonly TextGizmo instance;
    }

    public static TextGizmo Instance { get { return Singleton.instance; } }

    #endregion Singleton

    private const int CHAR_TEXTURE_HEIGHT = 11;
    private const int CHAR_TEXTURE_WIDTH = 8;
    private const string characters = " !#%'()+,-.0123456789;=abcdefghijklmnopqrstuvwxyz_{}~\\?\":/*";

    private Dictionary<char, string> texturePathLookup;
    private Dictionary<char, string> specialChars;

    private TextGizmo()
    {
        specialChars = new Dictionary<char, string>();

        specialChars.Add('\\', "backslash");
        specialChars.Add('?', "questionmark");
        specialChars.Add('"', "quotes");
        specialChars.Add(':', "colon");
        specialChars.Add('/', "slash");
        specialChars.Add('*', "star");

        texturePathLookup = new Dictionary<char, string>();

        for (int c = 0; c < characters.Length; c++)
        {
            string charName = specialChars.ContainsKey(characters[c]) ? specialChars[characters[c]] : characters[c].ToString();
            texturePathLookup.Add(characters[c], "TextGizmo/text_" + charName + ".png");
        }
    }

    public void DrawText(Vector3 position, object message)
    {
        DrawText(position, message != null ? message.ToString() : "(null)");
    }

    public void DrawText(Vector3 position, string format, params object[] args)
    {
        DrawText(position, string.Format(format, args));
    }

    private void DrawText(Vector3 position, string text)
    {
        // Set it up to be the current camera
        Camera camera = Camera.current;
        string lowerText = text.ToLower();

        Vector3 screenPoint = camera.WorldToScreenPoint(position);

        Vector3 offset = Vector3.zero;
        for (int c = 0, n = lowerText.Length; c < n; ++c)
        {
            if ('\n'.Equals(lowerText[c]))
            {
                offset.y += CHAR_TEXTURE_HEIGHT + 2;
                offset.x = 0;
                continue;
            }
            else if (texturePathLookup.ContainsKey(lowerText[c]))
            {
                Gizmos.DrawIcon(camera.ScreenToWorldPoint(screenPoint + offset), texturePathLookup[lowerText[c]]);
                offset.x += CHAR_TEXTURE_WIDTH;
            }
        }
    }
}