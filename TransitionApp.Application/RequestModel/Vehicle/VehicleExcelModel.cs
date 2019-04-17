using EPPlus.Core.Extensions.Attributes;

namespace TransitionApp.Application.RequestModel.Vehicle
{
    public class VehicleExcelModel
    {
        [ExcelTableColumn("Mã")]
        public string Code { get; set; }
        [ExcelTableColumn("Biển số")]
        public string LicensePlate { get; set; }
        [ExcelTableColumn("Tên loại xe")]
        public string Name { get; set; }
        [ExcelTableColumn("Loại phương tiện")]
        public string TypeVehicle { get; set; }
        [ExcelTableColumn("Người sử dụng")]
        public string UserOwner { get; set; }
        [ExcelTableColumn("Tải trọng tối đa (kg)")]
        public double MaxLoad { get; set; }
        [ExcelTableColumn("ghi chú")]
        public string Note { get; set; }
        [ExcelTableColumn("Mã lái xe")]
        public string CodeDriver { get; set; }


    }
}
