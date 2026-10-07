using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MiPlugin.Domain.Models;
using MiPlugin.Ribbon;
using System.Security.Cryptography.X509Certificates;

namespace MiPlugin
{
    public class App : IExternalApplication
    {

        public Result OnShutdown(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // new CreateButtonNoStatic(application).CreateTabPanelButton();
            CreateButton.CreateTabPanelButton(application);
            new CreateButton1(application).Create("Second", "Attempt", "Working", "4");
            
            return Result.Succeeded;
        }
    }
}
