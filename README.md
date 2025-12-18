# 🪟 Windows System Widget

A lightweight Windows system widget designed to monitor **RAM usage** and **available disk space** in real time.

This project was built as a hands-on exercise to explore **system-level data**, resource monitoring, and the development of small but useful desktop utilities.  
The focus is on **functionality, clarity, and performance**, rather than visual complexity or overengineering.

---

## 📦 Technologies

- JavaScript
- Windows system APIs
- Desktop environment utilities

*(The stack was intentionally kept simple to better understand how system information is retrieved and displayed.)*

---

## 🦄 Features

- 📊 **Real-time RAM monitoring**  
  Displays current memory usage with immediate updates.

- 💾 **Available disk space tracking**  
  Shows how much disk space is free on the system.

- 🪶 **Lightweight widget**  
  Minimal impact on system resources.

- 🧩 **Simple and clear UI**  
  Designed to be readable at a glance without distractions.

---

## 👩🏽‍🍳 The Process

The project started with researching how to retrieve reliable system information from Windows, focusing on memory and storage metrics.

Once the data source was identified, I worked on:
- Fetching system values safely and efficiently
- Updating the UI in real time
- Keeping the widget lightweight and responsive
- Structuring the code so it could be easily extended in the future

The goal was not to build a complex application, but a **useful and understandable tool** that solves a specific problem well.

---

## 📚 What I Learned

### 🧠 System Monitoring
- How RAM usage and disk space are calculated and exposed by the operating system
- Differences between raw system values and user-friendly metrics

### 🧱 Desktop Utility Design
- How to design small tools with a clear and focused purpose
- Balancing functionality and simplicity

### 🧩 Code Structure
- Writing maintainable code for system-related features
- Keeping logic separated from presentation

---

## 💭 How Can It Be Improved?

- Add CPU usage monitoring
- Add refresh rate customization
- Support multiple disks
- Add visual indicators or charts
- Improve UI customization options
- Package it as an installable executable

---

## 🚦 Running the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/Thecodepunisher/WindowsSystemWidget.git
