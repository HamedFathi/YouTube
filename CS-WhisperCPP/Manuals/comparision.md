# whisper.cpp (whisper-cli) vs whisper.net

- **https://github.com/ggml-org/whisper.cpp**
- **https://github.com/sandrohanea/whisper.net**

## Pros of whisper-cli
1. whisper-cli gives better results because it uses up-to-date implementations and includes the latest features and fixes.
2. whisper-cli shows more accurate timestamps.
3. whisper-cli supports more input formats like mp3, while whisper.net only supports wav (you must convert others with NAudio).
4. whisper-cli supports more output formats like vtt, srt, and lrc; whisper.net needs custom formatting based on SegmentData class.

## Pros of whisper.net
1. whisper.net is 2–3 times faster than whisper-cli.
2. whisper.net feels more natural and integrated for C# developers and no need a C++ compilation process.

---

**Hello.wav - Song by Adele 2015**

**Model: ggml-base.en.bin**

## whisper-cli (srt)

```
1
00:00:00,000 --> 00:00:02,500
 (piano music)

2
00:00:02,500 --> 00:00:11,340
 ♪ Hello, it's me ♪

3
00:00:11,340 --> 00:00:17,880
 ♪ I was wondering if after all these years you'd like to meet ♪

4
00:00:17,880 --> 00:00:22,880
 ♪ To go over everything ♪
```

## whisper.net (raw format)

```
00:00:00->00:00:06:  [Music]
00:00:06->00:00:12:  Hello, it's me.
00:00:12->00:00:16:  I was wondering if after all these years
00:00:16->00:00:20:  you'd like to me to go over
00:00:20->00:00:23:  everything.
```
