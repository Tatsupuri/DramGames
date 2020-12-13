    #import <Foundation/Foundation.h>
    #import <CoreMIDI/CoreMIDI.h>

    #ifdef __cplusplus
    extern "C" {
    #endif


    MIDINotifyBlock onMIDIStatusChanged = ^(const MIDINotification *message)
    {
        NSLog(@"MIDI status changed");
    };

    MIDIReadBlock onMIDIMessageReceived = ^(const MIDIPacketList *pktlist, void *srcConnRefCon)
    {
        NSLog(@"MIDI message recieved");
        
        //MIDIパケットリストの先頭のMIDIPacketのポインタを取得
        MIDIPacket *packet = (MIDIPacket *)&(pktlist->packet[0]);
        //パケットリストからパケットの数を取得
        UInt32 packetCount = pktlist->numPackets;
        
        for (NSInteger i = 0; i < packetCount; i++)
        {
            //data[0]からメッセージの種類とチャンネルを分けて取得する
            //Byte mes = packet->data[0] & 0xF0;
            //Byte ch = packet->data[0] & 0x0F;
            
            Byte mes = packet->data[0];
            Byte note = packet->data[1];
            
            NSString *str = [NSString stringWithFormat:@"%d", note];
            
            
            if((mes == 0x99) && (packet->data[2] != 0))
            {
                //note on
                UnitySendMessage("bar", "NoteJudge", [str UTF8String]);
            }
    //        else if(mes == 0x89 || mes == 0x99)
    //        {
    //            //note off
    //            //UnitySendMessage("bar", "NoteJudge", "0");
    //        }
            
            //次のパケットへ進む
            packet = MIDIPacketNext(packet);
        }
    };


    bool GetMIDI()
    {
        if (MIDIGetNumberOfSources() == 1)
        {
            
            MIDIEndpointRef source = MIDIGetSource(0);

            OSStatus err;
            CFStringRef strEndPointRef = NULL;

            err = MIDIObjectGetStringProperty(source, kMIDIPropertyName, &strEndPointRef);

            if (err == noErr)
            {

                MIDIClientRef client = MIDIClientRef();

                err = MIDIClientCreateWithBlock(strEndPointRef, &client,onMIDIStatusChanged);

                if(err == noErr)
                {
                    NSLog(@"MIDIClient created");

                    CFStringRef portName = (CFStringRef)@"inputPort";
                    MIDIPortRef port = MIDIPortRef();

                    err = MIDIInputPortCreateWithBlock(client, portName, &port, onMIDIMessageReceived);

                    if(err == noErr)
                    {
                            NSLog(@"MIDIInputPort created");
                            err = MIDIPortConnectSource(port, source, nil);

                            if(err == noErr)
                            {
                                NSLog(@"MIDIEndpoint connected to InputPort");
                                return true;
                            }
                    }

                }

            }
        }
        return false;
    }

    #ifdef __cplusplus
    }
    #endif
