using System;
using System.Diagnostics;

namespace BowlingApp
{
    public class Player
    {
        private const int MAX_PINS = 10;

        private int _playerID;
        private int _currentFrame;
        private int _roll1, _roll2;
        private int _debugRoll1, _debugRoll2;

        private bool enableDebug;

        private Frame[] frames;

        public Player(int playerID)
        {
            _playerID = playerID;
            _currentFrame = -1;

            _roll1 = -1; _roll2 = -1;

            enableDebug = false;

            frames = new Frame[12];
        }

        public int playerID
        {
            get { return _playerID; }
        }

        public int roll1
        {
            get { return _roll1; }
            set { _roll1 = value; }
        }

        public int roll2
        {
            get { return _roll2; }
            set { _roll2 = value; }
        }

        public int currentFrame
        {
            get { return _currentFrame; }
        }

        public Frame[] allFrames
        {
            get { return frames; }
        }

        public void goNextFrame()
        {
            _currentFrame++;
        }

        public void EnablePlayerDebug(int debugRoll1, int debugRoll2)
        {
            enableDebug = true;
            _debugRoll1 = debugRoll1;
            _debugRoll2 = debugRoll2;
        }

        public void DisablePlayerDebug()
        {
            enableDebug = false;
        }

        private int GenerateScore(int numPinsLeft)
        {
            Random random = new Random();
            return random.Next(0, numPinsLeft + 1);
        }

        public void Bowl() //Need to make logic for Bonus scores here, so I can simplify or delete the Score calculator
        {
            if(enableDebug) //Debug Mode
            {
                _currentFrame++;
                roll1 = _debugRoll1;
                Debug.WriteLine(roll1);
                if (roll1 == MAX_PINS)
                {
                    frames[currentFrame].score1 = 10;
                    frames[currentFrame].isStrike = true;
                    frames[currentFrame].isDone = true;
                }
                else
                {
                    frames[currentFrame].score1 = roll1;
                    roll2 = _debugRoll2;
                    Debug.WriteLine(roll2);
                    if ((roll1 + roll2) == MAX_PINS)
                    {
                        frames[currentFrame].isSpare = true;
                    }

                    frames[currentFrame].score2 = roll2;
                    frames[currentFrame].isDone = false;
                }
                return;
            }

            _currentFrame++;
            roll1 = GenerateScore(MAX_PINS);
            Debug.WriteLine(roll1);
            if (roll1 == MAX_PINS)
            {
                frames[currentFrame].score1 = 10;
                frames[currentFrame].isStrike = true;
                frames[currentFrame].isDone = true;
            }
            else
            {
                frames[currentFrame].score1 = roll1;
                roll2 = GenerateScore(MAX_PINS - roll1);
                Debug.WriteLine(roll2);
                if ((roll1 + roll2) == MAX_PINS)
                {
                    frames[currentFrame].isSpare = true;
                }

                frames[currentFrame].score2 = roll2;
                frames[currentFrame].isDone = false;
            }
        }
    }
}
