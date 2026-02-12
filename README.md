# 🪟 Windows System Widget
A lightweight desktop widget that displays real-time RAM usage and available disk space on Windows. Built with JavaScript and Windows system APIs, this project helped me understand how to interact with system-level data and create useful desktop utilities. I built this side project for learning purposes and to explore system monitoring concepts.

## 📦 Technologies
- JavaScript
- Windows system APIs
- Desktop environment utilities

## 🦄 Features
Here's what you can do with Windows System Widget:

**Real-Time RAM Monitoring**: See your current memory usage with instant updates. The widget continuously monitors your system's RAM consumption and displays it in an easy-to-read format.

**Available Disk Space Tracking**: Know exactly how much storage space you have left. The widget shows free disk space on your system drive, helping you avoid running out of storage unexpectedly.

**Lightweight Performance**: The widget runs efficiently in the background with minimal impact on system resources. It's designed to be unobtrusive while providing valuable information at a glance.

**Simple, Clear Interface**: A clean UI designed for quick readability without distractions. All the information you need is presented clearly, making it easy to monitor your system status without cognitive overhead.

**Always-On-Top Option**: Keep the widget visible while working in other applications. Perfect for monitoring system resources during resource-intensive tasks like video editing or gaming.

**Minimal Configuration**: Works out of the box with sensible defaults. No complex setup required – just run it and start monitoring your system.

## 🎯 Keyboard Shortcuts
Speed up your workflow with these shortcuts:

- **Ctrl+R**: Refresh system data manually
- **Ctrl+H**: Hide/show the widget
- **Ctrl+Q**: Quit the application

## 👩🏽‍🍳 The Process
I started this project because I wanted a simple, always-visible way to monitor my system resources without opening Task Manager constantly. The goal was to create something minimal and useful that sits quietly in the corner of my screen.

First, I researched how to access Windows system information reliably. I needed to find APIs that could provide accurate RAM and disk space metrics without requiring administrative privileges or causing security warnings.

Once I identified the right system calls, I focused on building a clean display. The challenge was making the information readable at a glance – I experimented with different layouts, font sizes, and update intervals to find the perfect balance between usefulness and unobtrusiveness.

I also had to think about performance. Since this widget would run continuously, I needed to ensure it didn't consume significant system resources itself. I implemented efficient update mechanisms and optimized the refresh cycle to minimize CPU usage.

The UI went through several iterations. Initially, it was too bare-bones and looked unpolished. I added subtle styling, proper typography, and visual indicators to make it feel more like a professional system tool rather than a quick hack.

Throughout development, I kept the scope intentionally limited. Instead of adding every possible metric (CPU usage, network stats, etc.), I focused on doing two things really well: RAM monitoring and disk space tracking. This discipline helped me finish the project and learn deeply about those specific features.

## 📚 What I Learned
During this project, I've picked up important skills and a deeper understanding of system-level programming and desktop application development.

### 🧠 System Monitoring Fundamentals
**Windows System APIs**: I learned how to safely access system information through Windows APIs. Understanding the difference between raw system values (bytes, sectors) and user-friendly metrics (GB, percentages) taught me about data transformation and presentation.

**Memory Management Concepts**: Diving into RAM monitoring deepened my understanding of how operating systems manage memory – total RAM, used RAM, available RAM, and the nuances between them. I learned why "available" memory isn't just "total minus used."

### 🖥️ Desktop Application Design
**Always-On Widgets**: Building a widget that runs continuously taught me about application lifecycle management. I learned how to handle startup, shutdown, and persistence without being intrusive to the user's workflow.

**Resource Efficiency**: Creating software that monitors resources while being resource-efficient itself was an interesting challenge. I learned about polling intervals, event-driven updates, and how to minimize CPU cycles.

### 📊 Real-Time Data Visualization
**Update Strategies**: I learned when to use polling vs. event-driven updates. For system metrics that change frequently, finding the right refresh rate was crucial – too fast wastes resources, too slow provides stale data.

**Data Formatting**: Converting raw bytes into human-readable formats taught me about units of measurement, significant figures, and how to present numerical data clearly.

### 🧱 Code Architecture
**Separation of Concerns**: I structured the code to separate data fetching, data processing, and UI presentation. This made the codebase easier to maintain and test each component independently.

**Error Handling**: Working with system APIs taught me about graceful degradation. What happens if a drive becomes unavailable? How do you handle permission issues? These edge cases made the application more robust.

### 🎨 UI/UX for Utility Apps
**Clarity Over Aesthetics**: I learned that for system utilities, function trumps form. The most beautiful design is worthless if users can't quickly parse the information they need.

**Visual Hierarchy**: Presenting two different metrics (RAM and disk space) taught me about establishing visual hierarchy – making it instantly clear which number represents what without requiring labels.

### 📈 Overall Growth
This project taught me that great software doesn't have to be complex. By focusing on a narrow use case and executing it well, I created something genuinely useful that I use daily.

Building a system monitoring tool gave me insights into how operating systems expose information to applications. It demystified concepts I'd only read about in theory – seeing real RAM usage numbers update in real-time connected abstract OS concepts to concrete reality.

Most importantly, I learned about intentional limitation. I could have added a dozen more features, but by keeping the scope tight, I actually finished the project and made something polished. Sometimes what you don't build is as important as what you do build.

## 💭 How can it be improved?
- Add CPU usage monitoring with history graph
- Add refresh rate customization in settings
- Support multiple disk drives with individual displays
- Add visual indicators or charts for historical data
- Improve UI with themes and customization options
- Package it as an installable executable (.exe)
- Add notification alerts when RAM or disk space reaches critical levels
- Support for network usage statistics
- Create mini-mode for even smaller footprint
- Add ability to export system stats to CSV for analysis

## 🚦 Running the Project
To run the project in your local environment, follow these steps:

1. Clone the repository to your local machine
2. Ensure you have Node.js installed on your Windows system
3. Run `npm install` in the project directory to install the required dependencies
4. Run `npm start` to launch the widget
5. The widget will appear on your screen showing real-time system metrics

## 🎥 Preview
The widget displays as a small, always-on-top window showing current RAM usage (e.g., "8.2 GB / 16 GB") and available disk space (e.g., "245 GB free"). The information updates automatically in real-time, providing instant visibility into your system's resource consumption.
