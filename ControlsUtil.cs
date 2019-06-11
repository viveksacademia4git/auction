using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Auction
{
    class ControlsUtil
    {
        private bool isAdmin = false;

        public ControlsUtil(int userType)
        {
            isAdmin = userType==UserType.Admin;
        }

        public void chkControlItemSold(CheckBox chk)
        {
            chk.IsEnabled = isAdmin;
        }
    }
}
