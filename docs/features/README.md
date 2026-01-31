# Feature Documentation

This directory contains specifications, implementation plans, and task lists for planned features.

## Document Structure

Each feature folder should contain:
- `spec.md` - Requirements and functional specification
- `plan.md` - Implementation approach and phases
- `tasks.md` - Detailed task checklist

## Creating a New Feature

1. Create a folder: `NN-feature-name/` (NN = priority number)
2. Create the three documents using templates below
3. Add entry to the Features Overview table

---

## Features Overview

| Priority | # | Feature | Description | Status |
|:--------:|---|---------|-------------|--------|
| 1 | [#1](https://github.com/StreckerCM/GeoMagSharpGUI/issues/1) | [WMM Format Support](01-wmm-format-support/) | Support WMM2020+ coefficient file format | Testing |
| 2 | - | [MVVM Architecture](02-mvvm-architecture/) | Refactor to Model-View-ViewModel pattern | Ready |

---

## Potential Features

Ideas for future development:

### High Priority
- **Unit Test Coverage** - Expand MSTest coverage for Calculator and ModelReader
- **Model Auto-Update** - Check for new coefficient file versions online
- **Export Formats** - CSV/Excel export in addition to text files
- ~~**MVVM Architecture** - Refactor to proper Model-View-ViewModel pattern~~ (see #2 above)

### Medium Priority
- **Batch Processing** - Calculate for multiple locations at once
- **Map Integration** - Visual map for coordinate selection
- **Date Range Plotting** - Graph magnetic values over time

### Lower Priority
- **Localization** - Multi-language support
- **Dark Mode** - Alternative UI theme
- **Command Line Interface** - Non-GUI calculation mode

---

## Template: spec.md

```markdown
# Feature Name - Specification

## Overview
[Brief description of the feature]

## User Story
As a [user type], I want [goal] so that [benefit].

## Requirements

### Functional Requirements
1. [Requirement 1]
2. [Requirement 2]

### Non-Functional Requirements
1. [Performance, security, etc.]

## Acceptance Criteria
- [ ] [Criterion 1]
- [ ] [Criterion 2]

## Dependencies
- [Dependencies on other features/libraries]

## Open Questions
- [Questions that need answers before implementation]
```

---

## Template: plan.md

```markdown
# Feature Name - Implementation Plan

## Overview
[Summary of implementation approach]

## Phase 1: [Phase Name]
### Goals
- [Goal 1]
- [Goal 2]

### Tasks
1. [Task 1]
2. [Task 2]

### Files Affected
- `path/to/file.cs`

## Phase 2: [Phase Name]
[Continue pattern...]

## Testing Strategy
[How to test this feature]

## Rollback Plan
[How to revert if issues arise]
```

---

## Template: tasks.md

```markdown
# Feature Name - Task List

## Phase 1: [Phase Name]
- [ ] Task 1
- [ ] Task 2
- [ ] Task 3

## Phase 2: [Phase Name]
- [ ] Task 4
- [ ] Task 5

## Verification
- [ ] Build succeeds
- [ ] Tests pass
- [ ] Manual testing complete

## Documentation
- [ ] AGENTS.md updated if needed
- [ ] README.md updated if needed
- [ ] Code comments added
```

---

## Status Definitions

| Status | Meaning |
|--------|---------|
| **Proposed** | Initial idea, needs specification |
| **Specifying** | Specification in progress |
| **Ready** | Fully specified, ready for implementation |
| **In Progress** | Currently being implemented |
| **Testing** | Implementation complete, under test |
| **Complete** | Merged and released |
| **Has Questions** | Blocked by open questions |
