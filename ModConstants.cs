using System.Reflection;
using Artitas.Utils;
using log4net;

namespace SoldierTotalColumn
{
    public static class ModConstants
    {
        public const string LogPrefix = "[SoldierTotalColumn]";

        public static readonly ILog Log = ArtitasLogger.GetLogger(
            MethodBase.GetCurrentMethod()!.DeclaringType
        );
    }
}
