using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace Informer.Models
{
    public class SimpleGroup: INotifyPropertyChanged 
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = null;
        public string Date { get; set; } = null;
        public string Order { get; set; } = null;

        [Ignore]
        public Photo Logo { get; set; } = null;

        [Column("Logo")]
        public String _db_Logo
        {
            get { return Logo != null ? Logo.Serialize() : null; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    Logo = null;
                else
                {
                    Logo = new Photo();
                    Logo.Deserialize(value);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
