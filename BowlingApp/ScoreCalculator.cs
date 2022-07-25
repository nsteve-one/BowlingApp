using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingApp
{
    internal class ScoreCalculator //will need heavily modified once Player Bowl() is better utilized
    {
        private const int FINAL_FRAME = 10;
        private const int NEED_ADDITIONAL_FINAL_FRAME = 11;

        private const int NUM_FRAMES = 11;

        private int[] _totalPerFrame;
        private Frame[] _frames;
        private int _currentFrame;

        public ScoreCalculator()
        {
            _frames = new Frame[NUM_FRAMES];
            _totalPerFrame = new int[NUM_FRAMES];
            ResetTotalPerFrame();
        }

        public void setFrames(Frame[] frames, int currentFrame)
        {
            _frames = frames;
            _currentFrame = currentFrame;
        }

        private void ResetTotalPerFrame()
        {
            for(int i = 0; i < _totalPerFrame.Length; i++)
            {
                _totalPerFrame[i] = -1;
            }
        }

        public int[] AllCurrentFrameTotals
        {
            get { return _totalPerFrame; }
        }

        public int getOneFrameScore(int frameNum)
        {
            if(_totalPerFrame[frameNum] == -1)
                return -1;
            else
                return _totalPerFrame[frameNum];
        }

        public int getCurrentTotalScore(ref int index)
        {
            int totalScore = 0;
            for(int i = _totalPerFrame.Length; i > 0; i--)
            {
                if (_totalPerFrame[i - 1] != -1)
                {
                    totalScore = _totalPerFrame[i];
                    index = i;
                    break;
                }
            }
            return totalScore;
        }

        private int getCurrentTotalScore()
        {
            int totalScore = 0;
            for (int i = 0; i < _totalPerFrame.Length; i++)
            {
                if (_totalPerFrame[i] != -1)
                    totalScore = _totalPerFrame[i];
            }
            return totalScore;
        }

        public void calculateScores() //Very manual, not *amazing* code
        {
            Frame thisFrame = _frames[_currentFrame];
            Frame previousFrame = new Frame();

            if ((_currentFrame - 1) >= 0)
                previousFrame = _frames[_currentFrame - 1];

            if (_currentFrame == -1)
                return;

            if (_currentFrame == NEED_ADDITIONAL_FINAL_FRAME) //Populate score total for 11th frame (must have had two strikes on the 10th frame)
            {
                if (thisFrame.isStrike)
                {
                    _totalPerFrame[_currentFrame - 2] = getCurrentTotalScore() + 30 + thisFrame.score1;
                }
                else
                {
                    _totalPerFrame[_currentFrame - 2] = getCurrentTotalScore() + 20 + thisFrame.score1;
                }
                return;
            }
            else if (_currentFrame == FINAL_FRAME) //Populate total score for final frame
            {
                if (previousFrame.isStrike) //If the previous frame is strike
                {
                    if (_frames[_currentFrame - 2].isStrike) //If the frame before that is a strike
                    {
                        if (thisFrame.isStrike) //Three strikes in a row
                        {
                            _totalPerFrame[_currentFrame - 2] = getCurrentTotalScore() + 30;
                        }
                        else if (thisFrame.isSpare) //Two strikes and a spare
                        {
                            _totalPerFrame[_currentFrame - 2] = getCurrentTotalScore() + 20 + thisFrame.score1;
                            _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 20;
                        }
                        else //Two strikes and two rolls
                        {
                            _totalPerFrame[_currentFrame - 2] = getCurrentTotalScore() + 20 + thisFrame.score1;
                            _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 10 + thisFrame.score1 + thisFrame.score2;
                        }
                    } //
                    else if (thisFrame.isSpare) //strike and then a spare
                    {
                        _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 20;
                    }
                    else //strike and then not a strike or spare
                    {
                        _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 10 + thisFrame.score1 + thisFrame.score2;
                    }

                }

                if (previousFrame.isSpare) //Previous frame is a spare
                {
                    if (thisFrame.isStrike) //This frame is a strike
                    {
                        _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 20;
                    }
                    else //this frame is a strike or a spare
                    {
                        _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 10 + thisFrame.score1;
                    }
                }
                return;
            }

            //MAIN FUNCTIONALITY OF THIS FUNCTION IS BELOW
            if (_currentFrame - 2 >= 0) //make sure at least two frames have been completed - Handles scores that cannot be completed immediately (Largely multiple strikes)
            {
                if (_frames[_currentFrame - 2].isStrike && previousFrame.isStrike) //Two previous frames were strikes
                {
                    if (thisFrame.isStrike)//three strikes (turkey)
                    {
                        _totalPerFrame[_currentFrame - 2] = getCurrentTotalScore() + 30;
                    }
                    else if (thisFrame.isSpare)// two strike and then a spare
                    {
                        _totalPerFrame[_currentFrame - 2] = getCurrentTotalScore() + 20 + thisFrame.score1;
                    }
                    else // two strike and then no spare or strike
                    {
                        _totalPerFrame[_currentFrame - 2] = getCurrentTotalScore() + 20 + thisFrame.score1;
                    }
                }
            }

            if (_currentFrame - 1 >= 0) //Make sure that at least one frame has been completed - (Strikes and spares)
            {
                if (previousFrame.isStrike) //Last frame is a strike
                {
                    if (thisFrame.isSpare) //strike and then a spare
                    {
                        _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 20;
                    }
                    else if (!thisFrame.isStrike) //strike and then no spare or strike
                    {
                        _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 10 + thisFrame.score1 + thisFrame.score2;
                    }
                }
                else if (previousFrame.isSpare) //last frame is a spare
                {
                    if (thisFrame.isStrike) //spare and then a strike
                    {
                        _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 20;
                    }
                    else //spare and then no strike
                    {
                        _totalPerFrame[_currentFrame - 1] = getCurrentTotalScore() + 10 + thisFrame.score1;
                    }
                }
            }

            if (!thisFrame.isStrike && !thisFrame.isSpare) //Calculate current frame if neither a strike nor a spare was rolled
            {
                _totalPerFrame[_currentFrame] = getCurrentTotalScore() + thisFrame.score1 + thisFrame.score2;
            }
        }

    }
}
