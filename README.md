# Assumptions Made
## Specification
- Summary endpoint should give "total value of open and closed invoices"
    - It is assumed that this is a typo and should be "open and closed receivables" as the example payload gives no way to differentiate between a credit note and an invoice

## References
- "Reference" is a unique ID for the receivable
    - If the reference already exists in the database, the following properties are updated:
        - Paid Value
        - Closed Date (if provided)
        - Cancelled
- "Debtor Reference" is a unique ID for the debtor

## Debtors
- If 2 receivables have the same debtor reference, the debtor is the same and existing information should be used

## Cancelled Receivables
- If a receivable isn't explicitly marked as cancelled, it is not cancelled
    - A cancelled receivable is always considered closed


## Closed Receivables
- A cancelled receivable is always considered closed
- If a closed date is set, the receivable is always considered closed
- If the paid value is equal to the opening value, the receivable is always considered closed
    - Unless closed for another reason, if paid > opening, the receivable is not closed as further action may be required

## Values
- The value of an open receivable is the amount left unpaid (i.e. opening value - paid value)
- The value of a closed receivable is the amount collected (i.e. paid value)


## Data Validation
### Data Types
- **Dates** - All dates conform to ISO 8601 in the format YYYY-MM-DD (with or without '-' seperator)
- **Currencies** - All currencies stored in as decimal as per [Microsoft Recommendation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)

#### Payload Validation
- **Reference** - Unique, further POSTS with same reference updates the value
- **Currency Code** - 3 letter code conforming to ISO 4217
- **Issue Date** - Valid Date either today or in past
- **Opening Value** - Positive decimal
- **Paid Value** - 0 or Positive decimal
- **Due Date** - Valid Date
- **Closed Date** - Valid Date either today or in past
- **Debtor Country Code** - ISO 3166-1 Alpha 2 code

#### Example Payload
``` json
[
  {
    "reference": "string",
    "currencyCode": "string",
    "issueDate": "string",
    "openingValue": 1234.56,
    "paidValue": 1234.56,
    "dueDate": "string",
    "closedDate": "string", //optional
    "cancelled": true|false, //optional
    "debtorName": "string",
    "debtorReference": "string",
    "debtorAddress1": "string", //optional
    "debtorAddress2": "string", //optional
    "debtorTown": "string", //optional
    "debtorState": "string", //optional
    "debtorZip": "string", //optional
    "debtorCountryCode": "string", 
    "debtorRegistrationNumber": "string" //optional
  }
]
```

# Time Taken
- **Research/Planning** ~0.5 hours
- **Creating Data model/DB for test** ~0.5 hours
- **Understanding end to end testing in ASP.NET Core** ~0.5 hours
- **Storing Receivables** ~2 hours
- **Receivable Summary** ~0.5 hours

# Reflection
With more time, I would have liked to:
- Use external services for currency/country codes, and currency conversion
- Added feedback if the validation failed for the post receivables method
- Put better logic in around debtors
    - The assumption was changed from "use the latest receivable's debtor information for all debtors with the same reference" to its current form to save time as I was already going over
- Use the swagger decorators to better document the API
- Improve logging across the whole system
    - Some logging was added around the file loading, but ideally the entire system should have it