using System;

public class Player
{
    private int _playerID;
    private int _currentFrame;
    private int _roll1, _roll2;

    private Frame[] frames;

    Player(int playerID)
    {
        _playerID = playerID;

        _roll1 = -1; _roll2 = -1;

        frames = new Frame[10];
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
        set { _currentFrame = value; }
    }

    public void goNextFrame()
    {
        currentFrame++;
    }

    public void Bowl()
    {

    }

}
