using System;
using GalaxyBudsClient.Generated.Model.Attributes;

namespace GalaxyBudsClient.Message.Encoder;

[MessageEncoder(MsgIds.UPDATE_TIME)]
public class UpdateTimeEncoder : BaseMessageEncoder
{
    public long Timestamp { get; set; } = -1;
    public int Offset { get; set; } = -1;
    
    public override SppMessage Encode()
    {
        if (Timestamp < 0 || Offset < 0)
        {
            var span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Timestamp = (long)span.TotalMilliseconds;
            Offset = (int)TimeZoneInfo.Local.BaseUtcOffset.TotalMilliseconds;
        }
    
        var payload = new byte[12];
        var timestampRaw = BitConverter.GetBytes(Timestamp);
        var offsetRaw = BitConverter.GetBytes(Offset);
        Array.Copy(timestampRaw, 0, payload, 0, 8);
        Array.Copy(payload, 8, offsetRaw, 0, 4);
        
        return new SppMessage(MsgIds.UPDATE_TIME, MsgTypes.Request, payload);
    }
}