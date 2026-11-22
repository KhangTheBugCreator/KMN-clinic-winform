-- Cho phép Khach_hang.gioi_tinh_kh nhận NULL (giới tính "Khác"), áp dụng cho CSDL PHONGKHAM đã tạo trước 22/11/2026.
-- Chạy một lần trong SSMS. Không mất dữ liệu: các khách hiện có vẫn giữ Nam/Nữ.
-- CHECK CK_Khach_hang_gioi_tinh (Nam hoặc Nữ) giữ nguyên: NULL không vi phạm CHECK nên vẫn lưu được.
USE PHONGKHAM;
GO
IF EXISTS (SELECT 1 FROM sys.columns
           WHERE object_id = OBJECT_ID(N'dbo.Khach_hang') AND name = N'gioi_tinh_kh' AND is_nullable = 0)
    ALTER TABLE dbo.Khach_hang ALTER COLUMN gioi_tinh_kh nvarchar(5) NULL;
GO
