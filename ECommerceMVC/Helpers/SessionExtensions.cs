using System.Text.Json;

namespace ECommerceMVC.Helpers
{
    public static class SessionExtensions
    {
        // Hàm lưu dữ liệu vào Session (ép thành chuỗi JSON)
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Hàm lấy dữ liệu từ Session ra (giải nén từ JSON)
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}