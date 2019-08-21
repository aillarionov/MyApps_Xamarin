using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Informer.Models
{
    public interface IItem
    {
        Task FinalizeLoad(List<Uri> uris, CancellationToken cancellationToken);
        string GetText();
        string GetRaw();
        void SetFavorite(Favorite favorite);
        bool IsFavoriteForThis(Favorite favorite);

    }
}
