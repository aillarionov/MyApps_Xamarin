using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(ViewCell), typeof(Informer.iOS.CustomAllViewCellRendereriOS))]
namespace Informer.iOS
{
    public class CustomAllViewCellRendereriOS : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            if (cell != null)
            {
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }


            return cell;
        }
    }
}
