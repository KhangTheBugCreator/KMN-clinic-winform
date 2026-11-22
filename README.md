# KMN Clinic – Phần mềm quản lý phòng khám (Windows Forms + ADO.NET)

Bản desktop của hệ thống quản lý phòng khám KMN Clinic, dùng chung CSDL SQL Server `PHONGKHAM`
với website (repo `kmn-clinic`). Không đổi schema: 11 bảng, CHECK, UNIQUE và 6 trigger giữ nguyên.

## Tình trạng

**Giai đoạn 2 – Khung giao diện (dữ liệu giả).** Chạy được, chưa nối CSDL. Dùng để duyệt bố cục và chụp ảnh cho Chương 3.
Các module W0–W9 (nối CSDL qua BUS/DAL) làm tiếp theo kế hoạch.

## Chạy trên Windows

Cần: Windows 10/11, .NET 10 SDK.

```powershell
git clone https://github.com/KhangTheBugCreator/KMN-clinic-winform.git
cd KMN-clinic-winform
git checkout giai-doan-2-khung-giao-dien
dotnet run --project src/KmnClinic.GUI
```

Hoặc mở `KmnClinic.WinForms.sln` bằng Visual Studio 2026 (workload ".NET desktop development") rồi bấm F5.

Tài khoản thử (mật khẩu đều là `matkhau123`):

| Vai trò | Số điện thoại | Màn hình dựng ở Giai đoạn 2 |
| --- | --- | --- |
| Quản trị viên | 0907112233 | Tổng quan, Thuốc (+ form thêm/sửa), Lịch làm việc |
| Lễ tân | 0932145678 | Lịch hẹn (xác nhận, hủy), Đặt lịch hộ, Tiếp nhận (có hẹn, vãng lai) |
| Bác sĩ | 0903126789 | Hàng chờ khám, Khám bệnh và kê đơn, Lịch trực của tôi |
| Thu ngân | 0708671234 | Hóa đơn chờ thu, Thu tiền, Xem trước bản in hóa đơn |

Các mục menu còn lại hiện trang "sẽ dựng ở module Wx".

## Cấu trúc

```
database/                 script cấu trúc + dữ liệu mẫu (chép từ repo web)
src/KmnClinic.DTO/        lớp dữ liệu dùng chung (KhoaDTO, ThuocDTO, LichHenDTO...)
src/KmnClinic.GUI/        Windows Forms
  Forms/                  frmDangNhap, frmChinh, frmThuocChiTiet, frmDatLichHo, frmKhamBenh, frmThuTien
  Views/                  các UserControl hiện trong form chính (uc...)
  Controls/               control tự vẽ: TheThongKe, KhungTrang, BieuDoCot
  Theme/                  màu KMN Clinic (giống theme.css của web), hàm định dạng chung
  DuLieuGia/              dữ liệu giả của Giai đoạn 2 (sẽ bỏ khi nối CSDL)
```

Từ module W0 sẽ thêm `KmnClinic.DAL` (ADO.NET: SqlConnection, SqlCommand, SqlTransaction),
`KmnClinic.BUS` (quy tắc nghiệp vụ, kiểm tra vai trò) và `KmnClinic.Tests`.

Form viết theo chuẩn `*.Designer.cs` nên mở được bằng Windows Forms Designer của Visual Studio.
