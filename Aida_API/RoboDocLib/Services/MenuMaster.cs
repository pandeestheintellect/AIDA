using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using RoboDocLib.Parsers;

namespace RoboDocLib.Services
{
    public class MenuMaster
    {
        string connectionString = "";
        int UserId = 99;
        public MenuMaster(string connection)
        {
            connectionString = connection;
        }

        public List<AppMenuModel> GetMenu(string role)
        {

            List<AppMenuModel> response = new List<AppMenuModel>();

            response.Add(new AppMenuModel()
            {
                Name = "Dashboard",
                Link = "/secured/dashboards/company",
                Open = false,
                Color= "parent-menu-color",
                Icon = new IconElementModel() { IconName = "tachometer-alt", Prefix = "fas" }
            });

            response.Add(new AppMenuModel()
            {
                Name = "Client View",
                Link = "/secured/dashboards/client-view",
                Open = false,
                Color = "parent-menu-color",
                Icon = new IconElementModel() { IconName = "street-view", Prefix = "fas" }
            });


            return response;
        }
    }
}
