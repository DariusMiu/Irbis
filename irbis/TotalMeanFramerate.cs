using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

struct TotalMeanFramerate
{
    long frames;
    double currentFrametimes;

    public double Framerate
    {
        get
        {
            return (frames / currentFrametimes);
        }
    }

    public TotalMeanFramerate(bool create)
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
