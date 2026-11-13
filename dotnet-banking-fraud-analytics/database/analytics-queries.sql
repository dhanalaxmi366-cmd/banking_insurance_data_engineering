-- Descriptive: transaction volume and value by calendar day.
SELECT CAST(OccurredAtUtc AS date) AS TransactionDate, COUNT(*) AS TransactionCount, SUM(Amount) AS TotalAmount
FROM dbo.BankTransactions
GROUP BY CAST(OccurredAtUtc AS date)
ORDER BY TransactionDate;

-- Diagnostic: accounts with the highest share of flagged transactions.
WITH AccountRisk AS (
    SELECT AccountId, COUNT(*) AS TransactionCount,
           SUM(CASE WHEN RiskScore >= 50 THEN 1 ELSE 0 END) AS FlaggedCount
    FROM dbo.BankTransactions GROUP BY AccountId
)
SELECT AccountId, TransactionCount, FlaggedCount,
       CAST(100.0 * FlaggedCount / NULLIF(TransactionCount, 0) AS decimal(6,2)) AS FlaggedPercent
FROM AccountRisk ORDER BY FlaggedPercent DESC, FlaggedCount DESC;

-- Window function: compare each transaction with its predecessor for the account.
WITH OrderedTransactions AS (
    SELECT AccountId, Id, Amount, OccurredAtUtc,
           LAG(OccurredAtUtc) OVER (PARTITION BY AccountId ORDER BY OccurredAtUtc) AS PreviousTransactionUtc
    FROM dbo.BankTransactions
)
SELECT AccountId, Id, Amount, OccurredAtUtc, PreviousTransactionUtc,
       DATEDIFF(MINUTE, PreviousTransactionUtc, OccurredAtUtc) AS MinutesSincePrevious
FROM OrderedTransactions
WHERE PreviousTransactionUtc IS NOT NULL
ORDER BY AccountId, OccurredAtUtc;
