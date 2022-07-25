using System.Diagnostics;

//Code by Noah Stevens

//DONE:
/* - A game will consist of 10 “frames” of play
 * - The user (me) will click a button/something which will result in me getting a score 
 * - Each frame, the maximum number of pins I can knock down is 10 between two balls I will throw. 
 *     If I get a 10 on the first ball, I don’t need to roll a second ball for the frame.
 * - On the 10th and final frame, if between the two balls I throw I get 10 pins, I get a bonus ball for extra points.
 * - Each time a frame is completed, update on the screen my total score so I can see how I am doing.
 * - When the game is complete, give me the option to start a new game which will reset the score and allow me to play again.
 * OPTIONAL DONE:
 * - Treat scoring like traditional bowling using spares and strikes (Needs some refactoring)
 * - Treat the 10th and final frame like traditional bowling (needs some refactoring)
 * - Debug mode lets you choose each frame (need try/catch!!!)
 */

//TO DO:
/* Would like to:
 * - Multiple players
 * - Multiple games with multiple players
 * 
 * Optional:
 * - Create something in the UI to let me see my score for each frame, and maybe if I want to change the score for a frame give me that option.
 * -Save my high score (in any way you would like) and have it displayed for me on the screen so I know what I am trying to beat!
 * -Give me a cheater button, so no matter what I roll it will always be a strike (so I can test logic for a 300 score)
 * -Give me a loser button, so no matter what I roll it will always be a 0 (so I can test logic for someone who clearly needs to play bumper bowling)
 * 
 */

namespace BowlingApp
{
    //make calculating and populating scores different
    public partial class GameForm : Form
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

        private Player player;
        private ScoreCalculator scoreCalculator;


        public GameForm()
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

            frames = new Frame[12];

            player = new Player(0);
            scoreCalculator = new ScoreCalculator();

        }

        //Fills in each label and textbox with "--" and also sets the runningTotals array
        private void Form1_Load(object sender, EventArgs e)
        {
            ResetAllText();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void ResetAllText()
        {
            lblDisplayScore.Text = "--";
            foreach (var lbl in LabelsTop) { lbl.Text = "--"; }
            foreach (var lbl in LabelsBottom) { lbl.Text = "--"; }
            foreach (var txtb in TextBoxes) { txtb.Text = "--"; }
        }

        private void ResetGame()
        {
            ResetAllText();
            currentFrame = 0;
            isFirstRoll = true;

            frames = new Frame[12];

            player = new Player(0);
            scoreCalculator.ResetTotalPerFrame();

            btnBowl.Enabled = true;
        }

        //Executed when Bowl Button is clicked
        private void btnBowl_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void Play()
        {
            if (isFirstRoll)
            {
                if (cBoxEnableDebug.Checked)
                    player.EnablePlayerDebug(Int32.Parse(txtDebug.Text), Int32.Parse(txtDebug2.Text));
                else
                    player.DisablePlayerDebug();

                player.Bowl();
                syncFrames();

                DisplayCurrentRoll();
                PopulateRolls();
                if (player.roll1 == MAX_PINS)
                {
                    PopulateTotalScores();
                    isFirstRoll = true;
                }
                else
                    isFirstRoll = false;
            }
            else
            {
                DisplayCurrentRoll();
                PopulateRolls();
                PopulateTotalScores();
                isFirstRoll = true;
            }
            if (currentFrame >= 9)
                checkEndGame();
        }

        //Ends game after final frames)
        private void checkEndGame() // <----- NEEDS REFACTORED A MESS ----------
         {
            if (isFirstRoll)
            {
                if (currentFrame == 9)
                {
                    if (!frames[currentFrame].isStrike && !frames[currentFrame].isSpare)
                    {
                        lblDisplayScore.Text = "All Done!";
                        btnBowl.Enabled = false;
                    }
                }
                else if (currentFrame == 10)
                {
                    if ((!frames[currentFrame].isStrike || !frames[currentFrame - 1].isStrike) )
                    {
                        if ((!frames[currentFrame].isSpare || !frames[currentFrame].isSpare) && (!frames[currentFrame - 1].isStrike) && !frames[currentFrame].isStrike)
                            PopulateTotalScores();

                        lblDisplayScore.Text = "All Done!";
                        btnBowl.Enabled = false;
                    }
                }
                else if (currentFrame == 11)
                {
                    lblDisplayScore.Text = "All Done!";
                    btnBowl.Enabled = false;
                }
            }
            if (currentFrame == 10)
            {
                if (frames[currentFrame - 1].isSpare && !frames[currentFrame].isStrike)
                {
                    PopulateTotalScores();
                    lblDisplayScore.Text = "All Done!";
                    btnBowl.Enabled = false;
                }
            }
            else if (currentFrame == 11)
            {
                if(frames[currentFrame].isStrike)
                {
                    lblDisplayScore.Text = "All Done!";
                    btnBowl.Enabled = false;
                }
                else 
                {
                    PopulateTotalScores();
                    lblDisplayScore.Text = "All Done!";
                    btnBowl.Enabled = false;
                }
                
            }
        }

        private void syncFrames()
        {
            frames = player.allFrames;
            currentFrame = player.currentFrame;
            scoreCalculator.setFrames(player.allFrames, currentFrame);
        }

        //Populates scores on bottom side of scoring sheet
        private void PopulateTotalScores()
        {
            scoreCalculator.calculateScores();
            for (int i = 0; i < LabelsBottom.Count; i++)
            {
                if (scoreCalculator.AllCurrentFrameTotals[i] != -1)
                {
                    LabelsBottom[i].Text = scoreCalculator.AllCurrentFrameTotals[i].ToString();
                }
            }
        }

        //Displays current roll on label when button is pressed
        private void DisplayCurrentRoll()
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
                        PopulateFrame10Rolls();
                    break;
                case NEED_EXTRA_FINAL_FRAME: //Popolates rolls for an extra frame if needed
                    PopulateFrame11Rolls();
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
        private void PopulateFrame10Rolls()
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
        private void PopulateFrame11Rolls()
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
    }
}