using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Informer.Utils;
using SQLite;

namespace Informer.Models
{
    public class Group: INotifyPropertyChanged 
    {
        [PrimaryKey]
        public int Id { get; set; } = 0;
        public string Name { get; set; } = null;
        public string Date { get; set; } = null;
        public string Order { get; set; } = null;

        [Ignore]
        public Map Map { get; set; } = new Map();

        [Ignore]
        public Schema Schema { get; set; } = new Schema();

        [Ignore]
        public Plan Plan { get; set; } = new Plan();


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

        [Column("Map")]
        public String _db_Map
        {
            get { return Serializer.Serialize(Map); }
            set { Map = Serializer.Deserialize<Map>(value); }
        }

        [Column("Schema")]
        public String _db_Schema
        {
            get { return Serializer.Serialize(Schema); }
            set { Schema = Serializer.Deserialize<Schema>(value); }
        }

        [Column("Plan")]
        public String _db_Plan
        {
            get { return Serializer.Serialize(Plan); }
            set { Plan = Serializer.Deserialize<Plan>(value); }
        }





        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}