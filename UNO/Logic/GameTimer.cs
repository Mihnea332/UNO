using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace UNO.Logic
{
    public class GameTimer
    {
        System.Timers.Timer game_timer;
        public bool TimeOut { get; private set; }
        public GameTimer(double interval)
        {
            game_timer = new System.Timers.Timer(interval);
            game_timer.AutoReset = false;
            game_timer.Elapsed += OnTimeElapsed;
        }
        public void Start()
        {
            TimeOut = false;
            game_timer.Start();
        }
        public void Stop()
        {
            TimeOut = false;
            game_timer.Stop();
        }
        private void OnTimeElapsed(object sender, ElapsedEventArgs e)
        {
            TimeOut = true;    
            game_timer.Stop();       
        }
    }
}
