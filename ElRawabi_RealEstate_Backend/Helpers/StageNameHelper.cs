namespace ElRawabi_RealEstate_Backend.Helpers
{
    public static class StageNameHelper
    {
        public static string ToArabic(string key) => key switch
        {
            "SitePreparation" => "تجهيز الموقع والحفر",
            "Foundation" => "الأساسات",
            "Structure" => "الهيكل الإنشائي",
            "MasonryAndWalls" => "المباني والحوائط",
            "InitialFinishing" => "التشطيبات الأولية",
            "FinalFinishing" => "التشطيبات النهائية",
            "Handover" => "التسليم",
            _ => key
        };
    }
}