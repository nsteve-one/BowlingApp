using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingApp
{
    public struct Frame
    {
        private bool _isStrike;
        private bool _isSpare;
        private bool _isDone;
        private int _score1;
        private int _score2;
        private int _bonus1;
        private int _bonus2;
        //bonus score 1 and 2
        //total score make a getter

        public bool isStrike
        {
            get { return _isStrike; }
            set { _isStrike = value; }
        }

        public bool isSpare
        {
            get { return _isSpare; }
            set { _isSpare = value; }
        }

        public bool isDone
        {
            get { return _isDone; }
            set { _isDone = value; }
        }

        public int score1
        {
            get { return _score1; }
            set { _score1 = value; }
        }

        public int score2
        {
            get { return _score2; }
            set { _score2 = value; }
        }

        public int bonus1
        {
            get { return _bonus1; }
            set { _bonus1 = value; }
        }

        public int bonus2
        {
            get { return _bonus2; }
            set { _bonus2 = value; }
        }

        public Frame()
        {
            _isStrike = false;
            _isSpare = false;
            _isDone = false;
            _score1 = -1;
            _score2 = -1;
            _bonus1 = -1;
            _bonus2 = -1;
        }

        public int getTotalFrame()
        {
            return (_score1 + _score2 + _bonus1 + _bonus2);
        }

    }
}
