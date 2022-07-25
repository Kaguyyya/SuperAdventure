using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Engine
{
   public class LivingCreature : INotifyPropertyChanged
    {
        private int _currentHitPoints;
        public int currentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged("CurrentHitPoints"); 
            } 
        }     
        public int maximumHitPoints { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public LivingCreature(int CurrentHitPoints, int MaximumHitPoints)
        {

            currentHitPoints = CurrentHitPoints;
            maximumHitPoints = MaximumHitPoints;
        }
    }
}
