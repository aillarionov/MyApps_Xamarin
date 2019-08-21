using System;
using SQLite;

namespace Informer.Models
{
    public class Config
    {
        [PrimaryKey]
        public int GroupId { get; set; }

        public bool IsPresenter { get; set; }
        public bool IsVisitor { get; set; }
        public bool ReceivePush { get; set; }

        public String LastChange { get; set; }


    }
}
