# Privacy and Personal Data

NuciCLI is a .NET library for building interactive command-line applications. It does not collect, process, transmit, or persist any personal data. This document describes the data-handling behaviour of the library itself.

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Data We Handle](#data-we-handle)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [Data Protection and Security](#data-protection-and-security)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how NuciCLI handles personal data. NuciCLI is a library that developers incorporate into their own applications; it does not operate as a standalone service and does not collect or process personal data on its own. Any personal data handled by an application using NuciCLI is the responsibility of that application's developer.

## 📥 Data We Handle

### Data Provided to the Application

NuciCLI does not request or receive any personal data from users, administrators, or connected systems.

### Data Generated or Collected by the Application

NuciCLI does not generate or collect any personal data, telemetry, crash reports, or usage analytics.

### Data Received from Integrations

NuciCLI has no built-in integrations and does not receive data from third parties.

## 🧭 Processing and Use

NuciCLI processes only the console input and output explicitly provided by the calling application. It does not process personal data for any purpose.

## 🗄️ Storage, Retention, and Deletion

NuciCLI does not store any data. All console interaction state exists only in memory during the execution of the calling application and is discarded when the application terminates.

## 🔗 External Processing and Integrations

NuciCLI has no built-in external data transfers, integrations, or third-party services.

## 🛡️ Data Protection and Security

NuciCLI does not handle personal data, so no specific data protection measures are required within the library. Applications using NuciCLI are responsible for implementing appropriate security controls for any personal data they process.

The original maintainers are not responsible for changes, modifications, or privacy issues introduced by downstream forks.

## 🔄 Document Changes

Update this document if NuciCLI's data-handling behaviour changes. The current version is published at [PRIVACY.md](./PRIVACY.md).

## 📬 Contact

For questions about NuciCLI's data handling, open an issue in the [NuciCLI repository](https://github.com/hmlendea/nucicli/issues).