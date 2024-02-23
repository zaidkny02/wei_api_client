using System.ComponentModel.DataAnnotations;

namespace webapi_client_211223.Models
{
    public class KhachHangModel
    {

        [Key]
        public int iMaKH { get; set; }
        public string? sTenKH { get; set; }
        public string? sDiachi { get; set; }
        public string? sDienthoai { get; set; }
        public bool bGioitinh { get; set; }
        public int iTuoi { get; set; }

        public KhachHangModel(int iMaKH, string? sTenKH, string? sDiachi, string? sDienthoai, bool bGioitinh, int iTuoi)
        {
            this.iMaKH = iMaKH;
            this.sTenKH = sTenKH;
            this.sDiachi = sDiachi;
            this.sDienthoai = sDienthoai;
            this.bGioitinh = bGioitinh;
            this.iTuoi = iTuoi;
        }
        public KhachHangModel() { }
    }
}
