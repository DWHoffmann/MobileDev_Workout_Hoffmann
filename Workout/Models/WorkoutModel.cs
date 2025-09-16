using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class WorkoutModel
    {
        public string WorkoutType { get; set; }

        public TimeSpan TimeSpan { get; set; }

        public DateTime Date { get; set; }

        public int MinutesWorkout { get; set; }

        public int CalroiesBurnt { get; set; }
    }
}
