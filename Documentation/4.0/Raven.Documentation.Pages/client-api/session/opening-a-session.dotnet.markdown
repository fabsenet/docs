# Session : Opening a Session

To open synchronous session use the `OpenSession` method from `DocumentStore` or `OpenAsyncSession` if you prefer working in an asynchronous manner.

## Syntax

There are three overloads of `OpenSession / OpenAsyncSession` methods

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_1_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

The first method is an equivalent of doing

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_2@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_2_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

The second method is an equivalent of doing

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_3@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_3_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | [OpenSessionOptions](../../glossary/open-session-options) | Options **containing** information such as **name of database** and **RequestExecutor**. |

| Return Value | |
| ------------- | ----- |
| IDocumentSession / IAsyncDocumentSession | Instance of a session object. |

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_4@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_5@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}


{DANGER:Important}
**Always remember to release session allocated resources after usage by invoking the `Dispose` method or wrapping the session object in the `using` statement.**
{DANGER/}

## Related Articles

- [What is a session and how does it work?](../../client-api/session/what-is-a-session-and-how-does-it-work)  
