# AI-assisted development and code review

AI-generated code is a draft, not an authority. This checklist demonstrates a disciplined workflow for reviewing code produced with Codex or similar tools.

## Review checklist
- Correctness: trace business rules to requirements; test boundary values and invalid input.
- Security: check authorization, injection risks, data exposure, secrets, logging, and rate limiting.
- Maintainability: review naming, separation of concerns, duplication, error handling, and configuration.
- Data integrity: verify decimal precision, UTC timestamps, indexes, transaction boundaries, and concurrency behavior.
- Testing: run unit and integration tests; add failure-path and edge-case coverage.
- Human approval: document generated code, changes, and rationale. Never merge solely because an AI tool produced it.

## Known demo limitations
- Fraud indicators are transparent heuristic rules, not a validated predictive model.
- Authentication and role-based analyst authorization are not implemented.
- Demo startup uses EnsureCreated; production schema evolution should use reviewed EF Core migrations.
- Integration tests against a disposable SQL Server instance are a recommended next step.
