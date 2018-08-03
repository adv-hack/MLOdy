using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahc.Health.Model
{
    public class HealthDataCollected
    {

        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public string Parameter { get; set; }
        public string UOM { get; set; }
        public string Reading { get; set; }
        public string PredictiveStatus { get; set; }
        public string Status { get; set; }
        public DateTime ReadingTime { get; set; }
        public bool IsMonitored { get; set; }
        public string ActionToBeTaken { get; set; }
        public bool IsActionTaken { get; set; }
        public string ActionTaken { get; set; }
        public string JsonString { get; set; }
        
    }

}
