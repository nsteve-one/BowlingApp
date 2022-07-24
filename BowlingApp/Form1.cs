using System.Diagnostics;
using System.Numerics;

namespace BowlingApp
{
    struct Frame
    {
        public bool isStrike;
        public bool isSpare;
        public int score1;
        public int score2;
        public bool isDone;

        public Frame()
        {
            isStrike = false;
            isSpare = false;
            score1 = -1;
            score2 = -1;
            isDone = false;
            
        }
    }

    public partial class Form1 : Form
    {
        public const int MAX_PINS = 10;
        int currentFrame;
        bool isFirstRoll;
        string strike;
        string spare;

        List<Label> LabelsTop;
        List<Label> LabelsBottom;
        List<TextBox> TextBoxes;
        Frame[] frames;
        Random random;
        public Form1()
        {
            InitializeComponent();
            
            //Labels on the top of the scoring sheet (next to Text Boxes); display initial roll
            LabelsTop = new List<Label> { lblScore1_1,lblScore2_1, lblScore3_1, lblScore4_1, 
                lblScore5_1, lblScore6_1, lblScore7_1, lblScore8_1, lblScore9_1};

            //Text Boxes on the top of the scoring sheet; display second roll and all frame-10 rolls
            TextBoxes = new List<TextBox> { txtboxScore1, txtboxScore2, txtboxScore3,txtboxScore4, txtboxScore5,
                txtboxScore6, txtboxScore7, txtboxScore8, txtboxScore9, txtboxScore10_1, txtboxScore10_2, txtboxScore10_3 };

            //Show total score
            LabelsBottom = new List<Label> { lblScore1_2, lblScore2_2, lblScore3_2, lblScore4_2, lblScore5_2,
            lblScore6_2, lblScore7_2, lblScore8_2, lblScore9_2, lblScore10};


            currentFrame = 0;
            isFirstRoll = true;
            strike = "X";
            spare = "/";


            frames = new Frame[12];
            random = new Random();

        }

        //Fills in each label and textbox with "--"
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach(var lbl in LabelsTop) { lbl.Text = "--"; }
            foreach (var lbl in LabelsBottom) { lbl.Text = "--"; }
            foreach (var txtb in TextBoxes) { txtb.Text = "--"; }
        }

        //Executed when Bowl Button is clicked
        private void btnBowl_Click(object sender, EventArgs e)
        {
            if (isFirstRoll)
            {
                Roll();
                DisplayCurrentRoll(currentFrame, isFirstRoll);
                PopulateRolls();
                if (frames[currentFrame].isStrike)
                {
                    currentFrame++;
                    isFirstRoll = true;
                    PopulateScoreTotals(currentFrame - 1);

                }
                else
                {
                    isFirstRoll = false;
                }
            }

            else
            {
                DisplayCurrentRoll(currentFrame, isFirstRoll);
                PopulateRolls();
                currentFrame++;
                isFirstRoll = true;
                PopulateScoreTotals(currentFrame - 1);
            }
            

            //Debug.WriteLine(currentFrame);
            //Ends game after final frames)
            if (currentFrame == 10 && !(frames[currentFrame - 1].isSpare || frames[currentFrame - 1].isStrike))
            {
                lblDisplayScore.Text = "All Done!";
                btnBowl.Enabled = false;
            }
            else if (currentFrame == 11 && (!frames[currentFrame-1].isStrike || txtboxScore10_3.Text == "X"))
            {
                lblDisplayScore.Text = "All Done!";
                btnBowl.Enabled = false;
            }
            else if(currentFrame >= 12)
            {
                lblDisplayScore.Text = "All Done!";
                btnBowl.Enabled = false;
            }
        }

        //Generates random score between 0 and number passed into function
        private int GenerateScore(int numPinsLeft)
        {
            return random.Next(0, numPinsLeft + 1);
        }

        //Displays each individual role on the top sode of the scoring sheet
        private void PopulateRolls()
        {
            Frame thisFrame = frames[currentFrame]; 
            //Frame lastFrame = frames[currentFrame - 1];

            switch (currentFrame)
            {
                case 9:
                    DisplayFrame9Rolls();
                    break;
                case 10:
                    Frame lastFrame = frames[currentFrame - 1];
                    if (lastFrame.isSpare || lastFrame.isStrike)
                        DisplayFrame10Rolls();
                    break;
                case 11:
                    DisplayFrame11Rolls();
                    break;
                default:
                    switch ((thisFrame.isStrike, thisFrame.isSpare))
                    {
                        case (true, false):
                            TextBoxes[currentFrame].Text = strike;
                            break;

                        case (false, true):
                            if (isFirstRoll)
                                LabelsTop[currentFrame].Text = thisFrame.score1.ToString();
                            else
                                TextBoxes[currentFrame].Text = spare;
                            break;

                        case (false, false):
                            if (isFirstRoll)
                                LabelsTop[currentFrame].Text = thisFrame.score1.ToString();
                            else
                                TextBoxes[currentFrame].Text = thisFrame.score2.ToString();
                            break;
                    }
                    break;

            }
        }

        //Logic for displaying Frame 9 rolls
        private void DisplayFrame9Rolls()
        {
            Frame thisFrame = frames[currentFrame];
            Frame lastFrame = frames[currentFrame - 1];

            switch ((thisFrame.isStrike, thisFrame.isSpare))
            {
                case (true, false):
                    TextBoxes[currentFrame].Text = strike;
                    break;

                case (false, true):
                    if (isFirstRoll)
                        TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
                    else
                        TextBoxes[currentFrame + 1].Text = spare;
                    break;

                case (false, false):
                    if (isFirstRoll)
                        TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
                    else
                        TextBoxes[currentFrame + 1].Text = thisFrame.score2.ToString();
                    break;
            }
        }

        //Logic for displaying Frame 10 rolls
        private void DisplayFrame10Rolls()
        {
            Frame thisFrame = frames[currentFrame];
            Frame lastFrame = frames[currentFrame - 1];

            if (thisFrame.isStrike)
            {
                if (lastFrame.isStrike)
                {
                    TextBoxes[currentFrame].Text = strike;
                }
                else
                {
                    TextBoxes[currentFrame + 1].Text = strike;
                }
            }

            if (thisFrame.isSpare)
            {
                if (!lastFrame.isStrike)
                    TextBoxes[currentFrame + 1].Text = thisFrame.score1.ToString();

                if (lastFrame.isStrike)
                {
                    if (isFirstRoll)
                        TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
                    else
                        TextBoxes[currentFrame + 1].Text = spare;
                }
            }

            if (!thisFrame.isStrike && !thisFrame.isSpare)
            {
                if (!lastFrame.isSpare)
                {
                    if (isFirstRoll)
                        TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
                    else
                        TextBoxes[currentFrame + 1].Text = thisFrame.score2.ToString();
                }
                if (lastFrame.isSpare)
                {
                    TextBoxes[currentFrame + 1].Text = thisFrame.score1.ToString();
                }
            }
        }

        //Logic for displaying Frame 11 rolls - Only occurs when there are two strikes on frame 10
        private void DisplayFrame11Rolls()
        {
            Frame thisFrame = frames[currentFrame];
            Frame lastFrame = frames[currentFrame - 1];


            if (lastFrame.isStrike && thisFrame.isStrike)
                {
                    TextBoxes[currentFrame].Text = strike;
                }
                if (!thisFrame.isStrike)
                {
                    TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
                }
        }

        //Calculates and displays scores on bottom side of scoring sheet
        private void PopulateScoreTotals(int currentFrame)
        {
            int newScore;

            Frame thisFrame = frames[currentFrame];
            Frame lastFrame = new Frame();
            if ((currentFrame -1) >= 0)
                lastFrame = frames[currentFrame - 1];

            if (currentFrame == -1)
                return;

            if(currentFrame == 11)
            {
                if (thisFrame.isStrike)
                {
                    newScore = currentScoreSum() + 30 + thisFrame.score1;
                    LabelsBottom[currentFrame - 2].Text = newScore.ToString();
                }
                else
                {
                    newScore = currentScoreSum() + 20 + thisFrame.score1;
                    LabelsBottom[currentFrame - 2].Text = newScore.ToString();
                }
                return;
            }
            else if(currentFrame == 10)
            {
                if (lastFrame.isStrike)
                {
                    if (frames[currentFrame - 2].isStrike)
                    {
                        if (thisFrame.isSpare)
                        {
                            newScore = currentScoreSum() + 20 + thisFrame.score1;
                            LabelsBottom[currentFrame - 2].Text = newScore.ToString();
                            newScore = currentScoreSum() + 20;
                            LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                        }
                        else
                        {
                            newScore = currentScoreSum() + 20 + thisFrame.score1;
                            LabelsBottom[currentFrame - 2].Text = newScore.ToString();
                            newScore = currentScoreSum() + 10 + thisFrame.score1 + thisFrame.score2;
                            LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                        }
                    }
                    else if (!thisFrame.isStrike)
                    {
                        newScore = currentScoreSum() + 10 + thisFrame.score1 + thisFrame.score2;
                        LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                    }

                }

                if (lastFrame.isSpare)
                {
                    if (thisFrame.isStrike)
                    {
                        newScore = currentScoreSum() + 20;
                        LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                    }
                    else
                    {
                        newScore = currentScoreSum() + 10 + thisFrame.score1;
                        LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                    }
                }
                return;
            }

            if (currentFrame - 2 >= 0)
            {
                if(frames[currentFrame - 2].isStrike && lastFrame.isStrike)
                {
                    if(thisFrame.isStrike)//three strikes (turkey)
                    {
                        newScore = currentScoreSum() + 30;
                        LabelsBottom[currentFrame - 2].Text = newScore.ToString();
                    }
                    else if (thisFrame.isSpare)//strike and then a spare
                    {
                        newScore = currentScoreSum() + 20;
                        LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                    }
                    else //strike and then no spare or strike
                    {
                        newScore = currentScoreSum() + 10 + thisFrame.score1 + thisFrame.score2;
                        LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                    }
                }
            }

            if (currentFrame - 1 >= 0)
            {
                if(lastFrame.isStrike)
                {
                    if (thisFrame.isSpare)//strike and then a spare
                    {
                        newScore = currentScoreSum() + 20;
                        LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                    }
                    else if(!thisFrame.isStrike) //strike and then no spare or strike
                    {
                        newScore = currentScoreSum() + 10 + thisFrame.score1 + thisFrame.score2;
                        LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                    }
                }
                else if(lastFrame.isSpare)
                {
                    if (thisFrame.isStrike) //spare and then a strike
                    {
                        newScore = currentScoreSum() + 20;
                        LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                    }
                    else //spare and then no strike
                    {
                        newScore = currentScoreSum() + 10 + thisFrame.score1;
                        LabelsBottom[currentFrame - 1].Text = newScore.ToString();
                    }
                }
            }

            if (!thisFrame.isStrike && !thisFrame.isSpare)
            {
                newScore = currentScoreSum() + thisFrame.score1 + thisFrame.score2;
                LabelsBottom[currentFrame].Text = newScore.ToString();
            }
            
            
        }

        //Calculates sum of all previous scores
        private int currentScoreSum()
        {
            int runningScore = 0;
            foreach(var lbl in LabelsBottom)
            {
                if (lbl.Text == "--")
                    break;

                runningScore = Int32.Parse(lbl.Text);
            }
            return runningScore;
        }

        //Displays current roll on label when button is pressed
        private void DisplayCurrentRoll(int frame, bool isFirstRoll)
        {
            if (frames[currentFrame].isStrike)
                lblDisplayScore.Text = "Strike!";
            else if (frames[currentFrame].isSpare && isFirstRoll)
                lblDisplayScore.Text = frames[currentFrame].score1.ToString();
            else if (frames[currentFrame].isSpare && !isFirstRoll)
                lblDisplayScore.Text = "Spare!";
            else if (isFirstRoll)
                lblDisplayScore.Text = frames[currentFrame].score1.ToString();
            else if (!isFirstRoll)
                lblDisplayScore.Text = frames[currentFrame].score2.ToString();

        }

        //Roll's the balls and populates the frames
        private void Roll()
        {
            int firstRoll, secondRoll;

            //firstRoll = Int32.Parse(txtDebug.Text);
            firstRoll = GenerateScore(MAX_PINS);
            Debug.WriteLine(firstRoll);
            if (firstRoll == MAX_PINS)
            {
                frames[currentFrame].isStrike = true;
                frames[currentFrame].isDone = true;
            }
            else
            {
                frames[currentFrame].score1 = firstRoll;
                secondRoll = GenerateScore(MAX_PINS - firstRoll);
                Debug.WriteLine(secondRoll);
                if ((firstRoll + secondRoll) == MAX_PINS)
                {
                    frames[currentFrame].isSpare = true;
                }
                else
                {
                    frames[currentFrame].score2 = secondRoll;
                }
                frames[currentFrame].isDone = false;
            }
        }

    }
}