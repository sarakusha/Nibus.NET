namespace NataInfo.Nibus.Nms
{
    /// <summary>
    /// Типы значений, используемых в NMS-сообщениях.
    /// </summary>
    public enum NmsValueType : byte
    {
        /// <summary>
        /// Логический тип. 8 бит Значение TRUE = 1/FALSE = 0
        /// </summary>
        Boolean = 11,

        /// <summary>
        /// Байт со знаком. 8 Бит.
        /// </summary>
        Int8 = 16,

        /// <summary>
        /// Знаковое короткое целое. 16 бит.
        /// </summary>
        Int16 = 2,

        /// <summary>
        /// Знаковое целое. 32 бита.
        /// </summary>
        Int32 = 3,

        /// <summary>
        /// Знаковое длинное целое. 64 бита.
        /// </summary>
        Int64 = 20, // 
        /// <summary>
        /// Байт. 8 бит.
        /// </summary>
        UInt8 = 17,

        /// <summary>
        /// Короткое целое. 16 бит.
        /// </summary>
        UInt16 = 18,

        /// <summary>
        /// Целое. 32 бита.
        /// </summary>
        UInt32 = 19,

        /// <summary>
        /// Длинное целое. 64 бита.
        /// </summary>
        UInt64 = 21,

        /// <summary>
        /// Значение с плавающей точкой. 32 бита.
        /// </summary>
        Real32 = 4,

        /// <summary>
        /// Значение с плавающей точкой удвоенной точности. 64 бита.
        /// </summary>
        Real64 = 5,

        /// <summary>
        /// Строка символов с терминирующим нулем.
        /// </summary>
        String = 30,

        /// <summary>
        /// Дата/время. 80 бит.
        /// </summary>
        DateTime = 7,

        /// <summary>
        /// Флаговый бит массива. Если длина массива не фиксированна, считается равной длине оставшегося сообщения.
        /// </summary>
        Array = 0x80,

        /// <summary>
        /// Массив булевых значений.
        /// </summary>
        BooleanArray = Boolean | Array,

        /// <summary>
        /// Массив байт со знаком.
        /// </summary>
        Int8Array = Int8 | Array,

        /// <summary>
        /// Массив коротких целых чисел со знаком.
        /// </summary>
        Int16Array = Int16 | Array,

        /// <summary>
        /// Массив целых чисел со знаком.
        /// </summary>
        Int32Array = Int32 | Array,

        /// <summary>
        /// Массив длинных целых чисел со знаком.
        /// </summary>
        Int64Array = Int64 | Array,

        /// <summary>
        /// Массив байт.
        /// </summary>
        UInt8Array = UInt8 | Array,

        /// <summary>
        /// Массив коротких целых чисел.
        /// </summary>
        UInt16Array = UInt16 | Array,

        /// <summary>
        /// Массив целых чисел.
        /// </summary>
        UInt32Array = UInt32 | Array,

        /// <summary>
        /// Массив длинных целых чисел.
        /// </summary>
        UInt64Array = UInt64 | Array,

        /// <summary>
        /// Массив чисел с плавающей точкой.
        /// </summary>
        Real32Array = Real32 | Array,

        /// <summary>
        /// Массив чисел с плавающей точкой удвоенной точности.
        /// </summary>
        Real64Array = Real64 | Array
    }
}