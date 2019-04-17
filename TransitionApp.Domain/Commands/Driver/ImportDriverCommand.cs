using System;
using System.Collections.Generic;

namespace TransitionApp.Domain.Commands.Driver
{
    public class ImportDriverCommand : BaseCommand
    {
        public List<DataImportDriver> Drivers { get; set; }
        public override bool IsValid(object obj = null)
        {
            throw new NotImplementedException();
        }

    }
    public class DataImportDriver
    {
        public DateTime StartDate { get; set; }
        public int Status { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime DoB { get; set; }
        public string IDCardNumber { get; set; }
        public string PhoneNumber { get; set; }

    }
}
