//
//  Plugin.mm
//  
//
//  Created by Tatsuki Nakajima on 2020/10/18.
//

#import <UnityFramework/UnityFramework-Swift.h>
//このコードはSwiftがコンパイルされると自動で定義されるヘッダファイル。

#ifdef __cplusplus
extern "C" {
#endif
   
//    Hello* hello_init() {
//        Hello *hello = [[Hello alloc] init];
//        CFRetain((CFTypeRef)hello);
//        return hello;
//    }

    void hhw() {
        NSLog(@"Hello, Happy World");
    }

//    void hhwSwift()
//    {
//        [Hello hhws];
//        //冒頭のヘッダファイルのおかげでSwiftで定義されたクラスとメソッドはこのように呼び出せる
//    }

//    void Number(Hello *hello)
//    {
//        [hello number];
//    }


//    int  GetTone()
//    {
//        return (int)[MIDIout toneInfo];
//    }

    int FindMIDI()
    {
        return (int)[MIDIout findMIDI];
    }

    bool GetMIDI()
    {
        return (bool)[MIDIout getMIDI];
    }

    uint MIDIinfo()
    {
        return (UInt8)[MIDIout midiInfo];
    }


#ifdef __cplusplus
}
#endif
