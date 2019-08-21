using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using SQLite;

namespace Informer.Models
{
    public class Item: IItem, INotifyPropertyChanged 
    {
        public int GroupId { get; set; } = 0;
        public int AlbumId { get; set; } = 0;
        public int Id { get; set; } = 0;

        public string Text { get; set; } = null;
        public string Raw { get; set; } = null;

        [Ignore]
        public string Caption 
        {
            get 
            {
                return this.GetCaption();
            }
        }

        [Ignore]
        public Photo Photo { get; set; } = null;
        [Ignore]
        public List<Photo> Photos { get; set; } = new List<Photo>();

        private Favorite _Favorite = null;
        [Ignore]
        public Favorite Favorite
        {
            get { return _Favorite; }
            set 
            {
                _Favorite = value;
                RaisePropertyChanged("Favorite");
            }
        }


        [Column("Photo")]
        public String _db_Photo
        {
            get { return Photo != null ? Photo.Serialize() : null; }
            set {
                if (String.IsNullOrEmpty(value))
                    Photo = null;
                else 
                {
                    Photo = new Photo();
                    Photo.Deserialize(value);
                }
            }
        }

        [Column("Photos")]
        public String _db_Photos
        {
            get
            {
                if (Photos != null && Photos.Count > 0)
                {
                    List<String> r = new List<string>();
                    foreach (Photo photo in Photos)
                    {
                        r.Add(photo.Serialize());
                    }

                    return String.Join("|", r);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    Photos = new List<Photo>();
                else
                {
                    Photos = new List<Photo>();

                    foreach (String s in value.Split('|'))
                    {
                        Photo p = new Photo();
                        p.Deserialize(s);
                        Photos.Add(p);
                    }
                }
            }
        }



        public async Task FinalizeLoad(List<Uri> uris, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (this.Photo != null)
            {
                await this.Photo.FinalizeLoad(uris, cancellationToken).ConfigureAwait(false);
            }

            foreach (Photo photo in Photos) 
            {
                await photo.FinalizeLoad(uris, cancellationToken).ConfigureAwait(false);   
            }
        }

        public string GetText() 
        {
            return this.Text;
        }

        public string GetRaw()
        {
            return this.Raw;
        }

        protected virtual string GetCaption()
        {
            return this.Text;
        }
            
        public void SetFavorite(Favorite favorite)
        {
            this.Favorite = favorite;
        }

        public bool IsFavoriteForThis(Favorite favorite)
        {
            return Favorite.IsFavoriteForItem(favorite, this);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
