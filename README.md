<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&height=280&color=0:0F2027,50:203A43,100:2C5364&text=Real-Time%20Chat%20API&fontColor=ffffff&fontSize=48&fontAlignY=38&desc=ASP.NET%20Core%208%20|%20SignalR%20|%20Clean%20Architecture&descAlignY=58&animation=fadeIn"/>
</p>

<h1 align="center">💬 Real-Time Chat Application API</h1>

<h3 align="center">
Production-Ready Real-Time Communication Platform built with ASP.NET Core 8 & SignalR
</h3>

<p align="center">
<img src="https://readme-typing-svg.herokuapp.com?font=Fira+Code&weight=600&size=22&pause=1000&color=00E5FF&center=true&vCenter=true&width=800&lines=Real-Time+Messaging;SignalR+Powered;Private+Messaging;Group+Chat;Clean+Architecture;CQRS+%2B+MediatR;JWT+Authentication;Enterprise+Backend"/>
</p>

<p align="center">

<a href="https://github.com/ziadyasserdev/RealTimeChatApp">
<img src="https://img.shields.io/badge/Repository-181717?style=for-the-badge&logo=github&logoColor=white"/>
</a>

<a href="#">
<img src="https://img.shields.io/badge/API-Demo-00C853?style=for-the-badge"/>
</a>

<img src="https://img.shields.io/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>

<img src="https://img.shields.io/badge/SignalR-7A3FF2?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Clean-Architecture-blue?style=for-the-badge"/>

<img src="https://img.shields.io/badge/CQRS-MediatR-success?style=for-the-badge"/>

<img src="https://img.shields.io/badge/License-MIT-orange?style=for-the-badge"/>

</p>

---

# 💬 Overview

The **Real-Time Chat Application API** is a production-ready backend solution built with **ASP.NET Core 8** and **SignalR**, delivering instant communication through private messaging, group conversations, live notifications, online presence tracking, typing indicators, and message synchronization.

Designed using **Clean Architecture** and **CQRS**, the platform demonstrates enterprise-level backend engineering practices with a focus on scalability, maintainability, performance, and real-time communication.

The system supports advanced messaging workflows, secure authentication, role-based group management, and seamless synchronization across multiple connected devices.

---

# ✨ Features

## 💬 Private Messaging

- One-to-One Conversations
- Real-Time Message Delivery
- Message History
- Read Receipts
- Unread Messages
- Message Editing
- Message Deletion
- Message Pinning
- Multi-Device Synchronization

---

## 👥 Group Chats

- Public Groups
- Private Groups
- Group Invite Codes
- Join Requests
- Member Invitations
- Announcement Mode
- Group Information
- Member List
- Group Search

---

## 👑 Group Administration

Support for multiple group roles:

- Owner
- Administrator
- Member

Administrators can:

- Promote Members

- Demote Members

- Remove Members

- Mute Members

- Restore Members

- Manage Invite Codes

- Enable Announcement Mode

---

## 🟢 User Presence

Real-time presence system including:

- Online Status

- Offline Status

- Last Seen

- Active Connections

- Multiple Connected Devices

- Connection Tracking

- Automatic Disconnect Handling

---

## 📨 Messaging System

Powerful messaging capabilities:

- Send Messages

- Edit Messages

- Delete Messages

- Restore Messages

- Pin Messages

- Unpin Messages

- Forward Ready Architecture

- Conversation History

- Delivery Tracking

---

## 👀 Read Receipts

Track message lifecycle:

- Sent

- Delivered

- Read

- Seen Time

- Unread Counter

---

## ⌨️ Typing Indicators

Live typing events:

- User Started Typing

- User Stopped Typing

- Group Typing Notifications

- Private Chat Typing

---

## 🔔 Live Notifications

Receive real-time events for:

- New Messages

- Group Invitations

- Member Joined

- Member Left

- Promotion

- Demotion

- Mute Events

- Announcement Updates

- Message Pinning

---

## 🔒 Security

Enterprise-grade security:

- JWT Authentication

- ASP.NET Identity

- Role-Based Authorization

- Secure SignalR Connections

- Authorization Policies

- FluentValidation

---

## ⚡ Enterprise Features

Built using modern backend practices:

- Clean Architecture

- CQRS using MediatR

- Repository Pattern

- Unit of Work

- Result Pattern

- Dependency Injection

- Entity Framework Core

- SignalR

- RESTful APIs

- Transaction Management

- Real-Time Synchronization
- ---

# 🏗️ Architecture

The project follows **Clean Architecture** combined with **CQRS** and **SignalR**, ensuring scalability, maintainability, and real-time communication.

```text
                    ┌────────────────────────┐
                    │     Client Apps        │
                    │ Web • Mobile • Desktop │
                    └────────────┬───────────┘
                                 │
                      HTTP / SignalR
                                 │
                                 ▼
                    ┌────────────────────────┐
                    │   ASP.NET Core API     │
                    │     SignalR Hubs       │
                    └────────────┬───────────┘
                                 │
                 ┌───────────────┴────────────────┐
                 ▼                                ▼
        REST API Requests                Real-Time Events
                 │                                │
                 └───────────────┬────────────────┘
                                 ▼
                    ┌────────────────────────┐
                    │    Application Layer   │
                    │ CQRS • MediatR         │
                    │ DTOs • Validators      │
                    │ Business Logic         │
                    └────────────┬───────────┘
                                 ▼
                    ┌────────────────────────┐
                    │      Domain Layer      │
                    │ Entities • Interfaces  │
                    │ Domain Rules           │
                    └────────────┬───────────┘
                                 ▼
                    ┌────────────────────────┐
                    │ Infrastructure Layer   │
                    │ EF Core • Identity     │
                    │ SQL Server             │
                    │ SignalR Services       │
                    └────────────────────────┘
```

---

# ⚡ Real-Time Communication Flow

```text
User A
   │
   │ Send Message
   ▼
SignalR Hub
   │
   │ Validate Request
   ▼
Application Layer
   │
   │ Save Message
   ▼
Database
   │
   │
   ▼
SignalR Hub
   │
   │ Notify Receiver
   ▼
User B
   │
   │ Read Message
   ▼
Read Receipt Sent Back
```

---

# 🛠️ Tech Stack

## Programming Language

<p>

<img src="https://skillicons.dev/icons?i=cs"/>

</p>

---

## Backend Technologies

<p>

<img src="https://skillicons.dev/icons?i=dotnet"/>

<img src="https://skillicons.dev/icons?i=mysql"/>

</p>

<p>

<img src="https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge"/>

<img src="https://img.shields.io/badge/SignalR-7A3FF2?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Entity_Framework_Core-68217A?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Clean_Architecture-blue?style=for-the-badge"/>

<img src="https://img.shields.io/badge/CQRS-MediatR-success?style=for-the-badge"/>

<img src="https://img.shields.io/badge/JWT_Authentication-black?style=for-the-badge"/>

<img src="https://img.shields.io/badge/ASP.NET_Identity-purple?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Repository_Pattern-0A66C2?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Unit_of_Work-success?style=for-the-badge"/>

<img src="https://img.shields.io/badge/FluentValidation-green?style=for-the-badge"/>

<img src="https://img.shields.io/badge/AutoMapper-orange?style=for-the-badge"/>

</p>

---

## Tools

<p>

<img src="https://skillicons.dev/icons?i=git"/>

<img src="https://skillicons.dev/icons?i=github"/>

<img src="https://skillicons.dev/icons?i=visualstudio"/>

<img src="https://skillicons.dev/icons?i=postman"/>

</p>

---

# 📂 Project Structure

```text
RealTimeChatApp
│
├── RealTimeChatApp.API
│
├── RealTimeChatApp.Application
│
├── RealTimeChatApp.Domain
│
├── RealTimeChatApp.Infrastructure
│
└── RealTimeChatApp.Persistence
```

---

# 📦 Core Modules

| Module | Description |
|---------|-------------|
| 🔐 Authentication | Secure user registration, login, JWT authentication, refresh tokens, and role-based authorization using ASP.NET Identity. |
| 👤 Users | User profiles, online presence, connection tracking, last seen, and account management. |
| 💬 Private Chats | Real-time one-to-one messaging with delivery tracking, read receipts, editing, deletion, and message history. |
| 👥 Groups | Public/private groups, invite codes, announcements, invitations, member management, and secure access control. |
| 📨 Messages | Send, edit, delete, restore, pin, unpin, unread tracking, and conversation synchronization. |
| 🟢 Presence | Online/offline status, multiple device connections, reconnection handling, and live presence updates. |
| ⌨️ Typing Indicators | Real-time typing notifications for private and group conversations. |
| 👀 Read Receipts | Track message delivery, read status, unread counters, and read timestamps. |
| 🔔 Notifications | Instant notifications for messages, invitations, promotions, announcements, and moderation events. |
| 👑 Group Roles | Owner, Admin, and Member permissions with promotion, demotion, muting, and moderation capabilities. |
| 📊 Analytics | Chat statistics, group activity insights, message metrics, and user engagement reports. |

---

# 🚀 Platform Highlights

## ⚡ Real-Time Messaging

- Instant Message Delivery

- Read Receipts

- Typing Indicators

- Online Presence

- Message Synchronization

---

## 👥 Advanced Group Management

- Public & Private Groups

- Invite Codes

- Group Invitations

- Owner/Admin Permissions

- Member Moderation

- Announcement Mode

---

## 🟢 Presence System

Track users in real time:

- Online

- Offline

- Last Seen

- Active Connections

- Multiple Devices

---

## 🔒 Enterprise Security

- JWT Authentication

- ASP.NET Identity

- Authorization Policies

- Secure SignalR Connections

- FluentValidation

---

## 🏗️ Engineering Practices

- Clean Architecture

- CQRS with MediatR

- Repository Pattern

- Unit of Work

- Dependency Injection

- Result Pattern

- Entity Framework Core

- Transaction Management

---
---

# 🚀 Getting Started

Follow these steps to run the project locally.

## 1️⃣ Clone the Repository

```bash
git clone https://github.com/ziadyasserdev/RealTimeChatApp.git
```

```bash
cd RealTimeChatApp
```

---

## 2️⃣ Configure Database

Update your connection string inside:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=RealTimeChatDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

## 3️⃣ Configure JWT

```json
"JwtSettings": {
    "SecretKey": "YOUR_SECRET_KEY",
    "Issuer": "...",
    "Audience": "...",
    "ExpiryMinutes": 60
}
```

---

## 4️⃣ Apply Migrations

```bash
dotnet ef database update
```

---

## 5️⃣ Run the Project

```bash
dotnet run
```

---

## 6️⃣ Open Swagger

```text
https://localhost:xxxx/swagger
```

---

# 📡 API Modules

The API is organized into feature-rich modules designed for scalable real-time communication.

| Module | Description |
|---------|-------------|
| 🔐 Authentication | User registration, login, JWT authentication, refresh tokens, password management, and role-based authorization. |
| 👤 Users | User profiles, account management, online presence, connection tracking, and last seen status. |
| 💬 Private Chats | One-to-one conversations with instant delivery, message history, editing, deletion, unread tracking, and read receipts. |
| 📨 Messages | Send, edit, delete, restore, pin, unpin, search, and synchronize messages across multiple devices. |
| 👥 Groups | Public/private groups, invite codes, invitations, announcements, secure membership, and group lifecycle management. |
| 👑 Group Administration | Owner/Admin/Member roles with promotion, demotion, muting, member removal, invite management, and announcement mode. |
| 🟢 Presence | Live online status, offline tracking, multiple simultaneous connections, reconnection handling, and active session management. |
| ⌨️ Typing Indicators | Real-time typing notifications for private chats and group conversations. |
| 👀 Read Receipts | Track sent, delivered, and read messages with timestamps and unread counters. |
| 🔔 Notifications | Instant notifications for new messages, invitations, promotions, announcements, member activities, and moderation events. |
| 📊 Analytics | Conversation statistics, group insights, user activity, and messaging metrics. |

---

# 🔄 Message Lifecycle

```text
User A
   │
   │ Send Message
   ▼
SignalR Hub
   │
   ▼
Validate Request
   │
   ▼
Save Message
   │
   ▼
Notify Receiver
   │
   ▼
Delivered
   │
   ▼
Read
   │
   ▼
Read Receipt
```

---

# 👥 Group Workflow

```text
Create Group
      │
      ▼
Invite Members
      │
      ▼
Members Join
      │
      ▼
Owner / Admin Manage Members
      │
      ▼
Send Group Messages
      │
      ▼
Real-Time Synchronization
```

# 📈 Future Improvements

- Redis Backplane for SignalR Scaling

- Docker Support

- Kubernetes Deployment

- CI/CD Pipeline

- Unit Testing

- Integration Testing

- Push Notifications

- Media & File Sharing

- Voice Messages

- Video Calls

- End-to-End Encryption

- Message Reactions

- Message Search Indexing

---

# 🤝 Contributing

Contributions are always welcome.

If you'd like to improve the project:

1. Fork the repository

2. Create a feature branch

3. Commit your changes

4. Push your branch

5. Open a Pull Request

---

# 📄 License

This project is licensed under the MIT License.

---

# 👨‍💻 Author

## Zeyad Yasser

Backend .NET Developer

Passionate about building scalable backend systems using ASP.NET Core, SignalR, Clean Architecture, and modern software engineering principles.

<p align="center">

<a href="mailto:ziadyasser.dev@gmail.com">
<img src="https://img.shields.io/badge/Email-EA4335?style=for-the-badge&logo=gmail&logoColor=white"/>
</a>

<a href="https://www.linkedin.com/in/ziad-yasser-6155b828b">
<img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"/>
</a>

<a href="https://github.com/ziadyasserdev">
<img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white"/>
</a>

<a href="https://wa.me/201033724845">
<img src="https://img.shields.io/badge/WhatsApp-25D366?style=for-the-badge&logo=whatsapp&logoColor=white"/>
</a>

</p>

<p align="center">

📧 ziadyasser.dev@gmail.com &nbsp;|&nbsp;
📱 +20 103 372 4845 &nbsp;|&nbsp;
📍 Egypt

</p>

---

<p align="center">
<img src="https://capsule-render.vercel.app/api?type=waving&height=120&section=footer&color=0:0F2027,50:203A43,100:2C5364"/>
</p>

<h3 align="center">

⭐ If you found this project useful, don't forget to leave a star! ⭐

</h3>
