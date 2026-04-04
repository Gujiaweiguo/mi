---
name: openspec-archive-change
description: Archive a completed change in the experimental workflow. Use when the user wants to finalize and archive a change after implementation is complete.
license: MIT
compatibility: Requires openspec CLI.
metadata:
  author: openspec
  version: "1.0"
  generatedBy: "1.1.1"
---

Archive a completed change in the experimental workflow.

**Input**: Optionally specify a change name. If omitted, check if it can be inferred from conversation context. If vague or ambiguous you MUST prompt for available changes.

**Steps**

1. **If no change name provided, prompt for selection**

   Run `openspec list --json` to get available changes. Use the **AskUserQuestion tool** to let the user select.

   Show only active changes (not already archived).
   Include the schema used for each change if available.

   **IMPORTANT**: Do NOT guess or auto-select a change. Always let the user choose.

2. **Check artifact completion status**

   Run `openspec status --change "<name>" --json` to check artifact completion.

   Parse the JSON to understand:
   - `schemaName`: The workflow being used
   - `artifacts`: List of artifacts with their status (`done` or other)

   **If any artifacts are not `done`:**
   - Display warning listing incomplete artifacts
   - Use **AskUserQuestion tool** to confirm user wants to proceed
   - Proceed if user confirms

3. **Check task completion status**

   Read the tasks file (typically `tasks.md`) to check for incomplete tasks.

   Count tasks marked with `- [ ]` (incomplete) vs `- [x]` (complete).

   **If incomplete tasks found:**
   - Display warning showing count of incomplete tasks
   - Use **AskUserQuestion tool** to confirm user wants to proceed
   - Proceed if user confirms

   **If no tasks file exists:** Proceed without task-related warning.

4. **Verify archive test gates (HARD BLOCK)**

   Before any sync or archive step, determine the current HEAD commit SHA and check for machine-readable evidence files for that exact commit.

   Read the test evidence contract from:
   - `openspec/changes/<name>/test-evidence-contract.md` when present
   - otherwise use the default convention:
     - `artifacts/verification/<commit-sha>/unit.json`
     - `artifacts/verification/<commit-sha>/integration.json`
     - `artifacts/verification/<commit-sha>/e2e.json`

   For each of the three evidence files:
   - Require the file to exist
   - Parse JSON successfully
   - Require fields: `schema_version`, `project`, `change`, `commit_sha`, `test_type`, `status`, `started_at`, `finished_at`, `source`, `stats`
   - Require `commit_sha` to equal the current HEAD commit SHA
   - Require `test_type` to match the expected file
   - Require `status` to be `passed`

   **Archive gate rule:**
   - Archive requires passing `unit`, `integration`, and `e2e` evidence for the current commit.

   **If any required evidence is missing, stale, malformed, or failed:**
   - STOP immediately
   - Do NOT ask for override confirmation
   - Report archive as blocked

5. **Assess delta spec sync state**

   Check for delta specs at `openspec/changes/<name>/specs/`. If none exist, proceed without sync prompt.

   **If delta specs exist:**
   - Compare each delta spec with its corresponding main spec at `openspec/specs/<capability>/spec.md`
   - Determine what changes would be applied (adds, modifications, removals, renames)
   - Show a combined summary before prompting

   **Prompt options:**
   - If changes needed: "Sync now (recommended)", "Archive without syncing"
   - If already synced: "Archive now", "Sync anyway", "Cancel"

   If user chooses sync, execute /opsx-sync logic (use the openspec-sync-specs skill). Proceed to archive regardless of choice.

6. **Perform the archive**

   Create the archive directory if it doesn't exist:
   ```bash
   mkdir -p openspec/changes/archive
   ```

   Generate target name using current date: `YYYY-MM-DD-<change-name>`

   **Check if target already exists:**
   - If yes: Fail with error, suggest renaming existing archive or using different date
   - If no: Move the change directory to archive

   ```bash
   mv openspec/changes/<name> openspec/changes/archive/YYYY-MM-DD-<name>
   ```

7. **Display summary**

   Show archive completion summary including:
   - Change name
   - Schema that was used
   - Archive location
   - Whether specs were synced (if applicable)
   - Note about any warnings (incomplete artifacts/tasks)

**Output On Success**

```
## Archive Complete

**Change:** <change-name>
**Schema:** <schema-name>
**Archived to:** openspec/changes/archive/YYYY-MM-DD-<name>/
**Specs:** ✓ Synced to main specs (or "No delta specs" or "Sync skipped")

All artifacts complete. All tasks complete.
Archive gate passed: unit + integration + e2e evidence matched current commit.
```

**Output On Error (Test Gate Failed)**

```
## Archive Blocked

**Change:** <change-name>
**Reason:** Archive requires passing unit, integration, and e2e evidence for the current HEAD commit.

**Failed Checks:**
- unit: missing / stale / malformed / failed
- integration: missing / stale / malformed / failed
- e2e: missing / stale / malformed / failed

Resolve the missing evidence and run verification again before archiving.
```

**Guardrails**
- Always prompt for change selection if not provided
- Use artifact graph (openspec status --json) for completion checking
- Never allow archive to proceed without passing current-commit unit + integration + e2e evidence
- Preserve .openspec.yaml when moving to archive (it moves with the directory)
- Show clear summary of what happened
- If sync is requested, use openspec-sync-specs approach (agent-driven)
- If delta specs exist, always run the sync assessment and show the combined summary before prompting
