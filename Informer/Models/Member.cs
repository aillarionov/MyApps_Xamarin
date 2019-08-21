using System;
using System.Collections.Generic;
using SQLite;

namespace Informer.Models
{
    public class Member : Item, IItem
    {
        public String Name { get; set; }

        [Ignore]
        public List<String> Categories { get; set; } = new List<String>();
        [Ignore]
        public List<String> Phones { get; set; } = new List<String>();
        [Ignore]
        public List<String> Emails { get; set; } = new List<String>();
        [Ignore]
        public List<String> Sites { get; set; } = new List<String>();
        [Ignore]
        public List<String> VKs { get; set; } = new List<String>();
        [Ignore]
        public List<String> FBs { get; set; } = new List<String>();
        [Ignore]
        public List<String> Insts { get; set; } = new List<String>();

        public String Stand { get; set; }


        protected override string GetCaption()
        {
            return this.Name;
        }


        [Column("Categories")]
        public String _db_Categories
        {
            get { return Categories != null ? String.Join("|", Categories) : null; }
            set { Categories = !String.IsNullOrEmpty(value) ? new List<String>(value.Split('|')) : new List<String>(); }
        }

        [Column("Phones")]
        public String _db_Phones 
        {
            get { return Phones != null ? String.Join("|", Phones) : null; }
            set { Phones = !String.IsNullOrEmpty(value) ? new List<String>(value.Split('|')) : new List<String>(); }
        }

        [Column("Emails")]
        public String _db_Sites
        {
            get { return Sites != null ? String.Join("|", Sites) : null; }
            set { Sites = !String.IsNullOrEmpty(value) ? new List<String>(value.Split('|')) : new List<String>(); }
        }

        [Column("Sites")]
        public String _db_Emails
        {
            get { return Emails != null ? String.Join("|", Emails) : null; }
            set { Emails = !String.IsNullOrEmpty(value) ? new List<String>(value.Split('|')) : new List<String>(); }
        }

        [Column("VKs")]
        public String _db_VKs
        {
            get { return VKs != null ? String.Join("|", VKs) : null; }
            set { VKs = !String.IsNullOrEmpty(value) ? new List<String>(value.Split('|')) : new List<String>(); }
        }

        [Column("FBs")]
        public String _db_FBs
        {
            get { return FBs != null ? String.Join("|", FBs) : null; }
            set { FBs = !String.IsNullOrEmpty(value) ? new List<String>(value.Split('|')) : new List<String>(); }
        }

        [Column("Insts")]
        public String _db_Insts
        {
            get { return Insts != null ? String.Join("|", Insts) : null; }
            set { Insts = !String.IsNullOrEmpty(value) ? new List<String>(value.Split('|')) : new List<String>(); }
        }


    }
}
