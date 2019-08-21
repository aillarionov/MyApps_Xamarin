using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Informer.Utils
{
    public class ObservableProgress : INotifyPropertyChanged
    {
        int _total;
        int _current;

        public ObservableProgress()
        {
        }

        public int Total 
        { 
            get
            {
                return _total;
            }
            set
            {
                _total = value;
                RaisePropertyChanged("Total");
                RaisePropertyChanged("Progress");
                RaisePropertyChanged("FloatProgress");
            }
        }

        public int Current 
        { 
            get
            {
                return _current;
            }
            set
            {
                _current = value;
                RaisePropertyChanged("Current");
                RaisePropertyChanged("Progress");
                RaisePropertyChanged("FloatProgress");
            }
        }

        public int Progress 
        { 
            get 
            {
                return _total > 0 ? (_current * 100 / _total) : 0;
            }
        }

        public double FloatProgress
        {
            get
            {
                return _total > 0 ? ((double)_current / (double)_total) : 0;
            }
        }

        public void ThreadSafeIncrementCurrent() 
        {
            Interlocked.Increment(ref _current);
            RaisePropertyChanged("Current");
            RaisePropertyChanged("Progress");
            RaisePropertyChanged("FloatProgress");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
