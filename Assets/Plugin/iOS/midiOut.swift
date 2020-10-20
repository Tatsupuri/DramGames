//
//  midiOut.swift
//
//
//  Created by Tatsuki Nakajima on 2020/10/18.


import Foundation
import CoreMIDI

import os.log

@objc public class MIDIout : NSObject
{
    static var numberOfSources = 0
    static var sourceName:String?
    static var port = MIDIPortRef()
    static var client = MIDIClientRef()
    
    static let source = MIDIGetSource(0)
    
    static let portName = NSString("inputPort")
    static var tone:UInt8 = 0

    @objc public static func findMIDI() -> Int
    {
        numberOfSources = MIDIGetNumberOfSources()
        return numberOfSources
    }

    @objc public static func getMIDI() -> Bool
    {
        if numberOfSources == 1
        {
            let source = MIDIGetSource(0)
            var cfStr: Unmanaged<CFString>?
            
            
            if let str = cfStr?.takeRetainedValue() as String?
            {
                sourceName = str
                    
                client = MIDIClientRef()
                var err = MIDIClientCreateWithBlock(sourceName as! CFString, &client, onMIDIStatusChanged)
                    
                if err == noErr
                {
                    
                    port = MIDIPortRef()
                    err = MIDIInputPortCreateWithBlock(client, portName, &port, onMIDIMessageReceived)
                    if err == noErr
                    {
                        return true
                    }
                    
                    
                }
                    
                    
            }
        }
        return false
    }
    
    
    @objc public static func midiInfo() -> UInt8
    {
        var err = MIDIPortConnectSource(port, source, nil)
        
        if err == noErr
        {
            
            MIDIInputPortCreateWithBlock(client, portName, &port, onMIDIMessageReceived)
        
            return tone
        }
        
        
        return 255
    }
    
    
    static func onMIDIStatusChanged(message: UnsafePointer<MIDINotification>)
    {
        os_log("MIDI Status changed!")
    }
    
    static func onMIDIMessageReceived(message: UnsafePointer<MIDIPacketList>, srcConnRefCon: UnsafeMutableRawPointer?)
    {
        let packetList: MIDIPacketList = message.pointee
        let n = packetList.numPackets
        var packet = packetList.packet
        for _ in 0 ..< n
        {
            // Handle MIDIPacket
            let mes: UInt8 = packet.data.0 & 0xF0
            let ch: UInt8 = packet.data.0 & 0x0F
            let noteNo = packet.data.1
            let velocity = packet.data.2
            
            tone = noteNo
            
            let packetPtr = MIDIPacketNext(&packet)
            packet = packetPtr.pointee
        }
    }
    
}

