using EPPlus.Core.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.RequestModel.Driver
{
    public class DriverExcelModel
    {
        [ExcelTableColumn("Mã Lái xe")]
        public string Code { get; set; }

        [ExcelTableColumn("Họ và tên")]
        public string Name { get; set; }

        [ExcelTableColumn("Giới tính")]
        public string Sex { get; set; }

        [ExcelTableColumn("Ngày tháng năm sinh")]
        public DateTime DoB { get; set; }

        [ExcelTableColumn("Số thẻ căn cước")]
        public string IDCardNumber { get; set; }

        [ExcelTableColumn("Số điện thoại")]
        public string PhoneNumber { get; set; }
    }
}
