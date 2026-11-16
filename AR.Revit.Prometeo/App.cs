using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AR.Revit.Prometeo.Ribbon;
using System.Security.Cryptography.X509Certificates;

namespace AR.Revit.Prometeo
{
    public class App : IExternalApplication
    {

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            CreateButton.
            CreateButton.CreateTabPanelButton(application, "AR_Tab", "AR_Panel", "AR_Medir", typeof(Command.ButtonCommand).FullName);
            CreateButton.CreateTabPanelButton(application, "AR_Tab", "AR_Panel", "AR_Parametros", typeof(Command.ParamButton).FullName);

            return Result.Succeeded;
        }

    }
}
