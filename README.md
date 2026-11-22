# KMN Clinic – Phần mềm quản lý phòng khám (Windows Forms + ADO.NET)

Bản desktop của hệ thống quản lý phòng khám KMN Clinic, dùng chung CSDL SQL Server `PHONGKHAM`
với website (repo `kmn-clinic`). Không đổi schema: 11 bảng, CHECK, UNIQUE và 6 trigger giữ nguyên.

## Tình trạng

- **Giai đoạn 2 – Khung giao diện:** xong. Giao diện kiểu WinForms cổ điển: menu trên cùng, thanh trạng thái, control mặc định.
- **W0 – Nền tảng:** kết nối CSDL bằng ADO.NET, đăng nhập thật (mật khẩu băm PBKDF2), đổi mật khẩu, phân quyền ở tầng BUS.
- **W1 – Danh mục:** Khoa, Phòng làm việc, Ca làm việc, Thuốc đọc/ghi CSDL thật (menu Danh mục của Quản trị viên).
- **W2 – Nhân viên, khách hàng:** Quản trị viên quản lý nhân viên (menu Nhân sự); Quản trị viên và Lễ tân quản lý khách hàng.
- Các màn hình nghiệp vụ khác vẫn dùng dữ liệu giả (`DuLieuGia/`), sẽ nối CSDL lần lượt ở W3–W9.

## Chạy trên Windows

Cần: Windows 10/11, .NET 10 SDK, SQL Server (bản Developer/Express đều được) và SQL Server Management Studio (SSMS).

### 1. Tạo CSDL

Mở SSMS, kết nối vào SQL Server trên máy, rồi chạy lần lượt (File → Open → File…, bấm Execute):

1. `database/cau-truc-phongkham.sql` – tạo CSDL `PHONGKHAM`, 11 bảng, ràng buộc, trigger.
2. `database/du-lieu-mau-day-du.sql` – dữ liệu mẫu.

Nếu đã có CSDL `PHONGKHAM` từ website thì bỏ qua bước này, hai bản dùng chung được.

### 2. Sửa chuỗi kết nối

Mở `src/KmnClinic.GUI/appsettings.json`, sửa `Server=` cho đúng tên SQL Server trên máy:

| Bản SQL Server | Giá trị `Server` |
| --- | --- |
| Developer / bản đầy đủ (mặc định) | `.` |
| Express | `.\\SQLEXPRESS` (trong JSON phải viết hai dấu `\`) |
| LocalDB | `(localdb)\\MSSQLLocalDB` |

Mặc định đăng nhập SQL Server bằng tài khoản Windows (`Trusted_Connection=True`).
Muốn dùng tài khoản `sa` thì thay bằng `User Id=sa;Password=...`.
File được chép cạnh `KmnClinic.exe` khi build, nên sau này có thể sửa thẳng file trong thư mục `bin`.

### 3. Chạy

```powershell
git clone https://github.com/KhangTheBugCreator/KMN-clinic-winform.git
cd KMN-clinic-winform
dotnet run --project src/KmnClinic.GUI
```

Hoặc mở `KmnClinic.WinForms.sln` bằng Visual Studio 2026 (workload ".NET desktop development"), chọn `KmnClinic.GUI` làm Startup Project rồi bấm F5.

Tài khoản dữ liệu mẫu (mật khẩu đều là `matkhau123`; lần đăng nhập đầu phần mềm tự băm lại mật khẩu trong CSDL):

| Vai trò | Số điện thoại |
| --- | --- |
| Quản trị viên | 0907112233 |
| Lễ tân | 0932145678 |
| Bác sĩ | 0903126789 |
| Thu ngân | 0708671234 |

Đổi mật khẩu: menu **Hệ thống → Đổi mật khẩu**.

### 4. Chạy kiểm thử

Kiểm thử chạy trên CSDL thật, tự thêm rồi xóa nhân viên thử (SĐT `09999…`), không sửa dữ liệu mẫu.

```powershell
# Mặc định dùng Server=. và đăng nhập Windows. Máy khác thì đặt biến môi trường trước:
# $env:KMN_TEST_DB = "Server=.\SQLEXPRESS;Database=PHONGKHAM;Trusted_Connection=True;TrustServerCertificate=True"
dotnet test
```

## Cấu trúc (3 lớp: GUI → BUS → DAL, DTO dùng chung)

```
database/                 script cấu trúc + dữ liệu mẫu (chép từ repo web)
src/KmnClinic.DTO/        lớp dữ liệu dùng chung (KhoaDTO, ThuocDTO, LichHenDTO...)
src/KmnClinic.DAL/        ADO.NET: SqlConnection, SqlCommand có tham số, SqlTransaction, SqlDataAdapter
  DbHelper.cs             mở kết nối, chạy trong transaction (thử lại khi deadlock 1205), đổ DataTable
  MaSoGenerator.cs        sinh mã KH0001, NV0012... (UPDLOCK + HOLDLOCK để không trùng)
  NhanVienDAL.cs          đăng nhập, đổi mật khẩu, quản lý nhân viên (W2)
  KhachHangDAL.cs         khách hàng (W2), không đọc/ghi mat_khau_kh
  KhoaDAL, PhongDAL, CaDAL, ThuocDAL   danh mục (W1); kiểm tra "đang được dùng" và ghi trong cùng transaction
src/KmnClinic.BUS/        quy tắc nghiệp vụ
  NhanVienBUS.cs          đăng nhập, đổi mật khẩu, quản lý nhân viên (RB5, RB6)
  KhachHangBUS.cs         khách hàng: QTV + Lễ tân thêm/sửa, chỉ QTV xóa
  KhoaBUS, PhongBUS, CaBUS, ThuocBUS   quy tắc danh mục (RB3, RB11, Q8), quyền xem/ghi theo vai trò
  MatKhau.cs              băm PBKDF2-SHA256 (cùng định dạng với website)
  PhienDangNhap.cs        nhân viên đang đăng nhập
  PhanQuyen.cs            kiểm tra vai trò trước mỗi thao tác ghi
  DichLoiSql.cs           đổi lỗi SQL Server (trigger, UNIQUE, CHECK) thành câu tiếng Việt
  LoiNghiepVu.cs          lỗi nghiệp vụ hiện cho người dùng
src/KmnClinic.GUI/        Windows Forms
  appsettings.json        chuỗi kết nối
  Forms/                  frmDangNhap, frmDoiMatKhau, frmChinh, frmNhanVienChiTiet, frmKhachHangChiTiet, frmThuocChiTiet, frmDatLichHo, frmKhamBenh, frmThuTien
  Views/                  các UserControl hiện trong form chính (ucKhoa, ucPhong, ucCa, ucThuoc...)
  Controls/               TheThongKe (ô số liệu), BieuDoCot (biểu đồ cột vẽ bằng GDI+)
  Theme/                  hàm dùng chung (cấu hình lưới, định dạng tiền)
  DuLieuGia/              dữ liệu giả cho các màn hình chưa nối CSDL (bỏ dần ở W1–W9)
tests/KmnClinic.Tests/    xUnit, chạy trên CSDL thật
```

Form viết theo chuẩn `*.Designer.cs` nên mở được bằng Windows Forms Designer của Visual Studio.
