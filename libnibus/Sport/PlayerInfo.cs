using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Services;

namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Информация об игроке.
    /// </summary>
    public sealed class PlayerInfo
    {
        private const int RoleOfs = 0;
        private const int IndexOfs = 2;
        private const int NumberOfs = 3;
        private const int FunctionOfs = 4;
        private const int TextOfs = 5;
        private const int NameMaxLength = 30;
        private const int CountryMaxLength = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInfo"/> class.
        /// </summary>
        /// <param name="role">Принадлежность команде.</param>
        /// <param name="index">Индекс в списке (начинается с 0).</param>
        /// <param name="number">Номер игрока.</param>
        /// <param name="function">Функция игрока в команде.</param>
        /// <param name="name">Имя игрока.</param>
        /// <param name="country">Страна/город.</param>
        /// <param name="info">Дополнительная информация.</param>
        public PlayerInfo(
            TeamRole role,
            int index,
            int number,
            PlayerFunction function,
            string name,
            string country,
            string info)
        {
            Role = role;
            Index = index;
            Number = number;
            Function = function;
            Name = name ?? string.Empty;
            Country = country ?? string.Empty;
            Info = info ?? string.Empty;
        }

        internal PlayerInfo(IList<byte> nmsData)
        {
            Role = (TeamRole)nmsData[RoleOfs];
            Index = nmsData[IndexOfs];
            Number = NmsMessage.UnpackByte(nmsData[NumberOfs]);
            Function = (PlayerFunction)nmsData[FunctionOfs];
            var i = 0;
            var splits = from b in nmsData.Skip(TextOfs)
                         group b by b == 0 ? i++ : i
                         into part
                         select Encoding.Default.GetString(part.TakeWhile(b => b != 0).ToArray());
            var lines = splits.Take(3).ToArray();
            Contract.Assume(lines.Length == 3);
            Name = lines[0];
            Country = lines[1];
            Info = lines[2];
        }

        /// <summary>
        /// Возвращает принадлежность к команде.
        /// </summary>
        public TeamRole Role { get; private set; }

        /// <summary>
        /// Возвращает индекс игрока в списке.
        /// </summary>
        /// <remarks>Нумерация начинается с <c>0</c>.</remarks>
        public int Index { get; private set; }

        /// <summary>
        /// Возвращает номер игрока.
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Функция игрока.
        /// </summary>
        public PlayerFunction Function { get; private set; }

        /// <summary>
        /// Фамилия игрока.
        /// </summary>
        /// <remarks>Не более 30 символов.</remarks>
        public string Name { get; private set; }

        /// <summary>
        /// Страна/город.
        /// </summary>
        /// <remarks>Не более 20 символов.</remarks>
        public string Country { get; private set; }

        /// <summary>
        /// Возвращает дополнительную информацию об игроке.
        /// </summary>
        /// <remarks>
        /// Максимальная длина <see cref="NmsMessage.NmsMaxDataLength"/> минус длина <see cref="Name"/>
        /// минус длина <see cref="Country"/> минус 9.</remarks>
        public string Info { get; private set; }

        internal byte[] GetData()
        {
            var text = new List<byte>(Name.Length + Country.Length + Info.Length + 3);
            text.AddRange(Encoding.Default.GetBytes(Name).Take(NameMaxLength));
            text.Add(0);
            text.AddRange(Encoding.Default.GetBytes(Country).Take(CountryMaxLength));
            text.Add(0);
            var rest = NmsMessage.NmsMaxDataLength - text.Count - (TextOfs + 1) - 1;
            text.AddRange(Encoding.Default.GetBytes(Info).Take(rest));
            text.Add(0);

            var data = new byte[(TextOfs + 1) + text.Count];

            data[RoleOfs] = (byte)Role;
            data[IndexOfs] = (byte)Index;
            data[NumberOfs] = NmsMessage.PackByte(Number);
            data[FunctionOfs] = (byte)Function;
            text.CopyTo(data, TextOfs);

            return data;
        }
    }

    /// <summary>
    /// Расширение для конвертации информации об игроке.
    /// </summary>
    internal static class PlayerInfoExtensions
    {
        public static NmsInformationReport CreateInformationReport(this PlayerInfo info, Address source = null)
        {
            return new NmsInformationReport(
                source ?? Address.Empty,
                (int)GameReports.PlayerInfo,
                NmsValueType.UInt8Array,
                info.GetData());
        }

        public static PlayerInfo GetPlayerInfo(this NmsInformationReport report)
        {
            Contract.Requires(report.Id == (byte)GameReports.PlayerInfo);
            return new PlayerInfo((byte[])report.Value);
        }
    }
}