using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTypes
{
    // Class for each flag: name and rectangle coordinates
    public class Country
    {
        public string Name;
        public int X;
        public int Y;
    }

    // Enum for randomizing what flag should the player choice
    public enum Choose
    {   
        Left,
        Right
    };

    // Enum for current game state
    public enum GameState
    {
        Menu,
        QuizFirst, // first question
        Quiz,
        Transition, // time between questions
        Result,
        Help,
        HowToPlay,
        About
    };

    // Enum for optimizing navigation between screens:
    // the program waits for Enter key to be released in several places
    public enum EnterReleased
    {
        Yes,
        No
    };
}
