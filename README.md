### Banking & Insurance Data Engineering Solution (Fraud Detection)

An end-to-end data engineering pipeline designed to ingest, clean, and analyze banking transactions and insurance claims to flag high-risk fraudulent behavior using Python (Pandas) and MySQL.

### 🚀 Business Problem Solved

Financial fraud costs institutions billions annually. This project solves that problem by automating the detection of anomalous patterns across banking accounts and insurance policies.

The pipeline identifies key fraud indicators such as:

*   Multiple insurance claims filed immediately after a policy opens.
*   Sudden, high-volume transactions on historically inactive bank accounts.
*   Mismatched customer profile information across banking and insurance databases.

### 🛠️ Tech Stack

*   **Language:** Python 3.x
*   **Data Manipulation (ETL):** Pandas
*   **Database & Analytics Warehouse:** MySQL

### 🏗️ Data Pipeline Architecture

1.  **Ingestion & Extraction:** Python scripts ingest raw transaction logs and insurance claim feeds.
2.  **Pandas Transformation:**
    *   Cleaned structural anomalies, formatted timestamps, and dropped duplicate transaction IDs.
    *   Engineered risk-scoring flags based on transaction velocity and claim amounts.
3.  **Database Loading:** Structured DataFrames are loaded into a relational MySQL schema optimized for indexing.
4.  **MySQL Analytics:** Stored procedures and complex SQL queries isolate suspicious accounts for fraud investigation teams.

### 💻 Included Files

*   `Capstone_Banking_Dhanalakshmi_Subramanian.ipynb`: The complete Python pipeline notebook utilizing Pandas for data cleaning, transformation, and structural manipulation.
