//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// M1702.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global
namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// model 0x0004xxxx | НАТА-1702 - тяжелая атлетика
    /// </summary>
    public enum M1702
    {
        /// <summary>
        /// VT_LPSTR | вид упражнения
        /// </summary>
        Excercise = 0x100,

        /// <summary>
        /// VT_UI2 | вес на щтанге
        /// </summary>
        Weight = 0x101,

        /// <summary>
        /// VT_LPSTR | весовая категория
        /// </summary>
        WeightCategory = 0x102,

        /// <summary>
        /// VT_LPSTR | фамилия спортсмена
        /// </summary>
        Name = 0x103,

        /// <summary>
        /// VT_LPSTR | страна
        /// </summary>
        Country = 0x104,

        /// <summary>
        /// VT_UI1 | попытка
        /// </summary>
        Attempt = 0x105,

        /// <summary>
        /// VT_DATE | время попытки
        /// </summary>
        AttemptTime = 0x106,
    }
}