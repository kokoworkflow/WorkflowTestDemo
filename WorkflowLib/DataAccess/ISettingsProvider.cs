using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowLib.Model;

namespace WorkflowLib.DataAccess
{
    public interface ISettingsProvider
    {
        Settings GetSettings();
    }
}
