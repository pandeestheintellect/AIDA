using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocCore.Models
{
    public class LoginResponseModel : ResponseModel
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string SessionId { get; set; }
        public int TimeDiff { get; set; }
        public List<AppMenuModel> Menus { get; set; }
    }

    public class AppMenuModel: MenuElementModel
    {
        public List<MenuElementModel> Sub { get; set; }
    }
    
    public class MenuElementModel
    {
        public string Name { get; set; }
        public IconElementModel Icon { get; set; }
        public string Link { get; set; }
        public bool Open { get; set; }
        public string Color { get; set; }
}

    public class IconElementModel
    {
        public string IconName { get; set; }
        public string Prefix{ get; set; }
    }

}
