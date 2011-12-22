using System.Collections.Generic;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace NataInfo.Nibus.Tests
{
    [TestFixture]
    public class AddressTests
    {
        [Test]
        public void Address_Bradcast_IsBroadcastType()
        {
            Assert.That(Address.Broadcast.Type, Is.EqualTo(AddressType.Broadcast));
        }

        [Test]
        public void Address_Empty_IsTypeEmpty()
        {
            Assert.That(Address.Empty.Type, Is.EqualTo(AddressType.Empty));
        }

        [Test]
        public void Address_Net_IsTypeNet()
        {
            var address = new Address(255, 10, 1);
            Assert.That(address.Type, Is.EqualTo(AddressType.Net));
            Assert.That(address.Domain, Is.EqualTo(255));
            Assert.That(address.Subnet, Is.EqualTo(10));
            Assert.That(address.Device, Is.EqualTo(1));
        }

        [Test]
        public void Address_Group_IsTypeGroup()
        {
            var address = new Address(1, 2);
            Assert.That(address.Type, Is.EqualTo(AddressType.Group));
            Assert.That(address.Domain, Is.EqualTo(1));
            Assert.That(address.Group, Is.EqualTo(2));
        }

        [Test]
        public void Address_MAC_IsTypeHardware()
        {
            var address = new Address(new byte[] {1, 2, 3, 4, 5});
            Assert.That(address.Type, Is.EqualTo(AddressType.Hardware));
            for (var i = 0; i < Address.MACLength; i++)
            {
                Assert.That(address[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void Address_String_IsTypeHardware()
        {
            var address = new Address("::10:20:30:40:50");
            Assert.That(address.Type == AddressType.Hardware);
            for (var i = 0; i < Address.MACLength; i++)
            {
                Assert.That(address[i] == i*16);
            }
        }

        [Test]
        public void Address_StringX_IsTypeNet()
        {
            var address = new Address("1.0x10.45");
            Assert.That(address.Type == AddressType.Net);
            Assert.That(address.Domain == 1);
            Assert.That(address.Subnet == 0x10);
            Assert.That(address.Device == 0x45);
        }
        
        [Test]
        public void Address_HexString_IsTypeNet()
        {
            var address = new Address("AaF1.10.4567");
            Assert.That(address.Type == AddressType.Net);
            Assert.That(address.Domain == 0xAAF1);
            Assert.That(address.Subnet == 0x10);
            Assert.That(address.Device == 0x4567);
        }

        [Test]
        public void Address_DecString_IsTypeNet()
        {
            var address = new Address("1.100.45");
            Assert.That(address.Type == AddressType.Net);
            Assert.That(address.Domain == 1);
            Assert.That(address.Subnet == 100);
            Assert.That(address.Device == 45);
        }

        [Test]
        public void Address_DecString_IsTypeGroup()
        {
            var address = new Address("1.2");
            Assert.That(address.Type == AddressType.Group);
            Assert.That(address.Domain == 1);
            Assert.That(address.Group == 2);
        }

        [Test]
        public void Value_IsNetString()
        {
            var address = new Address(1, 2, 3);
            Assert.That(address.Value == "1.2.3");
        }

        [Test]
        public void Value_IsGroupString()
        {
            var address = new Address(1, 2);
            Assert.That(address.Value == "1.2");
        }

        [Test]
        public void Value_IsMACString()
        {
            var address = new Address(new byte[] {10, 20, 30});
            Assert.That(address.Value == "::0A:14:1E");
            Assert.That(Address.Broadcast.Value == "FF:FF:FF:FF:FF:FF");
            Assert.That(Address.Empty.Value == "::0");
        }

        [Test]
        public void Equality()
        {
            var address11 = new Address(1, 2);
            var address12 = new Address(1, 2);
            var address21 = new Address(new byte[] {1, 2, 3, 4});
            var address22 = new Address(new byte[] {1, 2, 3, 4});
            Assert.That(address11, Is.EqualTo(address12));
            Assert.That(address12, Is.EqualTo(address11));
            Assert.That(address11, Is.EqualTo(address11));
            Assert.That(address21, Is.EqualTo(address22));
            Assert.That(address21, Is.Not.EqualTo(address11));
            Assert.That(address21, Is.Not.EqualTo(new byte[] {1, 2, 3, 4}));
            Assert.That(new Address(), Is.EqualTo(Address.Empty));
            Assert.That(new Address("FF:FF:FF:FF:FF:FF"), Is.EqualTo(Address.Broadcast));
        }

        [Test]
        public void BufferReadWrite(
            [ValueSource("Addresses")] Address address)
        {
            const int offset = 10;
            var buffer = new byte[offset + Address.Length];
            var count = address.Write(buffer, offset);
            Assert.That(count == Address.Length);
            Assert.That(address, Is.EqualTo(Address.ReadFrom(buffer, offset)));
        }

        public static IEnumerable<Address> Addresses()
        {
            return new[]
            {
                new Address(255, 255, 1),
                new Address(1, 2),
                new Address(new byte[] {0x10, 0x20, 0x30}),
                Address.Empty,
                Address.Broadcast
            };
        }

    }
}
