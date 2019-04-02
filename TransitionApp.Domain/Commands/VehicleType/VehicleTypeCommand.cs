using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Commands.VehicleType
{
    public abstract class VehicleTypeCommand : BaseCommand
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        #endregion

    }
}
