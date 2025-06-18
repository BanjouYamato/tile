# MusicTile - Unity Project

## Hướng dẫn chạy dự án

### Cách mở project trong Unity
1. Mở **Unity Hub**.
2. Nhấn **Add**, sau đó dẫn đến thư mục chứa project: MusicTile
3. Chọn đúng Unity phiên bản **2022.3.40f1** để đảm bảo tương thích.
4. Mở scene chính tại đường dẫn: Assets/Scenes/GamePlay.unity
5. Nhấn nút **Play** để chạy game.

## Điều khiển chính

- **Game điều khiển hoàn toàn bằng chuột**:
- **Click chuột** vào tile đúng lúc để đạt điểm (Perfect/Great/Good tuỳ thời điểm).

## Giải thích thiết kế

### 1. Vì sao chọn cấu trúc class hiện tại?

- Dự án sử dụng **phân chia class rõ ràng theo trách nhiệm**:
- Các class thuộc loại **Manager** (như `TileManager`, `MusicManager`, `SpawnManager`, v.v.) có nhiệm vụ **điều khiển luồng vận hành tổng thể**: quản lý tile rơi, phát nhạc, spawn tile,...
- Các class thuộc loại **Object** (như `Tile`, VFX,...) chỉ chịu trách nhiệm **xử lý trạng thái riêng của từng đối tượng**: tile xuất hiện, rơi, được click, v.v.
- Cách tổ chức này giúp **dễ bảo trì**, **mở rộng game**, và **tách biệt rõ vai trò** giữa logic tổng thể và hành vi cá nhân.

### 2. Design Pattern được sử dụng

-  **Singleton Pattern**:  
Được sử dụng cho các manager toàn cục như `GameManager`, `SFXManager` để đảm bảo chỉ có một instance duy nhất tồn tại trong toàn bộ game.

-  **Observer Pattern**:  
Được dùng để **theo dõi các sự kiện** trong game như tile được nhấn, kết thúc nhạc, update điểm số,... Giúp giảm phụ thuộc giữa các class.

-  **Object Pooling**:  
Dùng để **tái sử dụng tile thay vì tạo/destroy liên tục**, giúp cải thiện hiệu suất đáng kể, đặc biệt khi số lượng tile nhiều và spawn liên tục.

### 3. Cách đồng bộ nhạc & tile

- Game sử dụng **nhịp (beat) từ nhạc để tạo tile**:
- Một **tool tùy chỉnh** được tạo ra để lưu lại:
 - **Thời gian bắt đầu** của từng nốt nhạc (được xem là **hit time** – thời điểm nên click tile).
 - **Thời gian kết thúc** của nốt nhạc (nếu có giữ phím).

- Việc tile rơi được đồng bộ với nhạc như sau:
1. **Điểm spawn** của tile được đặt **cố định trên trục Y**.
2. Một đường gọi là **Perfect Line** là vị trí người chơi cần click dể tính perfect.
3. Dựa vào **khoảng cách từ điểm spawn đến perfect line**, tính ra **thời gian rơi cần thiết** để tile chạm đúng lúc với beat đã lưu.
4. Từ đó, dựa vào **BPM (beats per minute)**, tính ra tốc  rơi hợp lý** để đồng bộ toàn bộ tile theo nhạc.
5. Do tile **spawn ngay từ đầu**, game tính thêm **thời gian delay khi phát nhạc**, để **tile đầu tiên rơi xuống khớp với beat đầu tiên**.


