import sys
import clr
import array
sys.path.append(r"C:\SCOREBOARDS\2012\nibus.net\libnibus\bin\Debug")
clr.AddReference("libnibus.dll")
clr.AddReference("AsyncCtpLibrary.dll")
clr.AddReference("System.Threading.Tasks.Dataflow.dll")
clr.AddReference("NLog.dll")
import NataInfo.Nibus
from NataInfo.Nibus import *
stack = NataInfo.Nibus.NibusStack(NataInfo.Nibus.SerialTransport("COM7", 115200, True), NataInfo.Nibus.NibusDataCodec(), NataInfo.Nibus.Nms.NmsCodec())
try:
    protocol = stack.GetCodec[NataInfo.Nibus.Nms.NmsCodec]().Protocol
    print protocol.Ping(NataInfo.Nibus.Address("::20:44"))
finally:
    stack.Dispose()

