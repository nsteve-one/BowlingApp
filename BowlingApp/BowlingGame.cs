using System.Diagnostics;

namespace BowlingApp
{
    //missing a class called player and game

    //Holds the scores for each frame
    

    //make calculating and populating scores different
    public partial class BowlingGame : Form
    {
        private const int NINTH_FRAME = 9;
        private const int FINAL_FRAME = 10;
        private const int NEED_EXTRA_FINAL_FRAME = 11;
        private const int MAX_PINS = 10;
        private const string STRIKE = "X";
        private const string SPARE = "/";

        private int currentFrame;
        private bool isFirstRoll;


        private List<Label> LabelsTop;
        private List<Label> LabelsBottom;
        private List<TextBox> TextBoxes;
        private Frame[] frames;
        private int[] runningTotals;
        private Random random;
        public BowlingGame()
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

            runningTotals = new int[10];
            frames = new Frame[12];
            random = new Random();

        }

        //Fills in each label and textbox with "--" and also sets the runningTotals array
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var lbl in LabelsTop) { lbl.Text = "--"; }
            foreach (var lbl in LabelsBottom) { lbl.Text = "--"; }
            foreach (var txtb in TextBoxes) { txtb.Text = "--"; }
            for (int i = 0; i < runningTotals.Length; i++) { runningTotals[i] = -1; }
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
                    PopulateTotalScores();

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
                PopulateTotalScores();
            }


            //Debug.WriteLine(currentFrame);
            //Ends game after final frames)
            if (currentFrame == 10 && !(frames[currentFrame - 1].isSpare || frames[currentFrame - 1].isStrike))
            {
                lblDisplayScore.Text = "All Done!";
                btnBowl.Enabled = false;
            }
            else if (currentFrame == 11 && (!frames[currentFrame - 1].isStrike || txtboxScore10_3.Text == "X"))
            {
                lblDisplayScore.Text = "All Done!";
                btnBowl.Enabled = false;
            }
            else if (currentFrame >= 12)
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
            //Frame previousFrame = frames[currentFrame - 1];

            switch (currentFrame)
            {
                case NINTH_FRAME: //Populates the rolls of the ninth frame
                    if (!thisFrame.isStrike && !thisFrame.isSpare)
                    {
                        if (isFirstRoll)
                            TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
                        else
                            TextBoxes[currentFrame + 1].Text = thisFrame.score2.ToString();
                    }
                    else if (thisFrame.isStrike)
                    {
                        TextBoxes[currentFrame].Text = STRIKE;
                    }
                    else if (thisFrame.isSpare)
                    {
                        if (isFirstRoll)
                            TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
                        else
                            TextBoxes[currentFrame + 1].Text = SPARE;
                    }
                    break;
                case FINAL_FRAME: //Populates the rolls of the final frame
                    Frame previousFrame = frames[currentFrame - 1];
                    if (previousFrame.isSpare || previousFrame.isStrike) //only needed when a strike or a spare comes before this frame
                        DisplayFrame10Rolls();
                    break;
                case NEED_EXTRA_FINAL_FRAME: //Popolates rolls for an extra frame if needed
                    DisplayFrame11Rolls();
                    break;
                default: //Populates rolls for ever other frame except the ones above
                    switch ((thisFrame.isStrike, thisFrame.isSpare))
                    {
                        case (true, false):
                            TextBoxes[currentFrame].Text = STRIKE;
                            break;

                        case (false, true):
                            if (isFirstRoll)
                                LabelsTop[currentFrame].Text = thisFrame.score1.ToString();
                            else
                                TextBoxes[currentFrame].Text = SPARE;
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

        //Logic for displaying Frame 10 rolls (Too long so I wanted to put it in it's own function
        private void DisplayFrame10Rolls()
        {
            Frame thisFrame = frames[currentFrame];
            Frame previousFrame = frames[currentFrame - 1];

            if (thisFrame.isStrike)
            {
                if (previousFrame.isStrike)
                {
                    TextBoxes[currentFrame].Text = STRIKE;
                }
                else
                {
                    TextBoxes[currentFrame + 1].Text = STRIKE;
                }
            }
            else if (thisFrame.isSpare)
            {
                if (!previousFrame.isStrike)
                {
                    TextBoxes[currentFrame + 1].Text = thisFrame.score1.ToString();
                }
                else if (previousFrame.isStrike)
                {
                    if (isFirstRoll)
                        TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
                    else
                        TextBoxes[currentFrame + 1].Text = SPARE;
                }
            } 
            else if (!thisFrame.isStrike && !thisFrame.isSpare)
            {
                if (!previousFrame.isSpare)
                {
                    if (isFirstRoll)
                        TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
                    else
                        TextBoxes[currentFrame + 1].Text = thisFrame.score2.ToString();
                }
                else if (previousFrame.isSpare)
                {
                    TextBoxes[currentFrame + 1].Text = thisFrame.score1.ToString();
                }
            }
        }

        //Logic for displaying Frame 11 rolls - Only occurs when there are two strikes on frame 10
        private void DisplayFrame11Rolls()
        {
            Frame thisFrame = frames[currentFrame];
            Frame previousFrame = frames[currentFrame - 1];


            if (previousFrame.isStrike && thisFrame.isStrike)
            {
                TextBoxes[currentFrame].Text = STRIKE;
            }
            if (!thisFrame.isStrike)
            {
                TextBoxes[currentFrame].Text = thisFrame.score1.ToString();
            }
        }

        //Calculates then displays scores on bottom side of scoring sheet
        private void PopulateTotalScores()
        {
            CalculateCurrentTotalScore(currentFrame - 1);
            for (int i = 0; i < LabelsBottom.Count; i++)
            {
                if (runningTotals[i] != -1)
                    LabelsBottom[i].Text = runningTotals[i].ToString();
            }
        }

        private void CalculateCurrentTotalScore(int currentFrame)
        {
            //int newScore;

            Frame thisFrame = frames[currentFrame];
            Frame previousFrame = new Frame();

            if ((currentFrame - 1) >= 0)
                previousFrame = frames[currentFrame - 1];

            if (currentFrame == -1)
                return;

            if (currentFrame == NEED_EXTRA_FINAL_FRAME) //Populate score total for 11th frame (must have had two strikes on the 10th frame)
            {
                if (thisFrame.isStrike)
                {
                    runningTotals[currentFrame - 2] = currentScoreSum() + 30 + thisFrame.score1;
                }
                else
                {
                    runningTotals[currentFrame - 2] = currentScoreSum() + 20 + thisFrame.score1;
                }
                return;
            }
            else if (currentFrame == FINAL_FRAME) //Populate total score for final frame
            {
                if (previousFrame.isStrike) //If the previous frame is strike
                {
                    if (frames[currentFrame - 2].isStrike) //If the frame before that is a strike
                    {
                        if (thisFrame.isStrike) //Three strikes in a row
                        {
                            runningTotals[currentFrame - 2] = currentScoreSum() + 30;
                        }
                        else if (thisFrame.isSpare) //Two strikes and a spare
                        {
                            runningTotals[currentFrame - 2] = currentScoreSum() + 20 + thisFrame.score1;
                            runningTotals[currentFrame - 1] = currentScoreSum() + 20;
                        }
                        else //Two strikes and two rolls
                        {
                            runningTotals[currentFrame - 2] = currentScoreSum() + 20 + thisFrame.score1;
                            runningTotals[currentFrame - 1] = currentScoreSum() + 10 + thisFrame.score1 + thisFrame.score2;
                        }
                    } //
                    else if (thisFrame.isSpare) //strike and then a spare
                    {
                        runningTotals[currentFrame - 1] = currentScoreSum() + 20;
                    }
                    else //strike and then not a strike or spare
                    {
                        runningTotals[currentFrame - 1] = currentScoreSum() + 10 + thisFrame.score1 + thisFrame.score2;
                    }

                }

                if (previousFrame.isSpare) //Previous frame is a spare
                {
                    if (thisFrame.isStrike) //This frame is a strike
                    {
                        runningTotals[currentFrame - 1] = currentScoreSum() + 20;
                    }
                    else //this frame is a strike or a spare
                    {
                        runningTotals[currentFrame - 1] = currentScoreSum() + 10 + thisFrame.score1;
                    }
                }
                return;
            }

            //MAIN FUNCTIONALITY OF THIS FUNCTION IS BELOW
            if (currentFrame - 2 >= 0) //make sure at least two frames have been completed - Handles scores that cannot be completed immediately (Largely multiple strikes)
            {
                if (frames[currentFrame - 2].isStrike && previousFrame.isStrike) //Two previous frames were strikes
                {
                    if (thisFrame.isStrike)//three strikes (turkey)
                    {
                        runningTotals[currentFrame - 2] = currentScoreSum() + 30;
                    }
                    else if (thisFrame.isSpare)// two strike and then a spare
                    {
                        runningTotals[currentFrame - 2] = currentScoreSum() + 20 + thisFrame.score1;
                    }
                    else // two strike and then no spare or strike
                    {
                        runningTotals[currentFrame - 2] = currentScoreSum() + 20 + thisFrame.score1;
                    }
                }
            }

            if (currentFrame - 1 >= 0) //Make sure that at least one frame has been completed - (Strikes and spares)
            {
                if (previousFrame.isStrike) //Last frame is a strike
                {
                    if (thisFrame.isSpare) //strike and then a spare
                    {
                        runningTotals[currentFrame - 1] = currentScoreSum() + 20;
                    }
                    else if (!thisFrame.isStrike) //strike and then no spare or strike
                    {
                        runningTotals[currentFrame - 1] = currentScoreSum() + 10 + thisFrame.score1;
                    }
                }
                else if (previousFrame.isSpare) //last frame is a spare
                {
                    if (thisFrame.isStrike) //spare and then a strike
                    {
                        runningTotals[currentFrame - 1] = currentScoreSum() + 20;
                    }
                    else //spare and then no strike
                    {
                        runningTotals[currentFrame - 1] = currentScoreSum() + 10 + thisFrame.score1;
                    }
                }
            }

            if (!thisFrame.isStrike && !thisFrame.isSpare) //Calculate current frame if neither a strike nor a spare was rolled
            {
                runningTotals[currentFrame] = currentScoreSum() + thisFrame.score1 + thisFrame.score2;
            }


        }

        //Calculates sum of all previous scores
        private int currentScoreSum()
        {
            int runningScore = 0;
            for (int i = 0; i < runningTotals.Length; i++)
            {
                if (runningTotals[i] == -1)
                    break;

                runningScore = runningTotals[i];
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

            //firstRoll = Int32.Parse(txtDebug.Text); //forDebug
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