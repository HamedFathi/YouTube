# Whisper.cpp Installation and Setup Guide

This guide walks you through compiling and using whisper.cpp, a high-performance speech-to-text library that can leverage NVIDIA GPU acceleration.

## Prerequisites

Before starting, ensure you have the following tools installed:

### 1. Install Git
Download and install Git from the [official website](https://git-scm.com/).

Verify installation by running:
```bash
git --version
```

### 2. Install CMake
Download CMake from the [official website](https://cmake.org/download/).

Verify installation by running:
```bash
cmake --version
```

### 3. Install NVIDIA CUDA Toolkit (Optional but Recommended)
For GPU acceleration, install the CUDA Toolkit from [NVIDIA's developer site](https://developer.nvidia.com/cuda-downloads).

Verify installation by running:
```bash
nvcc --version
```

## Installation Steps

### 1. Clone the Repository
```bash
git clone https://github.com/ggml-org/whisper.cpp.git
cd whisper.cpp
```

### 2. Download AI Models
Navigate to the models directory and download the base English model:
```bash
cd models
.\download-ggml-model.cmd base.en
```

This will download the model file `ggml-base.en.bin` to the `whisper.cpp\models\` directory. Models are automatically downloaded from [Hugging Face](https://huggingface.co/ggerganov/whisper.cpp/tree/main).

### 3. Build the Project

#### For CPU-only Build:
```bash
cd ..
cmake -B build
cmake --build build --config Release
```

#### For GPU-accelerated Build (with CUDA):
```bash
cd ..
cmake -B build -DGGML_CUDA=1
cmake --build build --config Release
```

After building, the executable files will be located in `whisper.cpp\build\bin\Release\`.

## Usage

Navigate to the build directory and run the whisper CLI:
```bash
cd .\build\bin\Release\
```

### Usage Example
```bash
.\whisper-cli.exe -f "path\to\your\audio.wav" -m "path\to\model\ggml-base.en.bin" -of "path\to\output\directory" -osrt
```

## Command Line Parameters

- `-f`: Input audio file path
- `-m`: Model file path
- `-of`: Output file prefix/directory
- `-osrt`: Output in SRT subtitle format

## Notes

- The build process creates a `build` folder containing all compiled binaries
- GPU acceleration with CUDA significantly improves transcription speed
- Multiple model sizes are available (tiny, base, small, medium, large) with varying accuracy and speed trade-offs
- Supported audio formats include WAV, MP3, and other common formats

Your whisper.cpp installation is now ready for high-performance speech-to-text transcription!