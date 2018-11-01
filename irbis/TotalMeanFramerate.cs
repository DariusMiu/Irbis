using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TotalMeanFramerate
{
    long frames;
    double currentFrametimes;

    public double Framerate
    {
        get
        { return (frames / currentFrametimes); }
    }

    public TotalMeanFramerate()
    {
        frames = 0;
        currentFrametimes = 0d;
    }

    public void Update(double timeSinceLastFrame)
    {
        currentFrametimes += timeSinceLastFrame;
        frames++;
    }
}
