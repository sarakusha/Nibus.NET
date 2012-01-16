namespace NataInfo.Nibus.Nms
{
    public enum NmsValueType : byte
    {
        Boolean = 11, // 8 бит «начение TRUE = 1/FALSE = 0
        Int8 = 16, // 8 бит «наковый байт
        Int16 = 2, // 16 бит «наковое короткое целое
        Int32 = 3, // 32 бита «наковое целое
        Int64 = 20, // 64 бита «наковое длинное целое
        UInt8 = 17, // 8 бит Ѕайт
        UInt16 = 18, // 16 бит  ороткое целое
        UInt32 = 19, // 32 бита ÷елое
        UInt64 = 21, // 64 бита ƒлинное целое
        Real32 = 4, // 32 бита «начение с плавающей точкой
        Real64 = 5, // 64 бита «начение с плавающей точкой удвоенной точности
        String = 30, // —трока символов с терминирующим нулем
        DateTime = 7,
        // 80 бит ƒата/врем€ в формате BCD
        // DD-MM-YYYY HH:MM:SS.0mmmbW
        // DD Ц дата
        // MM Ц мес€ц
        // YYYY Ц год
        // HH Ц час (0..23)
        // MM Ц минуты
        // SS Ц секунды
        // mmm Ц миллисекунды
        // W Ц день недели (1..7,
        // 1 Ц вс,
        // 2 Ц пн,
        // Е 7 Ц сб)
        // b Ц зарезервировано
        Array = 0x80,
        BooleanArray = Boolean | Array,
        Int8Array = Int8 | Array,
        Int16Array = Int16 | Array,
        Int32Array = Int32 | Array,
        Int64Array = Int64 | Array,
        UInt8Array = UInt8 | Array,
        UInt16Array = UInt16 | Array,
        UInt32Array = UInt32 | Array,
        UInt64Array = UInt64 | Array,
        Real32Array = Real32 | Array,
        Real64Array = Real64 | Array
    }
}